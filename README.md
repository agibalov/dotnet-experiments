ef-codefirst-migrations-experiment
==================================

A lab aimed to achieve some basic understanding of Entity Framework Code-First migrations. Requirements are:

1. There should be 2 versions of the "same" database.
2. There should be migration defined to upgrade from first to second version.
3. There should be set of tests to demonstrate that both versions can be created from scratch and then upgraded/downgraded.

## Solution

1. There are 2 `DbContext`'s defined in the solution. They both work with the only physical database, but describe its different versions. There's migration defined to upgrade/downgrade between them.
2. There are 4 tests defined:
 * `CanCreateV1DatabaseAndPlayWithIt` demonstrates that DB v1 can be created from scratch and then used.
 * `CanCreateV2DatabaseAndPlayWithIt` demonstrates that DB v2 can be created from scratch and then used.
 * `CanCreateV1DatabaseThenPlayWithItThenMigrateToV2ThenPlayWithIt` demonstrates that one can create DB v1, then put some data there, then upgrade to v2 and still have this data available.
 * `CanCreateV1DatabaseThenPlayWithItThenMigrateToV2ThenPlayWithIt` demonstrates that one can create DB v2, then put some data there, then downgrade to v1 and still have this data available (in this scenario, the data is truncated).

## Comments

I do understand it's not how one should normally use it :-) So, that's perhaps why it feels like they didn't expect anybody to use it this way. Most of the magic is related with `IMigrationMetadata`.

First of all, it appeared that migration IDs should have specific format like `201305100145199_InitializeMigration`. I initially started with `0_InitializeMigration`, but didn't manage to make `DbMigrator` even see this migration.

Then, there's a trick around `Source` and `Target`. They describe respectively original model and updated model, when upgrading the DB. Sample values are like this one: `H4sIAAAAAAAEAM1WyW7bMBC9F+g/CDy1h5iOCw...blablabla...`. It appeared that they are **"BASE64 GZIPped EDMX XMLs"**. So, if you want to build one, you take your `DbContext`, render EDMX, GZIP it, BASE64 it, and then it's what they want (also see [SO](http://stackoverflow.com/questions/13725101/purpose-and-semantic-of-imigrationmetadata-interface-in-entity-framework)).

One thing that makes me upset about it is that it can't figure out a minimal set of migrations for given `DbContext`: no matter if you give it DB v1 or DB v2, it will always tell you that all migrations are pending, even if some of them will make DB schema incompatible with your bindings.
