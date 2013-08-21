namespace NUnitExperiment
{
    public interface IStringTests
    {
        void CanCheckIfStringsAreEqual();
        void CanCheckIfStringsAreNotEqual();
        void CanCheckIfStringHasASubstring();
        void CanCheckIfStringHasNoSubstring();
        void CanCheckIfStringStartsWith();
        void CanCheckIfStringDoesNotStartWith();
        void CanCheckIfStringEndsWith();
        void CanCheckIfStringDoesNotEndWith();
        void CanCheckIfStringsAreEqualIgnoringCase();
        void CanCheckIfStringMatches();
        void CanCheckIfStringDoesNotMatch();
    }
}