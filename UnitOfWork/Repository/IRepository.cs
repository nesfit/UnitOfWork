﻿using BaseDataEntity; 
  
namespace Repository
{
    public interface IRepository<TEntity> : IRepositoryReader<TEntity>, IRepositoryReaderAsync<TEntity>, IRepositoryWriter<TEntity>, IRepositoryWriterAsync<TEntity>
        where TEntity : class, IDataEntity, new()
    {
        
    }
    
    
}