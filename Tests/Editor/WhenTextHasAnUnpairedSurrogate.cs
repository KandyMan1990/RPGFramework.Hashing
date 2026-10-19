using NUnit.Framework;

namespace RPGFramework.Hashing.Tests
{
    internal sealed class WhenTextHasAnUnpairedSurrogate
    {
        [Test]
        public void ThenItHashesAsTheReplacementCharacter()
        {
            Assert.AreEqual(Fnv1a64.Hash("a�b"), Fnv1a64.Hash("a\uD800b"));
        }
    }
}
