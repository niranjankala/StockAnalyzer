using System;
using System.Linq;
using System.Text;

namespace StockAnalyzer.CloudJobManager.AzureBatch
{
    public class AzureBatchJobManagerOptions
    {
        public string BatchAccountName { get; private set; }

        public string BatchAccountKey { get; private set; }

        public string BatchAccountUrl { get; private set; }
        public string ApplicatoinName { get; private set; }
        public string ApplicationVersion { get; private set; }

        public string StorageAccountName { get; private set; }

        public string StorageAccountKey { get; private set; }

        public string StorageAccountUrl { get; private set; }

        public string BatchPoolId { get; private set; } = "DefaultAutoscalePool";
        public string BatchJobId { get; private set; } = "DefaultJob";
        public int BatchPoolMaxNodes { get; set; } = 20;
        public string BatchVirtualMachineSize { get; set; } = "standard_d1_v2";

        public AzureBatchJobManagerOptions(
            string batchAccountName,
            string batchAccountKey,
            string batchAccountUrl,
            string applicationName,
            string applicationVersion,
            int batchPoolMaxNodes,
            string batchVirtualMachineSize)
        {
            this.BatchAccountName = batchAccountName;
            this.BatchAccountKey = batchAccountKey;
            this.BatchAccountUrl = batchAccountUrl;
            this.ApplicatoinName = applicationName;
            this.ApplicationVersion = applicationVersion;
            this.BatchPoolMaxNodes = batchPoolMaxNodes;
            this.BatchVirtualMachineSize = batchVirtualMachineSize;        }

        public AzureBatchJobManagerOptions(
            string batchAccountName,
            string batchAccountKey,
            string batchAccountUrl,
            string applicationName,
            string applicationVersion,
            int batchPoolMaxNodes,
            string batchVirtualMachineSize,
            string storageAccountName,
            string storageAccountKey,
            string storageAccountUrl)
        {
            this.BatchAccountName = batchAccountName;
            this.BatchAccountKey = batchAccountKey;
            this.BatchAccountUrl = batchAccountUrl;
            this.ApplicatoinName = applicationName;
            this.ApplicationVersion = applicationVersion;
            this.BatchPoolMaxNodes = batchPoolMaxNodes;
            this.BatchVirtualMachineSize = batchVirtualMachineSize;
            this.StorageAccountName = storageAccountName;
            this.StorageAccountKey = storageAccountKey;
            this.StorageAccountUrl = storageAccountUrl;
        }

        public override string ToString()
        {
            StringBuilder stringBuilder = new StringBuilder();

            stringBuilder.AppendFormat("{0} = {1}", "BatchAccountName", this.BatchAccountName).AppendLine();
            stringBuilder.AppendFormat("{0} = {1}", "BatchAccountKey", this.BatchAccountKey).AppendLine();
            stringBuilder.AppendFormat("{0} = {1}", "BatchAccountUrl", this.BatchAccountUrl).AppendLine();
            stringBuilder.AppendFormat("{0} = {1}", "BatchPoolMaxNodes", this.BatchPoolMaxNodes).AppendLine();
            stringBuilder.AppendFormat("{0} = {1}", "BatchVirtualMachineSize", this.BatchVirtualMachineSize).AppendLine();

            stringBuilder.AppendFormat("{0} = {1}", "StorageAccountName", this.StorageAccountName).AppendLine();
            stringBuilder.AppendFormat("{0} = {1}", "StorageAccountKey", this.StorageAccountKey).AppendLine();
            stringBuilder.AppendFormat("{0} = {1}", "storageAccountUrl", this.StorageAccountUrl).AppendLine();

            return stringBuilder.ToString();
        }
    }
}