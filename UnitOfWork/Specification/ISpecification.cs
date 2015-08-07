using BaseDataModel;

namespace Specification
{
    /// <summary>
    /// Specification pattern interface for filtering usage
    /// </summary>
    /// <typeparam name="T">Type must be inherited from IDataModel interface</typeparam>
    public interface ISpecification<T> where T : IDataModel
    {
        /// <summary>
        /// Method to check whether specification is satisfied or not
        /// </summary>
        /// <param name="item"></param>
        /// <returns>Value that represents whether specification is satisfied or not</returns>
        bool IsSatisfiedBy(T item);

        /// <summary>
        /// Method to disjunct this specification with another
        /// </summary>
        /// <param name="specification">Specification to chain</param>
        /// <returns>Disjunction of specifications</returns>
        ISpecification<T> And(ISpecification<T> specification);

        /// <summary>
        /// Method to conjunct this specification with another
        /// </summary>
        /// <param name="specification"></param>
        /// <returns>Conjunction of specifications</returns>
        ISpecification<T> Or(ISpecification<T> specification);

        /// <summary>
        /// Method to neglect this specification
        /// </summary>
        /// <param name="specification"></param>
        /// <returns></returns>
        ISpecification<T> Not(ISpecification<T> specification);
    }
}