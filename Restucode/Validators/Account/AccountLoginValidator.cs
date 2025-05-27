using FluentValidation;
using Microsoft.EntityFrameworkCore;
using Restucode.Data;
using Restucode.Models.Account;
using System.Threading;

public class AccountLoginValidator : AbstractValidator<LoginModel>
{
    public AccountLoginValidator(RestucodeDBContext db)
    {
        RuleFor(x => x.Email)
            .Must(email => !string.IsNullOrEmpty(email)).WithMessage("Email є обов'язковою");

        RuleFor(x => x.Password)
            .Must(password => !string.IsNullOrEmpty(password)).WithMessage("Password є обов'язковою");
    }
}
