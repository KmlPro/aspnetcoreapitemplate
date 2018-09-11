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

            Log.Information($"GUID: '{queryNumber}'. Execute query: '{actionDecription}' z parameters: '{parameters}'");

            try
            {
                result = _queryBus.Process<TQuery, TQueryResult>(query);
            }
            catch (ValidationException ex)
            {
                Log.Debug($"Query validation error: '{actionDecription}' with parameters: '{parameters}'. Validation Errors: '{string.Join(", ", ex.Result.Messages)}'");
                throw new HttpStatusCodeException(StatusCodes.Status422UnprocessableEntity, ex.Result.Messages);
            }
            catch (Exception ex)
            {
                Log.Fatal($"Internal server error on query: '{actionDecription}' with parameters: '{parameters}'. Exception: '{ex}'");
#if DEBUG
                throw new HttpStatusCodeException(StatusCodes.Status500InternalServerError, $"Exception: '{ex}'.");
#else
                throw new HttpStatusCodeException(StatusCodes.Status500InternalServerError, $"Internal Server Error. Please contact with service developer");
#endif

            }

            Log.Information($"GUID: '{queryNumber}'. Query executed correctly: '{actionDecription}' with parameters: '{parameters}'");
            return result;
        }

        public void ExecuteCommand<TCommand>(TCommand command) where TCommand : ICommand
        {
            var actionDecription = AssemblyHelper.GetDescription(typeof(TCommand));
            var parameters = AssemblyHelper.GetPropertiesWithValues(command);

            var commandNumber = Guid.NewGuid();

            Log.Information($"GUID: '{commandNumber}'. Execute command: '{actionDecription}' with parameters: '{parameters}'");

            try
            {
                _commandBus.Send(command);
            }
            catch (ValidationException ex)
            {
                Log.Warning($"Command validation error: '{actionDecription}' with parameters: '{parameters}'. Validation errors: '{string.Join(" ,", ex.Result.Messages)}'");
                throw new HttpStatusCodeException(StatusCodes.Status422UnprocessableEntity, ex.Result.Messages);
            }
            catch (Exception ex)
            {
                Log.Fatal($"Internal server error on query: '{actionDecription}' with parameters: '{parameters}'. Exception: '{string.Join(" ,", ex.InnerException.ToString())}'");
#if DEBUG
                throw new HttpStatusCodeException(StatusCodes.Status500InternalServerError, $"Exception: '{ex}'.");
#else
                throw new HttpStatusCodeException(StatusCodes.Status500InternalServerError, $"Internal Server Error. Please contact with service developer");
#endif

            }

            Log.Information($"GUID: '{commandNumber}'. Query executed correctly: '{actionDecription}' with parameters: '{parameters}'");

        }
    }
}
