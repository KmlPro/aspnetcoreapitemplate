namespace APITemplate.CQRS.Commands.Interfaces
{
    public interface ICommandBus
    {
        void Send<TCommand>(TCommand command) where TCommand : ICommand;
    }
}
