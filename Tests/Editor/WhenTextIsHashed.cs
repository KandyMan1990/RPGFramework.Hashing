using System;
using System.Text;
using NUnit.Framework;

namespace RPGFramework.Hashing.Tests
{
    internal sealed class WhenTextIsHashed
    {
        // The algorithm's published test vectors.
        [TestCase("", 0xcbf29ce484222325UL)]
        [TestCase("a", 0xaf63dc4c8601ec8cUL)]
        [TestCase("foobar", 0x85944171f73967e8UL)]
        public void ThenItMatchesThePublishedValue(string text, ulong expected)
        {
            Assert.AreEqual(expected, Fnv1a64.Hash(text));
        }

        [Test]
        public void ThenItHashesAsItsUtf8Bytes()
        {
            const string TEXT = "Café — 東京";

            Assert.AreEqual(Fnv1a64.Hash(Encoding.UTF8.GetBytes(TEXT)), Fnv1a64.Hash(TEXT));
        }

        [Test]
        public void ThenNullHashesAsEmpty()
        {
            Assert.AreEqual(Fnv1a64.Hash(string.Empty), Fnv1a64.Hash((string)null));
        }

        [Test]
        public void ThenWhitespaceIsOrdinaryInput()
        {
            Assert.AreNotEqual(Fnv1a64.Hash(string.Empty), Fnv1a64.Hash(" "));
        }

        [Test]
        public void ThenItsCharactersAsASpanHashTheSame()
        {
            const string TEXT = "RPGFramework.PersistentMemory";

            Assert.AreEqual(Fnv1a64.Hash(TEXT), Fnv1a64.Hash(TEXT.AsSpan()));
        }
    }
}
