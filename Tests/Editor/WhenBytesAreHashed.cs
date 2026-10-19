using System;
using NUnit.Framework;

namespace RPGFramework.Hashing.Tests
{
    internal sealed class WhenBytesAreHashed
    {
        [Test]
        public void ThenNoneHashesAsTheOffsetBasis()
        {
            Assert.AreEqual(14695981039346656037UL, Fnv1a64.Hash(Array.Empty<byte>()));
        }

        [Test]
        public void ThenANullArrayHashesAsEmpty()
        {
            Assert.AreEqual(Fnv1a64.Hash(Array.Empty<byte>()), Fnv1a64.Hash((byte[])null));
        }

        [Test]
        public void ThenAnArrayAndItsSpanAgree()
        {
            byte[] bytes = { 0x00, 0x7F, 0x80, 0xFF };

            Assert.AreEqual(Fnv1a64.Hash(bytes), Fnv1a64.Hash(new ReadOnlySpan<byte>(bytes)));
        }

        [Test]
        public void ThenOrderMatters()
        {
            Assert.AreNotEqual(Fnv1a64.Hash(new byte[] { 1, 2 }), Fnv1a64.Hash(new byte[] { 2, 1 }));
        }
    }
}
