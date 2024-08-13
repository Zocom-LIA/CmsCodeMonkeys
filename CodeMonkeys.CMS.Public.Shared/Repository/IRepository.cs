using CodeMonkeys.CMS.Public.Shared.Repository;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using System.Linq.Expressions;

namespace CodeMonkeys.CMS.Public.Shared.Repository
{
    public interface IRepository<TEntity> where TEntity : class, IEntity
    {
        Task<TEntity?> AddAsync(TEntity entity, CancellationToken cancellation = default);
        Task<int> AddRangeAsync(IEnumerable<TEntity> entities, CancellationToken cancellation = default);
        Task<int> CountAsync(CancellationToken cancellation = default);
        Task<IEnumerable<TEntity>> FindAsync(Expression<Func<TEntity, bool>> predicate, CancellationToken cancellation = default);
        Task<IEnumerable<TEntity>> GetAllAsync(int pageIndex = 0, int pageSize = 10, CancellationToken cancellation = default);
        Task<TEntity?> GetByIdAsync(object id, CancellationToken cancellation = default);
        Task<int> RemoveAsync(TEntity entity, CancellationToken cancellation = default);
        Task<int> RemoveRangeAsync(IEnumerable<TEntity> entities, CancellationToken cancellation = default);
        Task<TEntity?> UpdateAsync(TEntity entity, CancellationToken cancellation = default);
        Task<IEnumerable<TEntity>> UpdateRangeAsync(TEntity entity, CancellationToken cancellation = default);
    }
}