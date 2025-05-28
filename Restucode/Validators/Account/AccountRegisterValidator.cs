using FluentValidation;
using Microsoft.EntityFrameworkCore;
using Restucode.Data;
using Restucode.Models.Account;
using Microsoft.AspNetCore.Identity;
using Restucode.Data.Entities.Identity;

namespace Restucode.Validators.Category
{
	public class AccountRegisterValidator: AbstractValidator<RegisterModel>
	{
		public AccountRegisterValidator(UserManager<UserEntity> userManager)
		{
	        RuleFor(x => x.Email)
	            .NotEmpty().WithMessage("Електронна пошта є обов'язковою")
	            .EmailAddress().WithMessage("Некоректний формат електронної пошти")
	            .MustAsync(async (email, cancellation) =>
	            {
	                var user = await userManager.FindByEmailAsync(email);
	                return user == null;
	            }).WithMessage("User with this Email already exists!");

	        RuleFor(x => x.Password)
	            .NotEmpty().WithMessage("Пароль є обов'язковим")
	            .MinimumLength(6).WithMessage("Пароль повинен містити щонайменше 6 символів");
				
            RuleFor(x => x.FirstName)
                .NotEmpty()
                .WithMessage("Назва є обов'язковою")
                .MaximumLength(250)
                .WithMessage("Назва повинна містити не більше 250 символів");
				
            RuleFor(x => x.LastName)
                .NotEmpty()
                .WithMessage("Назва є обов'язковою")
                .MaximumLength(250)
                .WithMessage("Назва повинна містити не більше 250 символів");
				
	        RuleFor(x => x.Image)
	            .NotEmpty().WithMessage("Image is required!");
		}
	}
}
