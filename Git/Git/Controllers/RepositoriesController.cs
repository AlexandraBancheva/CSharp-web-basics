using Git.Data;
using Git.Data.Models;
using Git.Models;
using Git.Services;
using MyWebServer.Controllers;
using MyWebServer.Http;
using System.Linq;

using static Git.Data.DataConstants;

namespace Git.Controllers
{
    public class RepositoriesController : Controller
    {
        private readonly IValidator validator;
        private readonly GitDbContext dbContext;

        public RepositoriesController(IValidator validator, GitDbContext dbContext)
        {
            this.validator = validator;
            this.dbContext = dbContext;
        }

        [Authorize]
        public HttpResponse Create() => this.View();

        [Authorize]
        [HttpPost]
        public HttpResponse Create(CreateRepoFormModel model)
        {
            var modelErrors = this.validator.ValidateRepository(model);

            if (modelErrors.Any())
            {
                return this.Error(modelErrors);
            }

            var repository = new Repository
            {
                Name = model.Name,
                IsPublic = model.RepositoryType == RepositoryTypePublic,
                OwnerId = this.User.Id,
            };

            this.dbContext.Repositories.Add(repository);
            this.dbContext.SaveChanges();

            return this.Redirect("/Repositories/All");
        }

        [Authorize]
        public HttpResponse All()
        {
            var repositoriesQuery = this.dbContext.Repositories.AsQueryable();

            if (this.User.IsAuthenticated)
            {
                repositoriesQuery = repositoriesQuery.Where(r => r.IsPublic || r.OwnerId == this.User.Id);
            }
            else
            {
                repositoriesQuery = repositoriesQuery.Where(r => r.IsPublic);
            }

            var repositories = repositoriesQuery.OrderByDescending(r => r.CreatedOn)
                .Select(r => new AllRepositoriesViewModel
                { 
                    Id = r.Id,
                    Name = r.Name,
                    Owner = r.Owner.Username,
                    CreatedOn = r.CreatedOn.ToLocalTime().ToString("F"),
                    CommitsCount = r.Commits.Count,
                })
                .ToList();

            return this.View(repositories);
        }

    }
}
