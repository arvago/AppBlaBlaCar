using Microsoft.Azure.Storage;
using Microsoft.Azure.Storage.Blob;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace AppBlaBlaCar.Services
{
    //Enumerador para identificar el tipo de blob (imagen)
    public enum AzureContainer
    {
        Image
    }

    public class AzureService
    {
        public CloudBlobContainer GetContainerAsync(AzureContainer type)
        {
            var account = CloudStorageAccount.Parse(Data.Constants.StorageConnection);
            var client = account.CreateCloudBlobClient();
            return client.GetContainerReference(type.ToString().ToLower());
        }

        public async Task<IList<string>> GetFilesListAsync(AzureContainer type)
        {
            var container = GetContainerAsync(type);
            var list = new List<string>();
            BlobContinuationToken token = null;
            do
            {
                var result = await container.ListBlobsSegmentedAsync(token);
                if (result.Results.Count() > 0)
                {
                    var blobs = result.Results.Cast<CloudBlockBlob>().Select(b => b.Name);
                    list.AddRange(blobs);
                }
                token = result.ContinuationToken;
            } while (token != null);
            return list;
        }

        public async Task<byte[]> GetFileAsync(AzureContainer type, string name)
        {
            // Descarga el archivo blob con el nombre "name" del contenedor "containerType"
            var container = GetContainerAsync(type);
            var blob = container.GetBlobReference(name);

            if (await blob.ExistsAsync())
            {
                await blob.FetchAttributesAsync();
                byte[] blobBytes = new byte[blob.Properties.Length];
                await blob.DownloadToByteArrayAsync(blobBytes, 0);
                return blobBytes;
            }
            return null;
        }

        public async Task<string> UploadFileAsync(AzureContainer type, Stream stream)
        {
            // Subimos un archivo "stream" al contenedor "containerType"
            var container = GetContainerAsync(type);
            await container.CreateIfNotExistsAsync();

            var name = Guid.NewGuid().ToString();
            var fileBlob = container.GetBlockBlobReference(name);
            await fileBlob.UploadFromStreamAsync(stream);
            return name;
        }

        public async Task<bool> DeleteFileAsync(AzureContainer type, string name)
        {
            // Borra un archivo blob "name" de un contenedor "containerType"
            var container = GetContainerAsync(type);
            var blob = container.GetBlobReference(name);
            return await blob.DeleteIfExistsAsync();
        }

        public async Task<bool> DeleteContainerAsync(AzureContainer type)
        {
            // Borra un contenedor "containerType"
            var container = GetContainerAsync(type);
            return await container.DeleteIfExistsAsync();
        }
    }
}
