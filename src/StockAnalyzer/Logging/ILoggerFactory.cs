using System;

namespace StockAnalyzer.Logging {
    public interface ILoggerFactory {
        ILogger CreateLogger(Type type);
    }
}