using Git.Data;
using Git.Data.Models;
using Git.Models;
using MyWebServer.Controllers;
using MyWebServer.Http;
using System.Linq;

using static Git.Data.DataConstants;

namespace Git.Controllers
{
    public class CommitsController : Controller
    {
        private readonly GitDbContext dbContext;

        public CommitsController(GitDbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        [Authorize]
        public HttpResponse Create(string id)
        {
            var repository = this.dbContext.Repositories
                .Where(r => r.Id == id)
                .Select(r => new CommitToRepositoryViewModel
                { 
                    Id = r.Id,
                    Name = r.Name,
                })
                .FirstOrDefault();

            if (repository == null)
            {
                return this.BadRequest();
            }

            return this.View(repository);
        }

        [HttpPost]
        [Authorize]
        public HttpResponse Create(CreateCommitFormModel model)
        {
            if (!this.dbContext.Repositories.Any(r => r.Id == model.Id))
            {
                return this.NotFound();
            }

            if (model.Description.Length < DescriptionMinLength)
            {
                return this.Error($"Commit description have be at least {DescriptionMinLength} characters.");
            }

            var commit = new Commit
            {
                Description = model.Description,
                RepositoryId = model.Id,
                CreatorId = this.User.Id,
            };

            this.dbContext.Commits.Add(commit);
            this.dbContext.SaveChanges();

            return this.Redirect("/Repositories/All");
        }

        [Authorize]
        public HttpResponse All()
        {
            var commits = this.dbContext
                .Commits
                .Where(c => c.CreatorId == this.User.Id)
                .OrderByDescending(c => c.CreatedOn)
                .Select(c => new CommitListingViewModel
                {
                    Id = c.Id,
                    Description = c.Description,
                    CreatedOn = c.CreatedOn.ToLocalTime().ToString("F"),
                    Repository = c.Repository.Name,
                })
                    .ToList();

            return this.View(commits);
        }


        [Authorize]
        public HttpResponse Delete(string id)
        {
            //var commit = this.dbContext.Commits.Find(id);
            var commit = this.dbContext.Commits.Where(c => c.Id == id).FirstOrDefault();

            if (commit == null || commit.CreatorId != this.User.Id)
            {
                return this.BadRequest();
            }

            this.dbContext.Commits.Remove(commit);
            this.dbContext.SaveChanges();

            return this.Redirect("/Commits/All");
        }
    }
}
