using APITemplate._Infrastructure.Commands.Interfaces;
using APITemplate._Infrastructure.Queries.Interfaces;

namespace APITemplate._Infrastructure.ICQRS
{
    public interface ICQRS
    {
        void ExecuteCommand<TCommand>(TCommand command) where TCommand : ICommand;

        TQueryResult ExecuteQuery<TQuery, TQueryResult>(TQuery query) where TQuery : IQuery
                                                                      where TQueryResult : IQueryResult;
    }
}
