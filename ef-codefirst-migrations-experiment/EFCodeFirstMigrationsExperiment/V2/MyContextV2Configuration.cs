using System.Data.Entity.Migrations;

namespace EFCodeFirstMigrationsExperiment.V2
{
    public class MyContextV2Configuration : DbMigrationsConfiguration<MyContextV2>
    {
        public MyContextV2Configuration()
        {
            AutomaticMigrationsEnabled = false;
            MigrationsNamespace = "EFCodeFirstMigrationsExperiment.Migrations";
        }
    }
}