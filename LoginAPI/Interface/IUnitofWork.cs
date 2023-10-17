namespace LoginAPI.Interface
{
    public interface IUnitofWork
    {
        iToDoItemRepository todoitemRepository { get; }
        Task CompleteAsync();
    }
}
