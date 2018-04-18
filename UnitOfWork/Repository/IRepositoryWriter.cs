﻿using System;
using System.Collections.Generic;
using UnitOfWork.BaseDataEntity;

namespace UnitOfWork.Repository
{
    /// <summary>
    ///     Generic writer repository interface
    /// </summary>
    /// <typeparam name="T">Type must be a class and inherited from IDataEntity interface</typeparam>
    public interface IRepositoryWriter<T> where T : class, IDataEntity
    {
        /// <summary>
        ///     Deletes item
        /// </summary>
        /// <param name="id">Id of item to delete</param>
        /// <returns></returns>
        T Delete(Guid id);

        /// <summary>
        ///     Deletes item
        /// </summary>
        /// <param name="item">Item to delete</param>
        /// <returns></returns>
        T Delete(T item);

        /// <summary>
        ///     Inserts new item
        /// </summary>
        /// <param name="item">Item to add</param>
        /// <returns>Newly created item</returns>
        T Insert(T item);

        /// <summary>
        ///     Inserts range of items
        /// </summary>
        /// <param name="items">Items to add</param>
        /// <returns>Newly created items</returns>
        IEnumerable<T> InsertRange(IEnumerable<T> items);

        /// <summary>
        ///     Updates existing item
        /// </summary>
        /// <param name="item">Item to update</param>
        void Update(T item);
    }
}