using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlServerCe;
using System.IO;
using System.Linq;
using EntityFrameworkExperiment2.DAL;
using EntityFrameworkExperiment2.DAL.Entities;

namespace EntityFrameworkExperiment2
{
    class Program
    {
        static void Main(string[] args)
        {
            var connectionStringBuilder = new SqlCeConnectionStringBuilder(
                ConfigurationManager.ConnectionStrings["RenoteConnectionString"].ConnectionString);
            var databaseFileName = connectionStringBuilder.DataSource;
            if (File.Exists(databaseFileName))
            {
                File.Delete(databaseFileName);
            }

            Console.WriteLine("Reset done");

            using (var context = new RenoteContext())
            {
                var user = new User {UserName = "loki2302"};
                context.Users.Add(user);
                context.SaveChanges();

                var pornTag = new Tag {User = user, TagName = "porn"};
                context.Tags.Add(pornTag);
                var programmingTag = new Tag {User = user, TagName = "programming"};
                context.Tags.Add(programmingTag);
                context.SaveChanges();

                context.Notes.Add(new Note { NoteText = "hello 1", User = user, Tags = new List<Tag> { pornTag, programmingTag }});
                context.SaveChanges();

                var tagNames = new List<string> {"porn", "programming"};
                var tags = context.Tags
                    .Where(tag => tag.UserId == user.UserId)
                    .Where(tag => tagNames.Contains(tag.TagName));
                Console.WriteLine("Found tags: {0}", string.Join(",", tags.ToList()));

                var tagIds = tags.Select(t => t.TagId).ToList();
                var q = tags.SelectMany(t => t.Notes).Distinct();
                Console.WriteLine(q);
                var foundNotes = q.ToList();
                Console.WriteLine("Found notes: {0}", string.Join(",", foundNotes));
            }
        }
    }
}
