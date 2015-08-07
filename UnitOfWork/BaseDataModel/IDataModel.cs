using System;

namespace BaseDataModel
{
    /// <summary>
    /// All data models in project must be inheritened from IDataModel interface
    /// </summary>
    public interface IDataModel
    {
        Guid Id { get; set; } 
    }
}