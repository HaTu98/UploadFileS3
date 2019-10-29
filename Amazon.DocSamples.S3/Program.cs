using Amazon.S3;
using Amazon.S3.Transfer;
using System;
using System.Diagnostics;
using System.IO;
using System.Threading.Tasks;

namespace Amazon.DocSamples.S3
{
    class UploadFileMPUHighLevelAPITest
    {
        private const string bucketName = "tuhvupload";
        private const string keyName = "file1";
        private const string filePath1 = @"C:\Users\TuHV\Documents\test1.mp4";
        private const string filePath2 = @"C:\Users\TuHV\Documents\test2.mp4";
        // Specify your bucket region (an example region is shown).
        private static readonly RegionEndpoint bucketRegion = RegionEndpoint.APSoutheast1;
        private static IAmazonS3 s3Client;

        public static void Main()
        {
            Console.WriteLine("start");
            s3Client = new AmazonS3Client("AKIA6PYYJMASGIKCXL53", "jK56S8EvgOA9lLNN8R3nNldE1IW2yVPXn9K0rNYH", bucketRegion);
            UploadFileAsync().Wait();
            Console.WriteLine("stop");
        
        }

        private static async Task UploadFileAsync()
        {
            try
            {
                var fileTransferUtility =
                    new TransferUtility(s3Client);

                Stopwatch sw = Stopwatch.StartNew();
               
                var uploadRequest =
                    new TransferUtilityUploadRequest
                    {
                        BucketName = bucketName,
                        FilePath = filePath2,
                        StorageClass = S3StorageClass.StandardInfrequentAccess,
                        Key = "file2",
                        PartSize = 5*1024*1024,
                        CannedACL = S3CannedACL.PublicRead
                    };

                uploadRequest.UploadProgressEvent +=
                    new EventHandler<UploadProgressArgs>
                        (uploadRequest_UploadPartProgressEvent);

                await fileTransferUtility.UploadAsync(uploadRequest);
                sw.Stop();
                Console.WriteLine("Upload completed : {0}", sw.Elapsed.TotalMilliseconds);

            }
            catch (AmazonS3Exception e)
            {
                Console.WriteLine("Error encountered on server. Message:'{0}' when writing an object", e.Message);
            }
            catch (Exception e)
            {
                Console.WriteLine("Unknown encountered on server. Message:'{0}' when writing an object", e.Message);
            }

        }

        static void uploadRequest_UploadPartProgressEvent(object sender, UploadProgressArgs e)
        {
            // Process event.
            Console.WriteLine("{0}/{1}", e.TransferredBytes, e.TotalBytes);
        }
    }
}
