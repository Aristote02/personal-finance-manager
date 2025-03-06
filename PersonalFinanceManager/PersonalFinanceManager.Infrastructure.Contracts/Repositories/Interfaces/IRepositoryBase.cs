namespace PersonalFinanceManager.Infrastructure.Contracts.Repositories.Interfaces;

public interface IRepositoryBase<T>
{
    IQueryable<T> FindAll(bool trackChanges);

    void Create(T entity);

    void Update(T entity);

    void Delete(T entity);
}