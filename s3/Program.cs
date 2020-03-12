using System;
using System.Threading.Tasks;
using Amazon;
using Amazon.S3;
using Amazon.S3.Model;

namespace s3
{
    class Program
    {
        static IAmazonS3 amazonS3Client;

        static void Main(string[] args)
        {
            Task.Run(MainAsync).GetAwaiter().GetResult();
        }

        static async Task MainAsync()
        {
            
            var config = new AmazonS3Config
            {
                RegionEndpoint = RegionEndpoint.USEast1, // MUST set this before setting ServiceURL and it should match the `MINIO_REGION` environment variable.
                ServiceURL = "http://localhost:9000", // replace http://localhost:9000 with URL of your MinIO server
                ForcePathStyle = true // MUST be true to work correctly with MinIO server
            };

            using(amazonS3Client = new AmazonS3Client("minioadmin", "minioadmin", config)) {

            
                PutBucketRequest request = new PutBucketRequest();
                request.BucketName = "teste2";
                await amazonS3Client.PutBucketAsync(request);


                var listBucketResponse = await amazonS3Client.ListBucketsAsync();

                foreach (var bucket in listBucketResponse.Buckets)
                {
                    Console.Out.WriteLine("bucket '" + bucket.BucketName + "' created at " + bucket.CreationDate);
                }
                if (listBucketResponse.Buckets.Count > 0)
                {
                    var bucketName = listBucketResponse.Buckets[0].BucketName;

                    var listObjectsResponse = await amazonS3Client.ListObjectsAsync(bucketName);

                    foreach (var obj in listObjectsResponse.S3Objects)
                    {
                        Console.Out.WriteLine("key = '" + obj.Key + "' | size = " + obj.Size + " | tags = '" + obj.ETag + "' | modified = " + obj.LastModified);
                    }
                }



            }

            Console.WriteLine("Hello World!");
        }
    }
}
