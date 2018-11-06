using APITemplate.CQRS.Commands.Interfaces;
using APITemplate.CQRS.Validator;
using APITemplate.CQRS.Validator.Interfaces;
using Microsoft.Extensions.DependencyInjection;
using System;

namespace APITemplate.CQRS.Commands
{
    public class CommandBus : ICommandBus
    {
        private readonly IServiceProvider _serviceProvider;
        public CommandBus(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;

        }
        public void Send<TCommand>(TCommand cmd) where TCommand : ICommand
        {
            using (var scope = _serviceProvider.CreateScope())
            {
                var commandValidator = (IValidator<TCommand>)scope.ServiceProvider
                      .GetRequiredService(typeof(IValidator<TCommand>));

                if (commandValidator != null)
                {
                    var res = commandValidator.Validate(cmd);
                    if (res.IsValid == false)
                    {
                        throw new ValidationException(res);
                    }
                }
                var commandHandler = (IHandleCommand<TCommand>)scope.ServiceProvider
                      .GetRequiredService(typeof(IHandleCommand<TCommand>)); 
                if (commandHandler == null)
                {
                    throw new Exception(string.Format("No handler found for command '{0}'", cmd.GetType().FullName));
                }

                commandHandler.Handle(cmd);
            }
        }
    }
}
