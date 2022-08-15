using StockAnalyzer.FileStorage;
using StockAnalyzer.FileStorage.FileSystem;
using Moq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using NUnit.Framework;

namespace StockAnalyzer.Tests.FileStorage.FileSystem
{
    [TestFixture]
    public class FileSystemStoreTests : IDisposable
    {
        private string filePath;
        private string folderPath;
        private IFileStore storageProvider;


        public FileSystemStoreTests()
        {
            folderPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Media");
            filePath = folderPath + "\\testfile.txt";

            if (Directory.Exists(folderPath))
            {
                try
                {
                    Directory.Delete(folderPath, true);
                }
                catch
                {
                    // happens sometimes
                }
            }

            Directory.CreateDirectory(folderPath);
            File.WriteAllText(filePath, "testfile contents");

            var subfolder1 = Path.Combine(folderPath, "Subfolder1");
            Directory.CreateDirectory(subfolder1);
            File.WriteAllText(Path.Combine(subfolder1, "one.txt"), "one contents");
            File.WriteAllText(Path.Combine(subfolder1, "two.txt"), "two contents");

            var subsubfolder1 = Path.Combine(subfolder1, "SubSubfolder1");
            Directory.CreateDirectory(subsubfolder1);

            storageProvider = new FileSystemStore(folderPath);


        }

        [Test]
        public async Task ExistsShouldBeTrueForExtistingFile()
        {
            var result = await storageProvider.GetFileInfoAsync("testfile.txt");
            Assert.NotNull(result);
        }

        [Test]
        public async Task ExistsShouldBeFalseForNonExtistingFile()
        {
            Assert.Null(await storageProvider.GetFileInfoAsync("notexisting"));
        }



        [Test]
        public async Task GetDirectoryInfoAsync_WithValidPath_ShouldReturnFolderDetails()
        {
            var result = await storageProvider.GetDirectoryInfoAsync(
                folderPath);

            // Assert
            Assert.NotNull(result);

        }

        [Test]
        public async Task GetDirectoryInfoAsync_WithInvalidPath_ShouldNotReturnFolderDetails()
        {
            var result = await storageProvider.GetDirectoryInfoAsync(
                "abc");

            // Assert
            Assert.Null(result);

        }

        [Test][Ignore("")]
        public async Task GetDirectoryContentAsync_WithInvalidPath_ShouldReturnNull()
        {
            // Arrange

            string path = "abc";
            bool includeSubDirectories = false;

            // Act
            var result = await storageProvider.GetDirectoryContentAsync(
                path,
                includeSubDirectories);

            // Assert
            Assert.True(false);

        }
        [Test]
        public async Task ShouldCreateFolders()
        {
            Directory.Delete(folderPath, true);
            await storageProvider.TryCreateDirectoryAsync("foo/bar/baz");
            Assert.Equalss((await ListFolders("")).Count(), 1);
            Assert.Equals((await ListFolders("foo")).Count(), 1);
            Assert.Equals((await ListFolders("foo/bar")).Count(), 1);
        }

        [Test]
        [Ignore("")]
        public async Task TryCreateDirectoryAsync_StateUnderTest_ExpectedBehavior()
        {
            // Arrange

            string path = null;

            // Act
            var result = await storageProvider.TryCreateDirectoryAsync(
                path);

            // Assert
            Assert.True(false);

        }

        [Test]
        public async Task DeleteFolderFailsInInvalidPath()
        {
            Assert.ThrowsAsync<ArgumentException>(async () => await storageProvider.TryDeleteDirectoryAsync(@"../InvalidFolder1"));
            Assert.ThrowsAsync<ArgumentException>(async () => await storageProvider.TryDeleteDirectoryAsync(@"../../InvalidFolder1"));

            // Valid create one level up within the storage provider domain
            Assert.NotNull(await storageProvider.GetDirectoryInfoAsync("SubFolder1"));
            await storageProvider.TryDeleteDirectoryAsync(@"SubFolder1\..\SubFolder1");
            Assert.Null(await storageProvider.GetDirectoryInfoAsync("SubFolder1"));
        }

        [Test][Ignore("")]
        public async Task TryDeleteDirectoryAsync_StateUnderTest_ExpectedBehavior()
        {
            // Arrange

            string path = null;

            // Act
            var result = await storageProvider.TryDeleteDirectoryAsync(
                path);

            // Assert
            Assert.True(false);

        }

        [Test][Ignore("")]
        public async Task MoveFileAsync_StateUnderTest_ExpectedBehavior()
        {
            // Arrange

            string oldPath = null;
            string newPath = null;

            // Act
            await storageProvider.MoveFileAsync(
                oldPath,
                newPath);

            // Assert
            Assert.True(false);

        }

        [Test][Ignore("")]
        public async Task CopyFileAsync_StateUnderTest_ExpectedBehavior()
        {
            // Arrange

            string srcPath = null;
            string dstPath = null;

            // Act
            await storageProvider.CopyFileAsync(
                srcPath,
                dstPath);

            // Assert
            Assert.True(false);

        }

        [Test][Ignore("")]
        public async Task GetFileStreamAsync_StateUnderTest_ExpectedBehavior()
        {
            // Arrange

            string path = null;

            // Act
            var result = await storageProvider.GetFileStreamAsync(
                path);

            // Assert
            Assert.True(false);

        }

        [Test][Ignore("")]
        public async Task GetFileStreamAsync_StateUnderTest_ExpectedBehavior1()
        {
            // Arrange

            IFileStoreEntry fileStoreEntry = null;

            // Act
            var result = await storageProvider.GetFileStreamAsync(
                fileStoreEntry);

            // Assert
            Assert.True(false);

        }

        [Test][Ignore("")]
        public async Task CreateFileFromStreamAsync_StateUnderTest_ExpectedBehavior()
        {
            // Arrange

            string path = null;
            Stream inputStream = null;
            bool overwrite = false;

            // Act
            var result = await storageProvider.CreateFileFromStreamAsync(
                path,
                inputStream,
                overwrite);

            // Assert
            Assert.True(false);

        }

        public void Dispose()
        {
            try
            {
                Directory.Delete(folderPath, true);
            }
            catch (IOException)
            {
                // if a system handle is still active give some time to release it
                Thread.Sleep(0);
                Directory.Delete(folderPath, true);
            }
        }

        private async Task<IEnumerable<IFileStoreEntry>> ListFolders(string path)
        {
            IEnumerable<IFileStoreEntry> fileEntries = new List<IFileStoreEntry>();
            IFileStoreEntry directoryInfo = await storageProvider.GetDirectoryInfoAsync(path);
            if (directoryInfo != null)
            {
                fileEntries = (await storageProvider.GetDirectoryContentAsync(path, false))
                    .Where(f => f.IsDirectory);
            }
            return fileEntries;
        }

        /// <summary>
        /// Lists the files within a storage provider's path.
        /// </summary>
        /// <param name="path">The relative path to the folder which files to list.</param>
        /// <returns>The list of files in the folder.</returns>
        private async Task<IEnumerable<IFileStoreEntry>> ListFiles(string path)
        {
            IFileStoreEntry directoryInfo = await storageProvider.GetDirectoryInfoAsync(path);
            if (directoryInfo == null)
            {
                throw new ArgumentException($"Directory {path} does not exist");
            }

            return (await storageProvider.GetDirectoryContentAsync(path, false))
                    .Where(f => !f.IsDirectory)
                    .ToList();
        }

        private async Task<bool> FolderExists(string path)
        {
            return (await storageProvider.GetDirectoryInfoAsync(path)) != null;
        }
    }
}
