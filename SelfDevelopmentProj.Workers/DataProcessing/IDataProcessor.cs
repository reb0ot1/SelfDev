using SelfDevelopmentProj.Workers.Data;

namespace SelfDevelopmentProj.Workers.DataProcessing
{
    public interface IDataProcessor
    {
        Task ScheduledDataProcessing(DataWithKey dataWithKey);
    }
}