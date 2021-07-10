using CarShop.Data;
using CarShop.Data.Models;
using CarShop.Models.Cars;
using CarShop.Services;
using MyWebServer.Controllers;
using MyWebServer.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CarShop.Controllers
{
    public class CarsController : Controller
    {
        private readonly IUserService userService;
        private readonly IValidator validator;
        private readonly CarShopDbContext dbContext;

        public CarsController(IUserService userService, IValidator validator, CarShopDbContext dbContext)
        {
            this.userService = userService;
            this.validator = validator;
            this.dbContext = dbContext;
        }

        [Authorize]
        public HttpResponse Add()
        {
            if (this.userService.IsMechanic(this.User.Id))
            {
                return this.Unauthorized();
            }

            return this.View();
        }

        [Authorize]
        [HttpPost]
        public HttpResponse Add(AddCarFormModel model)
        {
            if (this.userService.IsMechanic(this.User.Id))
            {
                return this.Unauthorized();
            }

            var modelErros = this.validator.ValidateCar(model);

            if (modelErros.Any())
            {
                return this.Error(modelErros);
            }

            var car = new Car
            { 
                Model = model.Model,
                Year = model.Year,
                PictureUrl = model.Image,
                PlateNumber = model.PlateNumber,
                OwnerId = this.User.Id,
            };

            this.dbContext.Cas.Add(car);
            this.dbContext.SaveChanges();

            return this.Redirect("/Cars/All");
        }

        [Authorize]
        public HttpResponse All()
        {
            var carsQuery = this.dbContext.Cas.AsQueryable();

            if (this.userService.IsMechanic(this.User.Id))
            {
                carsQuery = carsQuery.Where(c => c.Issues.Any(c => !c.IsFixed));
            }
            else
            {
                carsQuery = carsQuery.Where(c => c.OwnerId == this.User.Id);
            }

            var cars = carsQuery.Select(c => new CarListeningViewModel
            {
                Id = c.Id,
                Model = c.Model,
                Year = c.Year,
                Image= c.PictureUrl,
                PlateNumber = c.PlateNumber,
                FixedIssues = c.Issues.Where(i => i.IsFixed).Count(),
                RemainingIssues = c.Issues.Where(i => !i.IsFixed).Count()
            }).ToList();

            return this.View(cars);
        }


    }
}
