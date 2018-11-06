using APITemplate.CQRS.Commands.Interfaces;
using APITemplate.CQRS.Queries.Interfaces;

namespace APITemplate.CQRS.MainInterface
{
    public interface ICQRS
    {
        void ExecuteCommand<TCommand>(TCommand command) where TCommand : ICommand;

        TQueryResult ExecuteQuery<TQuery, TQueryResult>(TQuery query) where TQuery : IQuery
                                                                      where TQueryResult : IQueryResult;
    }
}
