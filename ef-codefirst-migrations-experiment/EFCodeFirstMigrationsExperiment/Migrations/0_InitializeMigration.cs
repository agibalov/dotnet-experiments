using System.Data.Entity.Migrations;
using System.Data.Entity.Migrations.Infrastructure;

namespace EFCodeFirstMigrationsExperiment.Migrations
{
    public class InitializeMigration : DbMigration, IMigrationMetadata
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Posts",
                c => new
                    {
                        PostId = c.Int(nullable: false, identity: true),
                        PostText = c.String(),
                    })
                .PrimaryKey(t => t.PostId);
        }

        public override void Down()
        {
            DropTable("dbo.Posts");
        }

        public string Id
        {
            get { return "201305100145199_InitializeMigration"; }
        }

        public string Source
        {
            get { return MigrationModelStates.Version0; }
        }

        public string Target
        {
            get { return MigrationModelStates.Version1; }
        }
    }
}