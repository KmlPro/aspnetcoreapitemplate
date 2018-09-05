using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace APITemplate._Infrastructure.Commands.Interfaces
{
    interface IHandleCommand<in TCommand> where TCommand : ICommand
    {
        void Handle(TCommand cmd);
    }
}
