using ClassLibrary1.InterfaceRepository.IPostgresRepo;
using System.Data;


namespace ClassLibrary1.Repository.PostgresRepo
{
    public class BasePostgresRepository : IBasePostgresRepository
    {
        private readonly IDbConnection _db;
        private IDbTransaction _transaction;
        public BasePostgresRepository(IDbConnection db)
        {
            _db = db;

        }
        public async Task BeginTransactionAsync()
        {
            if (_transaction != null)
            {
                //throw new InvalidOperationException("Transaction is already started.");
            }
            _db.Open();

            _transaction = _db.BeginTransaction();
        }

        public async Task CommitTransactionAsync()
        {
            if (_transaction == null)
            {
                //throw new InvalidOperationException("Transaction is not started.");
            }

            _transaction.Commit();
            _transaction.Dispose();
            _transaction = null;
        }

        public async Task RollbackTransactionAsync()
        {
            if (_transaction == null)
            {
                //throw new InvalidOperationException("Transaction is not started.");
            }

            _transaction.Rollback();
            _transaction = null;
        }
    }
}
