using NUnit.Framework;

namespace NUnitExperiment.Trivial.Classic
{
    public class CollectionTests : ICollectionTests
    {
        [Test]
        public void CanCheckIfCollectionsAreEqual()
        {
            CollectionAssert.AreEqual(new[] { 1, 2, 3 }, new[] { 1, 2, 3 });
        }

        [Test]
        public void CanCheckIfCollectionsAreNotEqual()
        {
            CollectionAssert.AreNotEqual(new[] { 1, 2, 3 }, new[] { 3, 1, 2 });
        }

        [Test]
        public void CanCheckIfCollectionsAreEquivalent()
        {
            CollectionAssert.AreEquivalent(new[] { 1, 2, 3 }, new[] { 3, 1, 2 });
        }

        [Test]
        public void CanCheckIfCollectionsAreNotEquivalen()
        {
            CollectionAssert.AreNotEquivalent(new[] { 1, 2, 3 }, new[] { 4, 5, 6 });
        }

        [Test]
        public void CanCheckIfCollectionIsEmpty()
        {
            CollectionAssert.IsEmpty(new int[] {});
        }

        [Test]
        public void CanCheckIfCollectionIsNotEmpty()
        {
            CollectionAssert.IsNotEmpty(new[] { 1 });
        }

        [Test]
        public void CanCheckIfCollectionIsASubsetOfAnotherCollection()
        {
            CollectionAssert.IsSubsetOf(new[] { 1, 3 }, new[] { 1, 2, 3 });
        }

        [Test]
        public void CanCheckIfCollectionIsNotASubsetOfAnotherCollection()
        {
            CollectionAssert.IsNotSubsetOf(new[] { 1, 4 }, new[] { 1, 2, 3 });
        }

        [Test]
        public void CanCheckIfCollectionContains()
        {
            CollectionAssert.Contains(new[] { 1, 2, 3 }, 3);
        }

        [Test]
        public void CanCheckIfCollectionDoesNotContain()
        {
            CollectionAssert.DoesNotContain(new[] { 1, 2, 3 }, 5);
        }

        [Test]
        public void CanCheckIfCollectionIsOrdered()
        {
            CollectionAssert.IsOrdered(new[] { 1, 2, 3 });
        }

        [Test]
        public void CanCheckIfAllItemsAreUnique()
        {
            CollectionAssert.AllItemsAreUnique(new[] { 1, 2, 3 });
        }
    }
}