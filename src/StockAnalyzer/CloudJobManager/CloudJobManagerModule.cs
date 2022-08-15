using Autofac;
using StockAnalyzer.CloudJobManager.AzureBatch;
using StockAnalyzer.FileStorage;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StockAnalyzer.CloudJobManager
{
    public class CloudJobManagerModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            int maxPoolNodes = -1;
            if (!Int32.TryParse(ConfigurationManager.AppSettings["BatchPoolMaxNodes"], out maxPoolNodes))
            {
                maxPoolNodes = 20;
            }

            AzureBatchJobManagerOptions batchJobManagerOptions = new AzureBatchJobManagerOptions(
                  ConfigurationManager.AppSettings["BatchAccountName"],
                 ConfigurationManager.AppSettings["BatchAccountKey"],
                 ConfigurationManager.AppSettings["BatchAccountUrl"],
                 ConfigurationManager.AppSettings["BatchAppName"],
                 ConfigurationManager.AppSettings["BatchAppVersion"],
                 maxPoolNodes,
                 ConfigurationManager.AppSettings["BatchVirtualMachineSize"]
                 );
            builder.Register(c => new AzureBatchJobManager(batchJobManagerOptions, c.Resolve<ICloudFileStore>())).As<ICloudJobManager>();

        }
    }
}