using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using System.Windows.Navigation;
using Amazon;
using Amazon.Runtime;
using Amazon.S3;
using Amazon.S3.Model;
using Microsoft.Extensions.Configuration;
using System.IO;
using Amazon.S3.Transfer;
using System.Collections.ObjectModel;

namespace WahidaHossainComp306Lab01
{
    class S3connection
    {
     /*  
        private static async void GetBucketList()
        {
            var builder = new ConfigurationBuilder()
                                .SetBasePath(Directory.GetCurrentDirectory())
                                .AddJsonFile("AppSettings.json", optional: false, reloadOnChange: true);

            var accessKeyID = builder.Build().GetSection("AWSCredentials").GetSection("AccesskeyID").Value;
            var secretKey = builder.Build().GetSection("AWSCredentials").GetSection("Secretaccesskey").Value;

            var credentials = new BasicAWSCredentials(accessKeyID, secretKey);

            using (AmazonS3Client s3Client = new AmazonS3Client(credentials, RegionEndpoint.USEast1))
            {
                ListBucketsResponse response = await s3Client.ListBucketsAsync();
                foreach (S3Bucket bucket in response.Buckets)
                {
                    Console.WriteLine(bucket.BucketName + " " + bucket.CreationDate.ToShortDateString());
}
}

        }

*/


//new code goes here..........................
public ObservableCollection<s3bucket> bucketCollection = new ObservableCollection<s3bucket>();
        public ObservableCollection<BucketElements> bucketObjectCollection = new ObservableCollection<BucketElements>();
        public List<string> listOfBucketNames = new List<string>();

        public S3connection()
        {

        }

        
        /// This method creats an AmazonS3Client and returns to caller 
       
        private AmazonS3Client CreateAmazonS3Client()
        {

            var builder = new ConfigurationBuilder()
                                .SetBasePath(Directory.GetCurrentDirectory())
                                .AddJsonFile("AppSettings.json", optional: true, reloadOnChange: true);

            var accessKeyID = builder.Build().GetSection("AWSCredentials").GetSection("AccesskeyID").Value;
            var secretKey = builder.Build().GetSection("AWSCredentials").GetSection("Secretaccesskey").Value;

            var credentials = new BasicAWSCredentials(accessKeyID, secretKey);

            AmazonS3Client s3Client = new AmazonS3Client(credentials, RegionEndpoint.CACentral1);

            return s3Client;

        }

        /// This method creats one AWS-S3 bucket 
        public void CreateBucket(string newBucketName)
        {
            AmazonS3Client s3Client = CreateAmazonS3Client();

            PutBucketRequest request = new PutBucketRequest
            {
                BucketName = newBucketName,
                BucketRegion = S3Region.CACentral1,         // set region to EU
                CannedACL = S3CannedACL.PublicRead  // make bucket publicly readable
            };

            // Issue call
            PutBucketResponse response = s3Client.PutBucket(request);
        }

        /// This method gets AWS-S3 bucket list 

        public async void GetBucketList()
        {
            
            //clearing buckets & elements---------------------------------------
            bucketCollection.Clear();            
            listOfBucketNames.Clear();


            // amazon default  object storage service -------------------------------
            AmazonS3Client s3Client = CreateAmazonS3Client();

            ListBucketsResponse response = await s3Client.ListBucketsAsync();
            foreach (S3Bucket bucket in response.Buckets)
            {             

                s3bucket oneNewBucket = new s3bucket(bucket.BucketName, bucket.CreationDate.ToShortDateString());
                bucketCollection.Add(oneNewBucket);
                listOfBucketNames.Add(bucket.BucketName);
               
            }


        }

        /// This method Uploads a file into a bucket list 
        /// Ref : docs.aws.amazon.com/AmazonS3/latest/userguide/create-bucket-overview.html
        public async Task UploadFileAsync(string filePath, string bucketName, string keyName)
        {

            try
            {

                AmazonS3Client s3Client = CreateAmazonS3Client();

                var fileTransferUtility = new TransferUtility(s3Client);

                // Option 1. Upload a file. The file name is used as the object key name.
                await fileTransferUtility.UploadAsync(filePath, bucketName);
                Console.WriteLine("Uploaded successfully");

               
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



        
        public void GetObjectList(string sourceBucketName)
        {
            
            bucketObjectCollection.Clear();

            //Coding ref : docs.aws.amazon.com/sdkfornet1/latest/apidocs/html/M_Amazon_S3_AmazonS3Client_ListObjects.htm-----------------
            AmazonS3Client s3Client = CreateAmazonS3Client();

            // List all bucket elements ------------------------------------------
            ListObjectsRequest listRequest = new ListObjectsRequest
            {
                BucketName = sourceBucketName,
            };

            ListObjectsResponse listResponse;
            do
            {
                // Get a list of objects
                listResponse = s3Client.ListObjects(listRequest);
                foreach (S3Object obj in listResponse.S3Objects)
                {

                    Console.WriteLine("File Name - " + obj.Key);
                    Console.WriteLine("File Size - " + obj.Size);
                    Console.WriteLine("Last Modified - " + obj.LastModified);
                    Console.WriteLine(" Storage class - " + obj.StorageClass);

                    
                    BucketElements oneNewBucketObject = new BucketElements(obj.Key, obj.Size.ToString(), obj.LastModified.ToString(), obj.StorageClass.ToString());
                    bucketObjectCollection.Add(oneNewBucketObject);
                    


                }

                // Set the marker property
                listRequest.Marker = listResponse.NextMarker;
            } while (listResponse.IsTruncated);

        }

    }
}
