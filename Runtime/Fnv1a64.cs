using System;
using System.Buffers;
using System.Text;

namespace RPGFramework.Hashing
{
    /// <summary>
    /// FNV-1a 64-bit hashing, mostly used in the localisation package.
    /// </summary>
    /// <remarks>
    /// <para>
    /// Strings are hashed as UTF-8. A null string or null array denotes an empty sequence and
    /// hashes identically to an empty one, matching how .NET already defines
    /// <c>((string)null).AsSpan()</c>. Whitespace is ordinary input.
    /// Reject null, empty or whitespace keys before hashing if they are invalid for your data.
    /// </para>
    /// <para>
    /// Invalid UTF-16 in the input (an unpaired surrogate) is encoded as the Unicode replacement
    /// character rather than throwing, so two differently-malformed strings can hash alike.
    /// </para>
    /// </remarks>
    public static class Fnv1a64
    {
        private const ulong FNV_OFFSET_BASIS = 14695981039346656037UL;
        private const ulong FNV_PRIME        = 1099511628211UL;

        // Input up to this many characters is encoded on the stack; anything longer uses a pooled
        // buffer. UTF-8 needs at most three bytes per UTF-16 char, so a buffer three times the
        // character limit can never overflow.
        private const int STACK_BUFFER_MAX_CHARS = 85;
        private const int STACK_BUFFER_SIZE      = STACK_BUFFER_MAX_CHARS * 3;

        private static readonly Encoding m_Utf8 = new UTF8Encoding(false, false);

        /// <summary>
        /// Computes an FNV-1a 64-bit hash over the UTF-8 representation of a string.
        /// A null string is treated as empty.
        /// </summary>
        public static ulong Hash(string input)
        {
            return Hash(input.AsSpan());
        }

        /// <summary>
        /// Computes an FNV-1a 64-bit hash over the UTF-8 representation of characters.
        /// This overload avoids allocation for short input and uses a pooled buffer for larger input.
        /// </summary>
        public static ulong Hash(ReadOnlySpan<char> input)
        {
            if (input.Length <= STACK_BUFFER_MAX_CHARS)
            {
                Span<byte> buffer  = stackalloc byte[STACK_BUFFER_SIZE];
                int        written = m_Utf8.GetBytes(input, buffer);

                return Hash(buffer[..written]);
            }

            int    byteCount = m_Utf8.GetByteCount(input);
            byte[] rented    = ArrayPool<byte>.Shared.Rent(byteCount);
            try
            {
                int written = m_Utf8.GetBytes(input, rented.AsSpan(0, byteCount));

                return Hash(rented.AsSpan(0, written));
            }
            finally
            {
                ArrayPool<byte>.Shared.Return(rented);
            }
        }

        /// <summary>
        /// Computes an FNV-1a 64-bit hash over bytes. A null array is treated as empty.
        /// </summary>
        public static ulong Hash(byte[] bytes)
        {
            return Hash(bytes.AsSpan());
        }

        /// <summary>
        /// Computes an FNV-1a 64-bit hash over bytes without allocating.
        /// An empty span returns the FNV-1a offset basis.
        /// </summary>
        public static ulong Hash(ReadOnlySpan<byte> bytes)
        {
            ulong hash = FNV_OFFSET_BASIS;

            // The wraparound below is the algorithm, not an oversight. C# is unchecked by default,
            // so this block is intent rather than necessity, and keeps the multiply correct if the
            // file is ever compiled in a checked context.
            unchecked
            {
                for (int i = 0; i < bytes.Length; i++)
                {
                    hash ^= bytes[i];
                    hash *= FNV_PRIME;
                }
            }

            return hash;
        }
    }
}