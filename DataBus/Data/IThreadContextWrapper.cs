namespace DataBus.Data
{
    public interface IThreadContextWrapper
    { 
        IDataContext CurrentDataContext { get; set; }
    }
}