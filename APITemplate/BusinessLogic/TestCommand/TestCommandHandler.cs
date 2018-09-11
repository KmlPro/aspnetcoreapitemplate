using APITemplate._Infrastructure.Commands.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace APITemplate.BusinessLogic.TestCommand
{
    public class TestCommandHandler : IHandleCommand<TestCommand>
    {
        public void Handle(TestCommand cmd)
        {
            //some logic
        }
    }
}
