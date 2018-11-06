namespace APITemplate.CQRS.Commands.Interfaces
{
    public interface IHandleCommand<in TCommand> where TCommand : ICommand
    {
        void Handle(TCommand cmd);
    }
}
