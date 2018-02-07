namespace NUnitExperiment.Trivial
{
    public interface ICollectionTests
    {
        void CanCheckIfCollectionsAreEqual();
        void CanCheckIfCollectionsAreNotEqual();
        void CanCheckIfCollectionsAreEquivalent();
        void CanCheckIfCollectionsAreNotEquivalen();
        void CanCheckIfCollectionIsEmpty();
        void CanCheckIfCollectionIsNotEmpty();
        void CanCheckIfCollectionIsASubsetOfAnotherCollection();
        void CanCheckIfCollectionIsNotASubsetOfAnotherCollection();
        void CanCheckIfCollectionContains();
        void CanCheckIfCollectionDoesNotContain();
        void CanCheckIfCollectionIsOrdered();
        void CanCheckIfAllItemsAreUnique();
    }
}