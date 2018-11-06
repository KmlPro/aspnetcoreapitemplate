namespace APITemplate.CQRS.Queries.Interfaces
{
    public interface IHandleQuery<in TQuery, TResult> where TQuery : IQuery where TResult : IQueryResult
    {
        TResult Execute(TQuery query);
    }
}
