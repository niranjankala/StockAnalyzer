using Microsoft.Azure.Batch;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StockAnalyzer.CloudJobManager
{
    public interface ICloudJobManager
    {
        void RunTask(string modelName);
        //Task RunTaskAsync(string modelName);
        Task RunTaskAsync(string modelName, Guid RequestIdentifier);
    }
}
