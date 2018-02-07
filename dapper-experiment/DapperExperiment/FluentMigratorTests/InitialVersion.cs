using FluentMigrator;

namespace DapperExperiment.FluentMigratorTests
{
    [Migration(1)]
    public class InitialVersion : Migration
    {
        public override void Up()
        {
            Create.Table("Users")
                  .WithColumn("UserId").AsInt32().PrimaryKey().Identity().NotNullable()
                  .WithColumn("UserName").AsString().NotNullable();
        }

        public override void Down()
        {
            Delete.Table("Users");
        }
    }
}