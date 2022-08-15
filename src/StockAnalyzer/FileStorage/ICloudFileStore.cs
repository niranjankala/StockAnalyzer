using Azure.Storage.Blobs;
using Azure.Storage.Sas;
using Microsoft.Azure.Batch;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace StockAnalyzer.FileStorage
{
    /// <summary>
    /// Represents a generic abstraction over a virtual file store.
    /// </summary>
    /// <remarks>
    /// The virtual file store uses forward slash (/) as the path delimiter, and has no concept of
    /// volumes or drives. All paths are specified and returned as relative to the root of the virtual
    /// file store. Absolute paths using a leading slash or leading period, and parent traversal
    /// using "../", are not supported.
    ///
    /// This abstraction does not dictate any case sensitivity semantics. Case sensitivity is left to
    /// the underlying storage system of concrete implementations. For example, the Windows file system
    /// is case insensitive, while Linux file system and Azure Blob Storage are case sensitive.
    /// </remarks>
    public interface ICloudFileStore : IFileStore
    {
        BlobClient GetBlobReference(string blobPath);

        string GetContainerBlobSasUrl(string blobPath, BlobSasPermissions permissions);
        string GetContainerSasUrl(BlobContainerSasPermissions permissions);

    }
}
