namespace TypeSystemExperiment.Types
{
    public class Class
    {
        private readonly string _name;

        public Class(string name)
        {
            _name = name;
        }

        public override string ToString()
        {
            return _name;
        }
    }
}