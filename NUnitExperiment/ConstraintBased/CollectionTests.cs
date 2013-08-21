using NUnit.Framework;

namespace NUnitExperiment.ConstraintBased
{
    public class CollectionTests : ICollectionTests
    {
        [Test]
        public void CanCheckIfCollectionsAreEqual()
        {
            Assert.That(new[] { 1, 2, 3 }, Is.EqualTo(new[] { 1, 2, 3 }));
        }

        [Test]
        public void CanCheckIfCollectionsAreNotEqual()
        {
            Assert.That(new[] { 1, 2, 3 }, Is.Not.EqualTo(new[] { 3, 1, 2 }));
        }

        [Test]
        public void CanCheckIfCollectionsAreEquivalent()
        {
            Assert.That(new[] { 1, 2, 3 }, Is.EquivalentTo(new[] { 3, 1, 2 }));
        }

        [Test]
        public void CanCheckIfCollectionsAreNotEquivalen()
        {
            Assert.That(new[] { 1, 2, 3 }, Is.Not.EquivalentTo(new[] { 4, 5, 6 }));
        }

        [Test]
        public void CanCheckIfCollectionIsEmpty()
        {
            Assert.That(new int[]{}, Is.Empty);
        }

        [Test]
        public void CanCheckIfCollectionIsNotEmpty()
        {
            Assert.That(new[] { 1 }, Is.Not.Empty);
        }

        [Test]
        public void CanCheckIfCollectionIsASubsetOfAnotherCollection()
        {
            Assert.That(new [] { 1, 3 }, Is.SubsetOf(new[]{1, 2, 3}));
        }

        [Test]
        public void CanCheckIfCollectionIsNotASubsetOfAnotherCollection()
        {
            Assert.That(new[] { 1, 4 }, Is.Not.SubsetOf(new[] { 1, 2, 3 }));
        }

        [Test]
        public void CanCheckIfCollectionContains()
        {
            Assert.That(new[] { 1, 2, 3 }, Contains.Item(3));
        }

        [Test]
        public void CanCheckIfCollectionDoesNotContain()
        {
            Assert.That(new[] { 1, 2, 3 }, Is.Not.Contains(4));
        }

        [Test]
        public void CanCheckIfCollectionIsOrdered()
        {
            Assert.That(new[] { 1, 2, 3 }, Is.Ordered);
            Assert.That(new[] { 3, 2, 1 }, Is.Ordered.Descending);
        }

        [Test]
        public void CanCheckIfAllItemsAreUnique()
        {
            Assert.That(new[] { 1, 2, 3 }, Is.Unique);
        }
    }
}