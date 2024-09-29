using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using swap_book.Models;
using System.Text.RegularExpressions;

namespace swap_book.Controllers
{
    public class CustomPasswordValidator<TUser> : IPasswordValidator<TUser> where TUser : ApplicationUser
    {
        public Task<IdentityResult> ValidateAsync(UserManager<TUser> manager, TUser user, string password)
        {
            var errors = new List<IdentityError>();

            if (!ContainsLettersAndArithmeticSigns(password))
            {
                errors.Add(new IdentityError
                {
                    Code = "PasswordRequiresLettersAndArithmeticSigns",
                    Description = "Password must contain letters and arithmetic operation signs (+, -, *, /)."
                });
            }

            return Task.FromResult(errors.Count == 0 ? IdentityResult.Success : IdentityResult.Failed(errors.ToArray()));
        }

        private bool ContainsLettersAndArithmeticSigns(string password)
        {
            return Regex.IsMatch(password, @"[a-zA-Z]") && Regex.IsMatch(password, @"[+\-*/]");
        }
    }
}
