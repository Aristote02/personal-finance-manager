using Microsoft.EntityFrameworkCore;
using PersonalFinanceManager.Infrastructure.Contracts.Repositories.Interfaces;
using PersonalFinanceManager.Infrastructure.Data;

namespace PersonalFinanceManager.Infrastructure.Repositories.Implementations;

public abstract class RepositoryBase<T> : IRepositoryBase<T> where T : class
{
    protected PersonalFinanceDbContext _dbContext;

    protected RepositoryBase(PersonalFinanceDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public void Create(T entity) => _dbContext.Set<T>().Add(entity);

    public void Update(T entity) => _dbContext.Set<T>().Update(entity);

    public void Delete(T entity) => _dbContext.Set<T>().Remove(entity);

    public IQueryable<T> FindAll(bool trackChanges) =>
        !trackChanges
            ? _dbContext.Set<T>().AsNoTracking()
            : _dbContext.Set<T>();
}