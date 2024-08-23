namespace Couple_Quiz.Interface.Repositories
{
    public interface IBaseRepository<TEntity>
    {
        public Task<TEntity> Create(TEntity entity);
        public Task<TEntity> Update(TEntity entity);
        public Task<TEntity> Delete(TEntity entity);
        public Task<List<TEntity>> Get();
      
    }
}
