namespace QLTS.Contract.Abstractions.Shared;
public interface IValidationResult
{

    public static readonly Error ValidationError = new Error("ValidationError", "A valdation occurred");
    Error[] Errors { get;  }
}
