using Git.Data;
using Git.Data.Models;
using Git.Models;
using Git.Services;
using MyWebServer.Controllers;
using MyWebServer.Http;
using System.Linq;

namespace Git.Controllers
{
    public class UsersController : Controller
    {
        private readonly IValidator validator;
        private readonly GitDbContext dbContext;
        private readonly IHashedPassword hashedPassword;

        public UsersController(IValidator validator, GitDbContext dbContext, IHashedPassword hashedPassword)
        {
            this.validator = validator;
            this.dbContext = dbContext;
            this.hashedPassword = hashedPassword;
        }

        public HttpResponse Register() => this.View();

        [HttpPost]
        public HttpResponse Register(RegisterUserFormModel model)
        {
            var modelErrors = this.validator.ValidateUser(model);

            if (this.dbContext.Users.Any(u => u.Username == model.Username))
            {
                modelErrors.Add($"User with '{model.Username}' username already exists.");
            }

            if (this.dbContext.Users.Any(u => u.Email == model.Email))
            {
                modelErrors.Add($"User with '{model.Email}' e-mail already exists.");
            }

            if (modelErrors.Any())
            {
                return Error(modelErrors);
            }

            var user = new User
            {
                Username = model.Username,
                Password = this.hashedPassword.HashPassword(model.Password),
                Email = model.Email,
            };

            this.dbContext.Users.Add(user);
            this.dbContext.SaveChanges();

            return this.Redirect("/Users/Login");
        }

        public HttpResponse Login() => this.View();

        [HttpPost]
        public HttpResponse Login(LoginUserFormModel model)
        {
            var hashedPassword = this.hashedPassword.HashPassword(model.Password);

            var userId = this.dbContext
                .Users
                .Where(u => u.Username == model.Username && u.Password == hashedPassword)
                .Select(x => x.Id)
                .FirstOrDefault();

            if (userId == null)
            {
                return this.Error("Username and password combination is not valid.");
            }

            this.SignIn(userId);

            return this.Redirect("/Repositories/All");
        }

        public HttpResponse Logout()
        {
            this.SignOut();

            return this.Redirect("/");
        }
    }
}
