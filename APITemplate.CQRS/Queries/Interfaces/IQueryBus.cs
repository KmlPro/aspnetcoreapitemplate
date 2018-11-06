namespace APITemplate.CQRS.Queries.Interfaces
{
    public interface IQueryBus
    {
        TResult Process<TQuery, TResult>(TQuery query) where TQuery : IQuery where TResult : IQueryResult;
    }
}
