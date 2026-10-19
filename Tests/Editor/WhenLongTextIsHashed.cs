using System.Text;
using NUnit.Framework;

namespace RPGFramework.Hashing.Tests
{
    /// <summary>
    /// Text up to 85 characters is encoded on the stack and longer text in a pooled buffer, the limit counted in
    /// characters: at three bytes each, a character limit compared as though it were bytes would overflow the stack
    /// buffer.
    /// </summary>
    internal sealed class WhenLongTextIsHashed
    {
        [TestCase(85)]
        [TestCase(86)]
        [TestCase(300)]
        public void ThenThreeByteCharactersHashAsTheirBytes(int length)
        {
            string text = new string('€', length);

            Assert.AreEqual(Fnv1a64.Hash(Encoding.UTF8.GetBytes(text)), Fnv1a64.Hash(text));
        }

        [TestCase(85)]
        [TestCase(86)]
        [TestCase(300)]
        public void ThenSingleByteCharactersHashAsTheirBytes(int length)
        {
            string text = new string('x', length);

            Assert.AreEqual(Fnv1a64.Hash(Encoding.UTF8.GetBytes(text)), Fnv1a64.Hash(text));
        }
    }
}
