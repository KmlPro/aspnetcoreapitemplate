using APITemplate.Common.Exceptions;
using APITemplate.CQRS.Commands.Interfaces;
using APITemplate.CQRS.LogUtils;
using APITemplate.CQRS.MainInterface;
using APITemplate.CQRS.Queries.Interfaces;
using APITemplate.CQRS.Validator;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Serilog;
using System;

namespace APITemplate.CQRS.Main
{
    public class CQRS : ICQRS
    {
        private readonly IQueryBus _queryBus;
        private readonly ICommandBus _commandBus;
        private readonly IHostingEnvironment _hostingEnvironment;

        public CQRS(IQueryBus queryBus, ICommandBus commandBus, IHostingEnvironment hostingEnvironment)
        {
            _queryBus = queryBus;
            _commandBus = commandBus;
            _hostingEnvironment = hostingEnvironment;
        }

        public TQueryResult ExecuteQuery<TQuery, TQueryResult>(TQuery query) where TQuery : IQuery
                                                                         where TQueryResult : IQueryResult
        {
            TQueryResult result = default(TQueryResult);
            ActionLog actionLog = new ActionLog(query);

            LogCallStarted(actionLog);

            try
            {
                result = _queryBus.Process<TQuery, TQueryResult>(query);
            }
            catch (ValidationException validationException)
            {
                LogAndRethrowValidationException(validationException, actionLog);
            }
            catch (Exception exception)
            {
                LogAndRethrowException(exception, actionLog);
            }

            LogCallHasFinished(actionLog);

            return result;
        }

        public void ExecuteCommand<TCommand>(TCommand command) where TCommand : ICommand
        {
            ActionLog actionLog = new ActionLog(command);

            LogCallStarted(actionLog);

            try
            {
                _commandBus.Send(command);
            }
            catch (ValidationException validationException)
            {
                LogAndRethrowValidationException(validationException, actionLog);
            }
            catch (Exception exception)
            {
                LogAndRethrowException(exception, actionLog);
            }

            LogCallHasFinished(actionLog);
        }

        private void LogAndRethrowValidationException(ValidationException validationException, ActionLog actionLog)
        {
            Log.Warning($"GUID: '{actionLog.ActionId}'. Validation error during the call ['{actionLog.TypeName}']['{actionLog.ActionDescription}'] with parameters: '{actionLog.Parameters}'. Error list: '{string.Join(", ", validationException.Result.Messages)}'");
            throw new HttpStatusCodeException(StatusCodes.Status422UnprocessableEntity, validationException.Result.Messages);
        }

        private void LogAndRethrowException(Exception exception, ActionLog actionLog)
        {
            string exceptionMessage = null;

            if (_hostingEnvironment.IsProduction())
            {
                exceptionMessage = $"Error on the server side. Contact the service provider";
            }
            else
            {
                exceptionMessage = $"Exception: '{exception}'.";
            }

            Log.Fatal($"GUID: '{actionLog.ActionId}'. Server error during the call ['{actionLog.TypeName}']['{actionLog.ActionDescription}'] with parameters: '{actionLog.Parameters}'.\r Exception: '{exception}'");
            throw new HttpStatusCodeException(StatusCodes.Status500InternalServerError, exceptionMessage);
        }

        private void LogCallStarted(ActionLog actionLog)
        {
            Log.Information($"GUID: '{actionLog.ActionId}'. The call has started ['{actionLog.TypeName}']['{actionLog.ActionDescription}'] with paramteres: '{actionLog.Parameters}'");
        }

        private void LogCallHasFinished(ActionLog actionLog)
        {
            Log.Information($"GUID: '{actionLog.ActionId}'. The call has been finished ['{actionLog.TypeName}']['{actionLog.ActionDescription}'] with parameters: '{actionLog.Parameters}'");
        }
    }
}


