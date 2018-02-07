ef-codefirst-migrations-ci-experiment
=====================================

A lab aimed to achieve basic understanding of including Entity Framework code-first migrations to continuous integration process. The requirements are:

1. Make a build script that has target to update the database
2. Set up Teamcity to use this build script

## Solution

1. There's only 1 project in this solution. This project defines data model, migrations and a simple test.
2. There's MSBuild script that has target `UpdateDatabase`. This target uses EF's `migrate.exe` tool to update the database.
3. When configuring Teamcity build, the only build parameter to be provided is `env.AppConnectionString`. The build steps are:
 * MSBuild, Build.xml, CreateEmptyBuildDirectory
 * MSBuild, Build.xml, UpdateAppConfiguration
 * MSBuild, Build.xml, BuildApp
 * MSBuild, Build.xml, UpdateDatabase
 * NUnit, EFCodeFirstMigrationsCIExperiment.dll
4. There's also `Build.cmd` to run this stuff manually.

## Comments

[Here](http://msdn.microsoft.com/en-us/data/jj618307.aspx) they describe some magic on making `migrate.exe` work, when installed via NuGet.
