
namespace NMF.Expressions
{
    /// <summary>
    /// Represents an incremental reversable expression
    /// </summary>
    /// <typeparam name="T">The expression type</typeparam>
    public interface INotifyReversableExpression<T> : INotifyExpression<T>, INotifyReversableValue<T>
    {
    }
}
