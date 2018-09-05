using APITemplate._Infrastructure.Commands.Interfaces;
using APITemplate._Infrastructure.Validator;
using APITemplate._Infrastructure.Validator.Interfaces;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace APITemplate._Infrastructure.Commands
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
