using Autofac;
using StockAnalyzer.FileStorage.AzureBlob;
using Microsoft.AspNetCore.StaticFiles;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StockAnalyzer.FileStorage
{
    public class FileStoreModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            // MIME types
            builder.RegisterType<FileExtensionContentTypeProvider>().As<IContentTypeProvider>().SingleInstance();

            var mediaPath = System.Web.Hosting.HostingEnvironment.IsHosted
                                   ? System.Web.Hosting.HostingEnvironment.MapPath("~/Media/") ?? ""
                                   : Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Media");

            BlobStorageOptions blobStorageOptions = new BlobStorageOptions()
            {
                ConnectionString = ConfigurationManager.AppSettings["StorageConnectionString"],
                ContainerName = ConfigurationManager.AppSettings["RequestBlobContainerName"]

            };



            builder.Register(c => new AzureBlob.BlobFileStore(blobStorageOptions, c.Resolve<IContentTypeProvider>())).As<ICloudFileStore>();

            builder.Register(c => new FileSystem.FileSystemStore(mediaPath)).As<IFileStore>();
        }
    }
}