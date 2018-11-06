using APITemplate.CQRS.Commands.Interfaces;
using System.ComponentModel;

namespace APITemplate.Logic.TestCommand
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
