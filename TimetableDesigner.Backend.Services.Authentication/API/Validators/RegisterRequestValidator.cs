using FluentValidation;
using Microsoft.EntityFrameworkCore;
using TimetableDesigner.Backend.Services.Authentication.Database;
using TimetableDesigner.Backend.Services.Authentication.DTO.API;

namespace TimetableDesigner.Backend.Services.Authentication.DTO.Validators;

public class RegisterRequestValidator : AbstractValidator<RegisterRequest>
{
    private readonly DatabaseContext _databaseContext;
    
    public RegisterRequestValidator(DatabaseContext databaseContext)
    {
        _databaseContext = databaseContext;
        
        RuleFor(x => x.Email)
           .NotEmpty()
           .EmailAddress()
           .MustAsync(EmailNotUsed).WithMessage("Email already used");
        RuleFor(x => x.Password)
           .NotEmpty()
           .MinimumLength(8)
           .Must(x => x.Any(char.IsUpper)).WithMessage("Password must contain at least one uppercase letter")
           .Must(x => x.Any(char.IsLower)).WithMessage("Password must contain at least one lowercase letter")
           .Must(x => x.Any(char.IsDigit)).WithMessage("Password must contain at least one digit");
        RuleFor(x => x.PasswordConfirmation)
           .NotEmpty()
           .Equal(x => x.Password);
    }

    private Task<bool> EmailNotUsed(string value, CancellationToken cancellationToken) => 
        _databaseContext.Accounts.AnyAsync(x => x.Email == value, cancellationToken);
}