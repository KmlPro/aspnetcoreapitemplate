using APITemplate._Infrastructure.Commands.Interfaces;
using APITemplate._Infrastructure.Middleware;
using APITemplate._Infrastructure.Queries.Interfaces;
using APITemplate._Infrastructure.Validator;
using APITemplate.Helpers;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Serilog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace APITemplate.Controllers
{
    public class BaseController : ControllerBase
    {
        private IQueryBus _queryBus;
        private ICommandBus _commandBus;

        public BaseController(IQueryBus queryBus, ICommandBus commandBus)
        {
            this._queryBus = queryBus;
            this._commandBus = commandBus;
        }

        public TQueryResult ExecuteQuery<TQuery, TQueryResult>(TQuery query) where TQuery : IQuery
                                                                           where TQueryResult : IQueryResult
        {
            TQueryResult result = default(TQueryResult);
            var actionDecription = AssemblyHelper.GetDescription(typeof(IQuery));
            var parameters = AssemblyHelper.GetPropertiesWithValues(query);
            var queryNumber = Guid.NewGuid();

            Log.Information($"GUID: {queryNumber}. Wywołanie query: {actionDecription} z parametrami: {parameters}");

            try
            {
                result = _queryBus.Process<TQuery, TQueryResult>(query);
            }
            catch (ValidationException ex)
            {
                Log.Debug($"Błąd walidacji query: {actionDecription} z parametrami: {parameters}. Lista błedów: {string.Join(" ,", ex.Result.Messages)}");
                throw new HttpStatusCodeException(StatusCodes.Status409Conflict, $"Lista błedów: {string.Join(" ,", ex.Result.Messages) }");
            }
            catch (Exception ex)
            {
                Log.Fatal($"Błąd serwera przy wywołaniu query: {actionDecription} z parametrami: {parameters}. Wyjątek: {ex}");
                throw new HttpStatusCodeException(StatusCodes.Status409Conflict, $"Wyjątek: {ex}.");
            }

            Log.Information($"GUID: {queryNumber}. Zakończono wywołanie query: {actionDecription} z parametrami: {parameters}");
            return result;
        }

        public void ExecuteCommand<TCommand>(TCommand command) where TCommand : ICommand
        {
            var actionDecription = AssemblyHelper.GetDescription(typeof(TCommand));
            var parameters = AssemblyHelper.GetPropertiesWithValues(command);

            var commandNumber = Guid.NewGuid();

            Log.Information($"GUID: {commandNumber}. Wywołanie command: {actionDecription} z parametrami: {parameters}");

            try
            {
                _commandBus.Send(command);
            }
            catch (ValidationException ex)
            {
                Log.Warning($"Błąd walidacji command: {actionDecription} z parametrami: {parameters}. Lista błedów: {string.Join(" ,", ex.Result.Messages)}");
                throw new HttpStatusCodeException(StatusCodes.Status409Conflict, $"Lista błedów: {string.Join(" ,", ex.Result.Messages) }");
            }
            catch (Exception ex)
            {
                Log.Fatal($"Błąd serwera przy wywołaniu command: {actionDecription} z parametrami: {parameters}. Wyjątek: {string.Join(" ,", ex.InnerException.ToString())}");
                throw new HttpStatusCodeException(StatusCodes.Status500InternalServerError, $"Wyjątek: {ex}.");
            }

            Log.Information($"GUID: {commandNumber}. Zakończono wywołanie command: {actionDecription} z parametrami: {parameters}");

        }
    }
}
