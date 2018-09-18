using APITemplate._Infrastructure.Commands.Interfaces;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;

namespace APITemplate.BusinessLogic.TestCommand
{
    [Description("Test command description")]
    public class TestCommand : ICommand
    {
        public string TestCommandValue { get; set; }

        public TestCommand(string testCommandValue)
        {
            this.TestCommandValue = testCommandValue;
        }
    }
}
