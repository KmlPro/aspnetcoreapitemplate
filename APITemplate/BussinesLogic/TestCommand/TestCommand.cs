using APITemplate._Infrastructure.Commands.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace APITemplate.BussinesLogic.TestCommand
{
    public class TestCommand : ICommand
    {
        public string TestCommandValue;

        public TestCommand(string testCommandValue)
        {
            this.TestCommandValue = testCommandValue;
        }
    }
}
