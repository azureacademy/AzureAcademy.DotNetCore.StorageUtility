using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Auth;
using Microsoft.WindowsAzure.Storage.Blob;
using System;

namespace AzureAcademy.DotNetCore.StorageUtility
{
    class Program
    {
        static void Main(string[] args)
        {

            string sa = Environment.GetEnvironmentVariable("storageAccount");
            string saKey = Environment.GetEnvironmentVariable("storageAccountKey");
            string saContainer = Environment.GetEnvironmentVariable("storageAccountContainer");

            if(string.IsNullOrEmpty(sa) || string.IsNullOrEmpty(saKey) || string.IsNullOrEmpty(saContainer))
            {
                throw new Exception("Missing environment variables");
            }

            var storageAccount = new CloudStorageAccount(
            new StorageCredentials(sa, saKey), true);            

            var blobClient = storageAccount.CreateCloudBlobClient();

            var container = blobClient.GetContainerReference(saContainer);

            BlobContinuationToken token = null;
            do
            {
                BlobResultSegment resultSegment = container.ListBlobsSegmentedAsync(token).Result;
                token = resultSegment.ContinuationToken;

                foreach (IListBlobItem item in resultSegment.Results)
                {
                    if (item.GetType() == typeof(CloudBlockBlob))
                    {
                        CloudBlockBlob blob = (CloudBlockBlob)item;
                        if (blob.Properties.LastModified.Value.DateTime < DateTime.UtcNow.AddMinutes(-1))
                            blob.DeleteAsync().Wait();
                    }

                }
            } while (token != null);


        }
    }
}
