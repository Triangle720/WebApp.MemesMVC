using Microsoft.AspNetCore.Http;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Blob;
using System;
using System.IO;
using System.Threading.Tasks;

namespace BlobStorageDemo
{
    public static class BlobStorageService
    {
        public static string BlobConnectionString { get; set; }

        public async static Task<CloudBlobContainer> GetBlobContainer()
        {
            var cloudStorageAccount = CloudStorageAccount.Parse(BlobConnectionString);
            var cloudBlobClient = cloudStorageAccount.CreateCloudBlobClient();
            var cloudBlobContainer = cloudBlobClient.GetContainerReference("image");

            if (await cloudBlobContainer.CreateIfNotExistsAsync())
            {
                await cloudBlobContainer.SetPermissionsAsync(
                    new BlobContainerPermissions
                    {
                        PublicAccess = BlobContainerPublicAccessType.Blob
                    }
                    );
            }

            return cloudBlobContainer;
        }

        public static async Task<string> UploadImageAsync(IFormFile file)
        {
            try
            {
                var cloudBlobContainer = await GetBlobContainer();
                var imageName = Guid.NewGuid().ToString() + "-" + Path.GetExtension(file.FileName);

                var cloudBlockBlob = cloudBlobContainer.GetBlockBlobReference(imageName);
                cloudBlockBlob.Properties.ContentType = file.ContentType;
                await cloudBlockBlob.UploadFromStreamAsync(file.OpenReadStream());

                return cloudBlockBlob.Uri.ToString(); 
            }
            catch
            {
                return string.Empty;
            }
        }

        public static async Task DeleteImageAsync(string imageLocalPath)
        {
            var cloudBlobContainer = await GetBlobContainer();
            var blob = cloudBlobContainer.GetBlockBlobReference(imageLocalPath);
            await blob.DeleteAsync();
        }
    }
}