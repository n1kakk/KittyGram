namespace UserServiceDAL.InterfaceRepository
{
    public interface IBaseRepository
    {
        public Task BeginTransactionAsync();
        public Task CommitTransactionAsync();
        public Task RollbackTransactionAsync();

    }
}
