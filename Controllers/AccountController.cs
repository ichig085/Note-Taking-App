using System.Security.Claims;
using Auth0.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NoteItEasyApp.Data;
using NoteItEasyApp.Models;


namespace NoteItEasyApp.Controllers
{
    public class AccountController : Controller
    {
        private readonly DataContext _dbContext;

        public AccountController(DataContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task Login(string returnUrl = "/")
        {
            var authenticationProperties = new LoginAuthenticationPropertiesBuilder()
                .WithRedirectUri(returnUrl)
                .Build();

            await HttpContext.ChallengeAsync(Auth0Constants.AuthenticationScheme, authenticationProperties);
        }

        [Authorize]
        public async Task Logout()
        {
            var authenticationProperties = new LogoutAuthenticationPropertiesBuilder()
                // Indicate here where Auth0 should redirect the user after a logout.
                // Note that the resulting absolute Uri must be whitelisted in 
                .WithRedirectUri(Url.Action("Index", "Home"))
                .Build();

            await HttpContext.SignOutAsync(Auth0Constants.AuthenticationScheme, authenticationProperties);
        }

        [Authorize]
        public async Task<IActionResult> ProfileAsync()
        {
            // Get user data from the Auth0 claims
            var auth0UserId = User.Claims.FirstOrDefault(c => c.Type == "sub")?.Value;
            var name = User.Identity.Name;
            var emailAddress = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Email)?.Value;
            var profileImage = User.Claims.FirstOrDefault(c => c.Type == "picture")?.Value;

            // Check if the user with the Auth0 user ID already exists in your database
            var existingUser = await _dbContext.UserModels.FirstOrDefaultAsync(u => u.Id == auth0UserId);

            if (existingUser == null)
            {
                // Create a new user since one doesn't exist.
                var newUser = new UserModel
                {
                    Id = auth0UserId,
                    Name = name,
                    Email = emailAddress,
                    ProfileImage = profileImage,
                    // Map other properties here.
                };

                _dbContext.UserModels.Add(newUser);
            }
            else
            {
                // Update the existing user's information.
                existingUser.Name = name;
                existingUser.Email = emailAddress;
                existingUser.ProfileImage = profileImage;
                // Update other properties here.
            }

            await _dbContext.SaveChangesAsync();

            return View(new UserModel()
            {
                Name = name,
                Email = emailAddress,
                ProfileImage = profileImage
            });
        }
    }
}

