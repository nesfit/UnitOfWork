﻿using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using UnitOfWork.BaseDataEntity;

namespace UnitOfWork.Repository
{
    /// <summary>
    ///     Generic repository reader interface
    /// </summary>
    /// <typeparam name="T">Type must be a class and inherited from IDataEntity interface</typeparam>
    public interface IRepositoryReader<T> where T : class, IDataEntity
    {
        /// <summary>
        ///     Gets all items in persistence storage
        /// </summary>
        /// <returns>All items of type T in persistence storage</returns>
        IEnumerable<T> GetAll();

        /// <summary>
        ///     Gets item by id
        /// </summary>
        /// <param name="id">Id of item to get</param>
        /// <returns>Item if found or null otherwise</returns>
        T GetById(Guid id);
        
        /// <summary>
        ///     Get first item that satisfies a predicate
        /// </summary>
        /// <param name="predicate">Filter predicate</param>
        /// <returns>The first item of type T in persistence storage which satisfy a predicate</returns>
        T GetSingleWhere(Expression<Func<T, bool>> predicate);
        
        /// <summary>
        ///     Get all items filtered by a predicate
        /// </summary>
        /// <param name="predicate">Filter predicate</param>
        /// <returns>All items of type T in persistence storage which satisfy a predicate</returns>
        IEnumerable<T> GetAllWhere(Expression<Func<T, bool>> predicate);
    }
}