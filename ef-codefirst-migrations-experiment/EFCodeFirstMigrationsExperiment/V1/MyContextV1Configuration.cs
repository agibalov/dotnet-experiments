using System.Data.Entity.Migrations;

namespace EFCodeFirstMigrationsExperiment.V1
{
    public class MyContextV1Configuration : DbMigrationsConfiguration<MyContextV1>
    {
        public MyContextV1Configuration()
        {
            AutomaticMigrationsEnabled = false;
            MigrationsNamespace = "EFCodeFirstMigrationsExperiment.Migrations";
        }
    }
}