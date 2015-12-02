namespace DataBus.Data.Commands
{
    public interface IDataCommandExecutor
    {
        void Execute(IDataCommand cmd);
    }
}