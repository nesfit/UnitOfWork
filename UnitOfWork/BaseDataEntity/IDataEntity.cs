using System;

namespace BaseDataEntity
{
    /// <summary>
    ///     Base interface for all data entities in project
    /// </summary>
    public interface IDataEntity
    {
        Guid Id { get; set; }
    }
}