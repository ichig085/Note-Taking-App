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

        // Action method for initiating the login process
        public async Task Login(string returnUrl = "/")
        {
            // Build authentication properties for the login process
            var authenticationProperties = new LoginAuthenticationPropertiesBuilder()
                .WithRedirectUri(returnUrl)
                .Build();

            // Challenge the user to authenticate using Auth0
            await HttpContext.ChallengeAsync(Auth0Constants.AuthenticationScheme, authenticationProperties);
        }

        // Action method for logging out the user
        [Authorize]
        public async Task Logout()
        {
            // Build authentication properties for the logout process
            var authenticationProperties = new LogoutAuthenticationPropertiesBuilder()
                .WithRedirectUri(Url.Action("Index", "Home"))
                .Build();

            // Sign the user out using Auth0
            await HttpContext.SignOutAsync(Auth0Constants.AuthenticationScheme, authenticationProperties);
        }

        // Action method for displaying the user's profile
        [Authorize]
        public async Task<IActionResult> ProfileAsync()
        {
            // Get user data from the Auth0 claims
            var auth0UserId = User.Claims.FirstOrDefault(c => c.Type == "http://schemas.xmlsoap.org/ws/2005/05/identity/claims/nameidentifier")?.Value;
            var name = User.Identity.Name;
            var profileImage = User.Claims.FirstOrDefault(c => c.Type == "picture")?.Value;

            // Check if the user with the Auth0 user ID already exists in your database
            var existingUser = await _dbContext.UserModels.FirstOrDefaultAsync(u => u.Id == auth0UserId);

            if (existingUser == null)
            {
                // Create a new user since one doesn't exist
                var newUser = new UserModel
                {
                    Id = auth0UserId,
                    Name = name,
                    ProfileImage = profileImage,
                };

                _dbContext.UserModels.Add(newUser);
            }
            else
            {
                // Update the existing user's information
                existingUser.Name = name;
                existingUser.ProfileImage = profileImage;
            }

            // Save changes to the database
            await _dbContext.SaveChangesAsync();

            // Return a view (you may need to specify the view name)
            return View();
        }
    }
}