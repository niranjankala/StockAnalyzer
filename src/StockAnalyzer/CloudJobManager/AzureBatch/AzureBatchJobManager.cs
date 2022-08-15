using Azure.Storage.Sas;
using StockAnalyzer.FileStorage;
using StockAnalyzer.Logging;
using Microsoft.Azure.Batch;
using Microsoft.Azure.Batch.Auth;
using Microsoft.Azure.Batch.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace StockAnalyzer.CloudJobManager.AzureBatch
{
    public class AzureBatchJobManager : ICloudJobManager
    {
        private readonly AzureBatchJobManagerOptions options;
        private readonly ICloudFileStore cloudFileStore;
        BatchClient batchClient;
        public AzureBatchJobManager(AzureBatchJobManagerOptions batchOptions, ICloudFileStore cloudFileStore)
        {
            options = batchOptions;
            this.cloudFileStore = cloudFileStore;
            // Get a Batch client using account creds
            BatchSharedKeyCredentials cred = new BatchSharedKeyCredentials(options.BatchAccountUrl, options.BatchAccountName, options.BatchAccountKey);
            batchClient = BatchClient.Open(cred);
        }
        public ILogger Logger { get; set; }
        public void RunTask(string modelName)
        {

        }

        public async Task RunTaskAsync(string modelName, Guid requestId)
        {
            await CreatePoolIfNotExistAsync(options);
            await CreateJobAsync(options.BatchJobId, options.BatchPoolId);
            List<ResourceFile> resources = new List<ResourceFile>();

            foreach (var datFile in await cloudFileStore.GetDirectoryContentAsync(modelName))
            {
                if (!datFile.IsDirectory && (datFile.Name?.EndsWith(".dat") == true || datFile.Name == $"{modelName}.xml"))
                {
                    resources.Add(GetResourceFile(datFile.Path, datFile.Path));
                }
            }

            //        if (inputFiles?.Length > 0)
            //{
            //    foreach (string blobPath in inputFiles)
            //    {
            //        //resources.Add()
            //    }
            //}
            await AddTaskAsync(options.BatchJobId, modelName, requestId, resources);
        }


        public async Task CreatePoolIfNotExistAsync(AzureBatchJobManagerOptions options)
        {
            try
            {
                Logger.Information("Creating pool [{0}]...", options.BatchPoolId);

                Func<ImageInformation, bool> imageScanner = imageRef =>
                    imageRef.ImageReference.Publisher.Equals("microsoftwindowsserver", StringComparison.InvariantCultureIgnoreCase) &&
                    imageRef.ImageReference.Offer.Equals("windowsserver", StringComparison.InvariantCultureIgnoreCase) &&
                    imageRef.ImageReference.Sku.Contains("2016-datacenter-smalldisk");

                var skuAndImage = await GetNodeAgentSkuReferenceAsync(imageScanner);
                //var imageReference = new ImageReference(
                //  publisher: "microsoftwindowsserver",
                //  offer: "windowsserver",
                //  sku: "2016-datacenter-smalldisk",
                //  version: "latest");

                // ToDo: Create the Pool
                var pool = batchClient.PoolOperations.CreatePool(poolId: options.BatchPoolId,

                    // ToDo: Specify the VM size
                    virtualMachineSize: options.BatchVirtualMachineSize,
                    //targetDedicatedComputeNodes: 1,

                    // ToDo: Create a VM configuration
                    virtualMachineConfiguration: new VirtualMachineConfiguration(
                        skuAndImage.ImageReference, skuAndImage.NodeAgentSkuId)
                    //new VirtualMachineConfiguration(
                    //    imageReference: imageReference,
                    //    nodeAgentSkuId: "batch.node.windows amd64")
                    );
                pool.AutoScaleEnabled = true;
                StringBuilder formulaBuilder = new StringBuilder();

                pool.AutoScaleFormula = @$"$MaxNodes = {options.BatchPoolMaxNodes};
$CurrentActiveTasks = $ActiveTasks.GetSample(1);
$CurrentRunningTasks = $RunningTasks.GetSample(1);
$TotalTasks=$CurrentActiveTasks+$CurrentRunningTasks;
$Nodes = min($TotalTasks, $MaxNodes);
$TargetDedicated = $Nodes;";
                pool.AutoScaleEvaluationInterval = TimeSpan.FromMinutes(5);

                // ToDo: Add the application reference
                pool.ApplicationPackageReferences = new List<ApplicationPackageReference>
                {
                    new ApplicationPackageReference{ ApplicationId = options.ApplicatoinName, Version = options.ApplicationVersion }
                };

                //pool.NetworkConfiguration

                // ToDo: Commit the changes
                await pool.CommitAsync();
            }
            catch (BatchException be)
            {
                if (be.RequestInformation?.BatchError != null && be.RequestInformation.BatchError.Code == BatchErrorCodeStrings.PoolExists)
                {
                    Logger.Information("The pool {0} already existed when we tried to create it", options.BatchPoolId);
                }
                else
                {
                    throw;
                }
            }
        }


        public async Task CreateJobAsync(string jobId, string poolId)
        {
            Logger.Information($"Creating job [{jobId}]...");

            try
            {
                // ToDo: Create the job definition
                CloudJob job = batchClient.JobOperations.CreateJob();
                job.Id = jobId;
                //job.Id = Guid.NewGuid().ToString();
                //job.DisplayName = $"Kcc-RR-Job-{DateTime.Now}";

                // ToDo: Specify the pool
                job.PoolInformation = new PoolInformation { PoolId = poolId };

                // ToDo: Set job to completed state when all tasks are complete
                //job.OnAllTasksComplete = OnAllTasksComplete.TerminateJob;

                // ToDo: Commit the job
                await job.CommitAsync();

            }
            catch (BatchException be)
            {
                // Accept the specific error code JobExists as that is expected if the job already exists
                if (be.RequestInformation?.BatchError?.Code == BatchErrorCodeStrings.JobExists)
                {
                    Logger.Information($"The job {jobId} already existed when we tried to create it");
                }
                else
                {
                    throw; // Any other exception is unexpected
                }
            }
        }

        public async Task<CloudTask> AddTaskAsync(string jobId, string taskId, Guid requestId, List<ResourceFile> inputDatFiles)
        {            
            Logger.Information("Adding task {0}to job [{1}]...", taskId, jobId);
            List<CloudTask> tasks = new List<CloudTask>();
            string applicationPath = $"%AZ_BATCH_APP_PACKAGE_{options.ApplicatoinName}#{options.ApplicationVersion}%";
            string taskCommandLine = $"cmd /c {applicationPath}\\StockAnalyzer.JobRunner.exe \"{taskId}\" \"{requestId}\"";
            // ToDo: Create the task and specify the input file            
            CloudTask task = new CloudTask($"{taskId}_{Guid.NewGuid()}", taskCommandLine);
            task.DisplayName = taskId;
            task.ResourceFiles = inputDatFiles;
            string containerSasUrl = cloudFileStore.GetContainerSasUrl(BlobContainerSasPermissions.Write);
            task.OutputFiles = new List<OutputFile>
            {
                new OutputFile(
                    filePattern: @"..\std*.txt",
                    destination: new OutputFileDestination(
                        new OutputFileBlobContainerDestination(
                            containerUrl: containerSasUrl,
                            path: $"{taskId}/batchoutput")),
                    uploadOptions: new OutputFileUploadOptions(
                        uploadCondition: OutputFileUploadCondition.TaskCompletion)),
                new OutputFile(
                    filePattern: @".\Logs\*.log",
                    destination: new OutputFileDestination(
                        new OutputFileBlobContainerDestination(
                            containerUrl: containerSasUrl,
                            path: $"{taskId}/AppLogs")),
                    uploadOptions: new OutputFileUploadOptions(
                        uploadCondition: OutputFileUploadCondition.TaskCompletion))
            };
            // ToDo: Add the task to the collection
            tasks.Add(task);
            // ToDo: Add the tasks collection to the job
            await batchClient.JobOperations.AddTaskAsync(jobId, tasks);
            return task;
        }
        private async Task<ImageInformation> GetNodeAgentSkuReferenceAsync(Func<ImageInformation, bool> scanFunc)
        {
            ServicePointManager.DefaultConnectionLimit = 100;
            ServicePointManager.SecurityProtocol = SecurityProtocolType.Ssl3 |
                                                   SecurityProtocolType.Tls |
                                                   SecurityProtocolType.Tls11 |
                                                   SecurityProtocolType.Tls12;
            List<ImageInformation> nodeAgentSkus = await batchClient.PoolOperations.ListSupportedImages().ToListAsync();
            return nodeAgentSkus.First(scanFunc);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="blobPath"></param>
        /// <param name="filePath">The location on the Compute Node to which to download the file(s), relative to the Task's working directory. 
        /// If the httpUrl property is specified, the filePath is required and describes the path which the file will be downloaded to, including the filename.
        /// </param>
        /// <returns></returns>
        public ResourceFile GetResourceFile(string blobPath, string filePath)
        {
            string blobSasUri = cloudFileStore.GetContainerBlobSasUrl(blobPath, Azure.Storage.Sas.BlobSasPermissions.Read);
            return ResourceFile.FromUrl(blobSasUri, filePath);
        }
    }
}
