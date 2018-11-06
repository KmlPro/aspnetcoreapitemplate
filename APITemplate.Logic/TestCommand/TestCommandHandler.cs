using APITemplate.CQRS.Commands.Interfaces;
using APITemplate.Model.DatabaseContext;
using APITemplate.Model.Model;

namespace APITemplate.Logic.TestCommand
{
    public class TestCommandHandler : IHandleCommand<TestCommand>
    {
        APITemplateContext _context;
        public TestCommandHandler(APITemplateContext context)
        {
            _context = context;
        }
        public void Handle(TestCommand cmd)
        {
            int newValue = int.Parse(cmd.TestCommandValue);

            _context.TestModel.Add(new TestModel() { TestText = newValue });

            _context.SaveChanges();
        }
    }
}
