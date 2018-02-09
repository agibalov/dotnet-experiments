using System.IO;
using System.Text;
using System.Windows.Markup;
using NUnit.Framework;

namespace HelloProgrammaticXaml
{
    public class XamlReaderTest
    {
        [Test]
        public void CanConstructCustomObjectFromXaml()
        {
            var s = new MemoryStream(Encoding.UTF8.GetBytes("<Something Message='hello there'></Something>"));
            var xamlTypeMapper = new XamlTypeMapper(new string[] { });
            xamlTypeMapper.AddMappingProcessingInstruction("", "HelloProgrammaticXaml", "HelloProgrammaticXaml");

            var somethingLoadedFromXaml = (Something)XamlReader.Load(s, new ParserContext
            {
                XamlTypeMapper = xamlTypeMapper
            });

            Assert.AreEqual("hello there", somethingLoadedFromXaml.Message);
        }
    }

    public class Something
    {
        public string Message { get; set; }
    }
}
