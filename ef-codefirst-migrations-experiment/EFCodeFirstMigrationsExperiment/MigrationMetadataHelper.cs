using System;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.IO;
using System.IO.Compression;
using System.Xml;
using System.Xml.Linq;

namespace EFCodeFirstMigrationsExperiment
{
    public static class MigrationMetadataHelper
    {
        public static string MakeModelStateFromDbContext(DbContext dbContext)
        {
            string edmxString;
            using (var stringWriter = new StringWriter())
            {
                using (var xmlWriter = new XmlTextWriter(stringWriter))
                {
                    EdmxWriter.WriteEdmx(dbContext, xmlWriter);
                    edmxString = stringWriter.ToString();
                }
            }

            byte[] gzippedEdmxStringBytes;
            using (var stringReader = new StringReader(edmxString))
            {
                var model = XDocument.Load(stringReader);
                using (var memoryStream = new MemoryStream())
                {
                    using (var gzipStream = new GZipStream(memoryStream, CompressionMode.Compress))
                    {
                        model.Save(gzipStream);
                    }

                    gzippedEdmxStringBytes = memoryStream.ToArray();
                }
            }

            return Convert.ToBase64String(gzippedEdmxStringBytes);
        }
    }
}