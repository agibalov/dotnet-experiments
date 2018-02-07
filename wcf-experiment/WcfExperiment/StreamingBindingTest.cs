using System;
using System.IO;
using System.ServiceModel;
using System.ServiceModel.Channels;
using NUnit.Framework;

namespace WcfExperiment
{
    public class StreamingBindingTest
    {
        private const int OneGbInBytes = 1024 * 1024 * 1024;
        private const int HundredMbInBytes = 100 * 1024 * 1024;
        private const int MaxMessageSizeInBytes = OneGbInBytes;
        private const int TestDataSizeInBytes = HundredMbInBytes;

        [Test]
        public void CanHaveStreamingForUploadsAndDownloads()
        {
            var transport = new TcpTransportBindingElement
            {
                TransferMode = TransferMode.Streamed,
                MaxBufferSize = 2 * 1024, // Note: the buffer is 2KB, while the test data is 100MB (this may not work sometimes, though)
                MaxReceivedMessageSize = MaxMessageSizeInBytes
            };
            var encoder = new BinaryMessageEncodingBindingElement();
            var customBinding = new CustomBinding(encoder, transport);

            var fileExchangeService = new FileExchangeService();
            using (var serviceHost = new ServiceHost(fileExchangeService))
            {
                serviceHost.AddServiceEndpoint(typeof (IFileExchangeService), customBinding, "net.tcp://localhost:2302/");
                serviceHost.Open();

                using (var channelFactory = new ChannelFactory<IFileExchangeService>(customBinding, "net.tcp://localhost:2302/"))
                {
                    var fileExchangeServiceClient = channelFactory.CreateChannel();
                    using (var tempFile = DataFileUtils.MakeDataFile(TestDataSizeInBytes))
                    using (var tempFileInputStream = tempFile.GetInputStream())
                    {
                        var read = fileExchangeServiceClient.SendData(tempFileInputStream);
                        Assert.AreEqual(TestDataSizeInBytes, read);
                    }

                    using (var dataStream = fileExchangeServiceClient.ReceiveData(TestDataSizeInBytes))
                    {
                        var read = StreamUtils.ReadAndCountBytes(dataStream);
                        Assert.AreEqual(TestDataSizeInBytes, read);
                    }
                }
            }
        }

        [ServiceContract]
        public interface IFileExchangeService
        {
            [OperationContract]
            int SendData(Stream dataStream);

            [OperationContract]
            Stream ReceiveData(int dataSize);
        }

        [ServiceBehavior(InstanceContextMode = InstanceContextMode.Single)]
        public class FileExchangeService : IFileExchangeService
        {
            public int SendData(Stream dataStream)
            {
                return StreamUtils.ReadAndCountBytes(dataStream);
            }

            public Stream ReceiveData(int dataSize)
            {
                var dataFile = DataFileUtils.MakeDataFile(dataSize);

                OperationContext.Current.OperationCompleted += (sender, args) =>
                {
                    dataFile.Dispose();
                    dataFile = null;
                };
                
                return dataFile.GetInputStream();
            }
        }

        public static class StreamUtils
        {
            public static int ReadAndCountBytes(Stream stream)
            {
                var readTotal = 0;
                var buf = new char[256];
                using (var streamReader = new StreamReader(stream))
                {
                    while (!streamReader.EndOfStream)
                    {
                        var read = streamReader.Read(buf, 0, buf.Length);
                        readTotal += read;
                    }
                }

                return readTotal;
            }
        }

        public static class DataFileUtils
        {
            public static DataFile MakeDataFile(int size)
            {
                var filename = Path.GetTempFileName();
                using (var fileStream = File.OpenWrite(filename))
                using (var bufferedStream = new BufferedStream(fileStream))
                {
                    for (var i = 0; i < size; ++i)
                    {
                        bufferedStream.WriteByte((byte)(i % 16));
                    }
                }

                return new DataFile(filename);
            }
        }

        public class DataFile : IDisposable
        {
            private readonly string _filename;

            public DataFile(string filename)
            {
                _filename = filename;
            }

            public Stream GetInputStream()
            {
                return File.OpenRead(_filename);
            }

            public void Dispose()
            {
                File.Delete(_filename);
            }
        }
    }
}
