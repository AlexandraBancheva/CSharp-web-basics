using BattleCards.Data;
using BattleCards.Data.Models;
using BattleCards.Models.Cards;
using BattleCards.Service;
using MyWebServer.Controllers;
using MyWebServer.Http;
using System.Linq;

namespace BattleCards.Controllers
{
    public class CardsController : Controller
    {
        private readonly IValidator validator;
        private readonly BattleCardsDbContext dbContext;

        public CardsController(IValidator validator, BattleCardsDbContext dbContext)
        {
            this.validator = validator;
            this.dbContext = dbContext;
        }

        [Authorize]
        public HttpResponse Add() => this.View();

        [Authorize]
        [HttpPost]
        public HttpResponse Add(AddCardFormModel model)
        {
            var modelErrors = this.validator.ValidateCard(model);

            if (modelErrors.Any())
            {
                return this.Error(modelErrors);
            }

            var card = new Card
            {
                Name = model.Name,
                ImageUrl = model.Image,
                Keyword = model.Keyword,
                Attack = model.Attack,
                Health = model.Health,
                Description = model.Description,
            };

            this.dbContext.Cards.Add(card);
            this.dbContext.SaveChanges();

            var user = this.User.Id;

            this.dbContext.Add(new UserCard
            {
                CardId =card.Id,
                UserId = user,
            });

            this.dbContext.SaveChanges();

            return this.Redirect("/Cards/All");
        }

        [Authorize]
        public HttpResponse All()
        {
            var cards = this.dbContext.Cards
                .Select(c => new AllCardsListingViewModel
                    {
                        Id = c.Id,
                        Name = c.Name,
                        ImageUrl = c.ImageUrl,
                        Attack = c.Attack,
                        Health = c.Health,
                        Description = c.Description,
                        Keyword = c.Keyword,
                    })
                .ToList();


            return this.View(cards);
        }

        [Authorize]
        public HttpResponse Collection()
        {
            var collection = this.dbContext.UsersCards
                .Where(uc => uc.UserId == this.User.Id)
                .Select(c => c.Card)
                .Select(c => new AllCardsListingViewModel
                {
                    Id = c.Id,
                    Name = c.Name,
                    Keyword = c.Keyword,
                    Description = c.Description,
                    Attack = c.Attack,
                    Health = c.Health,
                    ImageUrl = c.ImageUrl,
                })
                .ToList();

            return this.View(collection);
        }

        [Authorize]
        public HttpResponse AddToCollection(int cardId)
        {
            if (this.dbContext.UsersCards.Any(us => us.CardId == cardId && us.UserId == this.User.Id))
            {
                return this.Redirect("/Cards/All");
            }

            this.dbContext.Add(new UserCard
            {
                UserId = this.User.Id,
                CardId = cardId
            });

            this.dbContext.SaveChanges();

            return this.Redirect("/Cards/All");
        }

        [Authorize]
        public HttpResponse RemoveFromCollection(int cardId)
        {
            var userCard = this.dbContext.UsersCards.First(uc => uc.CardId == cardId && uc.UserId == this.User.Id);

            this.dbContext.UsersCards.Remove(userCard);
            this.dbContext.SaveChanges();

            return this.Redirect("/Cards/Collection");
        }
    }
}
