namespace SharedTrip.Controllers
{
    using MyWebServer.Controllers;
    using MyWebServer.Http;
    using SharedTrip.Data;
    using SharedTrip.Models;
    using SharedTrip.Services;
    using SharedTrip.ViewModels.Trips;
    using System;
    using System.Globalization;
    using System.Linq;

    public class TripsController : Controller
    {
        private readonly IValidator validator;
        private readonly ApplicationDbContext dbContext;

        public TripsController(IValidator validator, ApplicationDbContext dbContext)
        {
            this.validator = validator;
            this.dbContext = dbContext;
        }

        [Authorize]
        public HttpResponse Add() => this.View();

        [Authorize]
        [HttpPost]
        public HttpResponse Add(AddTripFormModel model)
        {
            var modelErrors = this.validator.ValidateTrip(model);

            if (modelErrors.Any())
            {
                return this.Redirect("/Trips/Add");
            }

            var trip = new Trip
            {
                StartPoint = model.StartPoint,
                EndPoint = model.EndPoint,
                DepartureTime = DateTime.ParseExact(model.DepartureTime, "dd.MM.yyyy HH:mm", CultureInfo.InvariantCulture),
                Seats = model.Seats,
                Description = model.Description, 
                ImagePath = model.ImagePath,
            };

            this.dbContext.Trips.Add(trip);
            this.dbContext.SaveChanges();

            return this.Redirect("/Trips/All");
        }

        public HttpResponse All()
        {
            var trips = this.dbContext
                .Trips
                .Select(t => new TripsListingViewModel
                {
                    Id = t.Id,
                    StartPoint = t.StartPoint,
                    EndPoint = t.EndPoint,
                    DepartureTime = t.DepartureTime.ToString("dd.MM.yyyy HH:mm"),
                    AvailableSeats = t.Seats - t.UserTrips.Count,
                })
                .ToList();

            return this.View(trips);
        }

        [Authorize]
        public HttpResponse Details(string tripId)
        {
            var trip = this.dbContext.Trips.Where(t => t.Id == tripId)
                .Select(t => new TripDetailViewModel
                {
                    Id = t.Id,
                    StartPoint = t.StartPoint,
                    EndPoint = t.EndPoint,
                    DepartureTime = t.DepartureTime.ToLocalTime().ToString("dd.MM.yyyy HH:mm"),
                    AvailableSeats = t.Seats - t.UserTrips.Count,
                    Description = t.Description,
                    ImagePath = t.ImagePath,
                })
                .FirstOrDefault();

            return this.View(trip);
        }

        [Authorize]
        public HttpResponse AddUserToTrip(string tripId)
        {
            var userId = this.User.Id;

            if (this.dbContext.UsersTrips.Any(ut => ut.UserId == userId && ut.TripId == tripId))
            {
                return Redirect($"/Trips/Details?tripId={tripId}");
            }

            var userToTrip = new UserTrip
            { 
                UserId = userId,
                TripId = tripId,
            };

            this.dbContext.UsersTrips.Add(userToTrip);
            this.dbContext.SaveChanges();

            return this.Redirect("/Trips/All");
        }
    }
}
