using System.Data.Entity.Migrations;
using System.Data.Entity.Migrations.Infrastructure;

namespace EFCodeFirstMigrationsExperiment.Migrations
{
    public class AddPostTitleMigration : DbMigration, IMigrationMetadata
    {
        public override void Up()
        {
            AddColumn("dbo.Posts", "PostTitle", c => c.String(nullable: false, defaultValue: "Default Post Title"));
        }

        public override void Down()
        {
            DropColumn("dbo.Posts", "PostTitle");
        }

        public string Id
        {
            get { return "201305100152178_AddPostTitle"; }
        }

        public string Source
        {
            get { return MigrationModelStates.Version1; }
        }

        public string Target
        {
            get { return MigrationModelStates.Version2; }
        }
    }
}