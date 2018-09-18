using APITemplate._Infrastructure.Commands.Interfaces;
using APITemplate._Infrastructure.Middleware;
using APITemplate._Infrastructure.Queries.Interfaces;
using APITemplate._Infrastructure.Validator;
using APITemplate.Helpers;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Serilog;
using System;

namespace APITemplate._Infrastructure.ICQRS
{
    public class CQRS : ICQRS
    {
        private readonly IQueryBus _queryBus;
        private readonly ICommandBus _commandBus;
        private readonly IHostingEnvironment _hostingEnvironment;

        public CQRS(IQueryBus queryBus, ICommandBus commandBus, IHostingEnvironment hostingEnvironment)
        {
            this._queryBus = queryBus;
            this._commandBus = commandBus;
            _hostingEnvironment = hostingEnvironment;
        }

        public TQueryResult ExecuteQuery<TQuery, TQueryResult>(TQuery query) where TQuery : IQuery
                                                                         where TQueryResult : IQueryResult
        {
            TQueryResult result = default(TQueryResult);
            var typeName = query.GetType().Name;
            var parameters = AssemblyHelper.GetPropertiesWithValues(query);
            var actionId = Guid.NewGuid();

            Log.Information($"GUID: '{actionId}'. Execute query: '{typeName}' z parameters: '{parameters}'");

            try
            {
                result = _queryBus.Process<TQuery, TQueryResult>(query);
            }
            catch (ValidationException validationException)
            {
                LogAndRethrowValidationException(validationException, actionId, typeName, parameters);
            }
            catch (Exception exception)
            {
                LogAndRethrowException(exception, actionId, typeName, parameters);
            }

            Log.Information($"GUID: '{actionId}'. Executed sucesfully ['{typeName}'] z parametrami: '{parameters}'");
            return result;
        }

        public void ExecuteCommand<TCommand>(TCommand command) where TCommand : ICommand
        {
            var typeName = command.GetType().Name;
            var parameters = AssemblyHelper.GetPropertiesWithValues(command);
            var actionId = Guid.NewGuid();

            Log.Information($"GUID: '{actionId}'. Execute command ['{typeName}'] with parameters: '{parameters}'");

            try
            {
                _commandBus.Send(command);
            }
            catch (ValidationException validationException)
            {
                LogAndRethrowValidationException(validationException, actionId, typeName, parameters);
            }
            catch (Exception exception)
            {
                LogAndRethrowException(exception, actionId, typeName, parameters);
            }

            Log.Information($"GUID: '{actionId}'. Executed sucesfully ['{typeName}'] with parameters: '{parameters}'");
        }

        private void LogAndRethrowValidationException(ValidationException validationException, Guid actionId, string typeName, string parameters)
        {
            Log.Warning($"GUID: '{actionId}'. Validation error while executing ['{typeName}'] with parameters: '{parameters}'. Error list: '{string.Join(", ", validationException.Result.Messages)}'");
            throw new HttpStatusCodeException(StatusCodes.Status422UnprocessableEntity, validationException.Result.Messages);
        }

        private void LogAndRethrowException(Exception exception, Guid actionId, string typeName, string parameters)
        {
            string exceptionMessage = null;

            if (_hostingEnvironment.IsProduction())
            {
                exceptionMessage = $"Internal server error. Please send message to developer team";
            }
            else
            {
                exceptionMessage = $"Exception: '{exception}'.";
            }

            Log.Fatal($"GUID: '{actionId}'. Error while executing ['{typeName}'] with parameters: '{parameters}'.\r\n\Exception: '{exception}'");
            throw new HttpStatusCodeException(StatusCodes.Status500InternalServerError, exceptionMessage);
        }
    }
}

