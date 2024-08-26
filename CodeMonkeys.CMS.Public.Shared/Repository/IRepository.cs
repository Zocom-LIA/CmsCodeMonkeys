using CodeMonkeys.CMS.Public.Shared.Entities;

namespace CodeMonkeys.CMS.Public.Shared.Repository
{
    public interface IRepository<TEntity> where TEntity : class, IEntity
    {
    }
}