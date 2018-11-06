namespace APITemplate.CQRS.Validator.Interfaces
{
    public interface IValidator<TAction> where TAction : IValidatable
    {
        ValidationResult Validate(TAction command);
    }
}
