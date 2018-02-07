using EFCodeFirstMigrationsExperiment.V1;
using EFCodeFirstMigrationsExperiment.V2;

namespace EFCodeFirstMigrationsExperiment.Migrations
{
    public static class MigrationModelStates
    {
        public static readonly string Version0 = null;
        public static readonly string Version1 = MigrationMetadataHelper.MakeModelStateFromDbContext(new MyContextV1());
        public static readonly string Version2 = MigrationMetadataHelper.MakeModelStateFromDbContext(new MyContextV2());
    }
}