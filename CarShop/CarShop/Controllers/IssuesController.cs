namespace CarShop.Controllers
{
    using CarShop.Data;
    using CarShop.Data.Models;
    using CarShop.Models.Issues;
    using CarShop.Services;
    using MyWebServer.Controllers;
    using MyWebServer.Http;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    public class IssuesController : Controller
    {
        private readonly IUserService userService;
        private readonly CarShopDbContext dbContext;
        private readonly IValidator validator;

        public IssuesController(IUserService userService, CarShopDbContext dbContext, IValidator validator)
        {
            this.userService = userService;
            this.dbContext = dbContext;
            this.validator = validator;
        }

        [Authorize]
        public HttpResponse CarIssues(string carId)
        {
            if (!this.userService.IsMechanic(this.User.Id))
            {
                var userOwnCar = this.dbContext.Cas.Any(c => c.Id == carId && c.OwnerId == this.User.Id);

                if (!userOwnCar)
                {
                    return Error("You do not have access to this car.");
                }
            }

            var carWithIssues = this.dbContext.Cas.Where(c => c.Id == carId).Select(c => new CarIssuesViewModel
            {
                Id = c.Id,
                Model = c.Model,
                Year = c.Year,
                Issues = c.Issues.Select(i => new IssueListingViewModel
                {
                    Id = i.Id,
                    Description = i.Description,
                    IsFixed = i.IsFixed
                })
            })
                .FirstOrDefault();

            if (carWithIssues == null)
            {
                return Error($"Car with '{carId}' does not exist.");
            }

            return this.View(carWithIssues);
        }


        [Authorize]
        public HttpResponse Add()
        {
            return this.View();
        }


        [Authorize]
        [HttpPost]
        public HttpResponse Add(string carId, string description)
        {
            var modelErrors = this.validator.ValidateIssue(description);

            if (modelErrors.Any())
            {
                return Error(modelErrors);
            }

            var issue = new Issue
            { 
                CarId = carId,
                Description = description,
            };

            this.dbContext.Issues.Add(issue);
            this.dbContext.SaveChanges();

            return this.Redirect($"/Issues/CarIssues?carId={carId}");
        }

        [Authorize]
        public HttpResponse Fix(string issueId)
        {
            if (!this.userService.IsMechanic(this.User.Id))
            {
                return this.Error("You are not mechanic!");
            }

            var issue = this.dbContext.Issues.FirstOrDefault(i => i.Id == issueId);
            issue.IsFixed = true;

            this.dbContext.SaveChanges();

            return this.Redirect("/Cars/All");
        }

        public HttpResponse Delete(string issueId, string carId)
        {
            if (!this.userService.IsMechanic(this.User.Id))
            {
                return this.Error("You are not mechanic!");
            }

            var issue = this.dbContext.Issues.FirstOrDefault(i => i.Id == issueId && i.CarId == carId);

            this.dbContext.Issues.Remove(issue);
            this.dbContext.SaveChanges();

            return this.Redirect("/Cars/All");
        }
    }
}
