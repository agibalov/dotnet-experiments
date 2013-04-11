using AOPValidationExperiment.Validation;

namespace AOPValidationExperiment
{
    public class PostTitleAttribute : ValidationAttribute
    {
        public override bool Test(object x)
        {
            if (!(x is string))
            {
                return false;
            }

            var s = (string)x;
            return !string.IsNullOrEmpty(s);
        }
    }
}