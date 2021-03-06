using BattleCards.Data;
using BattleCards.Service;
using Microsoft.EntityFrameworkCore;
using MyWebServer;
using MyWebServer.Controllers;
using MyWebServer.Results.Views;
using System;
using System.Threading.Tasks;

namespace BattleCards
{
    public class Starup
    {
        public static async Task Main()
        => await HttpServer
             .WithRoutes(routes => routes
                    .MapStaticFiles()
                    .MapControllers())
                .WithServices(services => services
                    .Add<IViewEngine, CompilationViewEngine>()
                    .Add<IValidator, Validator>()
                .Add<IPasswordHasher, PasswordHasher>()
               .Add<BattleCardsDbContext>())
                .WithConfiguration<BattleCardsDbContext>(context => context
                    .Database.Migrate())
                .Start();
    }
}
