
using AutoMapper;
using Ecovadis.API;
using Ecovadis.API.Services;
using Ecovadis.DAL.Contexts;
using Ecovadis.DAL.Models;
using Ecovadis.DL.Infrastructures;
using Ecovadis.DL.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace Ecovadis.DL.Tests.Services
{
    public class LogicServiceTest : IClassFixture<TestFixture<Startup>>
    {
        private readonly EcovadisContext icontext;
        private readonly IMapper mapper;
        private readonly IGameService game;
        public LogicServiceTest(TestFixture<Startup> fixture)
        {
            mapper = (IMapper)fixture.Server.Host.Services.GetService(typeof(IMapper));
            var configurationBuilder = new ConfigurationBuilder();
            var path = Path.Combine(Directory.GetCurrentDirectory(), "appsettings.json");
            configurationBuilder.AddJsonFile(path, false);

            var root = configurationBuilder.Build();
            var sqlConnection = root.GetConnectionString("DefaultConnection");

            var optionsBuilder = new DbContextOptionsBuilder<EcovadisContext>();
            optionsBuilder.UseSqlServer<EcovadisContext>(sqlConnection);

            icontext = new EcovadisContext(optionsBuilder.Options);
            var logic = new LogicService();
            var error = new ErrorHandler();
            game = new GameService(logic, mapper, error, icontext);
        }

        [Fact]
        public void CreateTest()
        {
            var match = game.Create();
            Assert.NotNull(match);
            var detail = game.Detail(match.Id);
            Assert.Equal(1, detail.Periods.Count);
            Assert.Equal(0, detail.Periods.First().Goals.Count);
        }

        [Fact]
        public void GetAllTest()
        {
            var matches = game.List();
            Assert.NotNull(matches);
        }

        [Theory]
        [InlineData(1,15,TableSide.Blue)]
        [InlineData(1, 10,TableSide.Red)]
        public void UpdateTest(int matchId,int repetition, TableSide side)
        {
            for (int i = 0; i < repetition; i++)
            {
               game.Score(matchId, side);
            }
            var match = game.Detail(matchId);
           Assert.Equal((repetition/10)+1, match.Periods.Count);
        }

        [Theory]
        [InlineData(21, TableSide.Blue)]
        public void OneSidedTest(int repetition, TableSide side)
        {
            var match = game.Create();
            for (int i = 0; i < repetition; i++)
            {
               game.Score(match.Id, side);
            }
            var matchResult = game.Detail(match.Id);
            Assert.True(matchResult.IsFinished);
            Assert.True(matchResult.WasOneSided);
        }

        [Fact]
        public void BestOfThreeTest()
        {
            var match = game.Create();
            Assert.NotNull(match);
            for (int i = 0; i < 30; i++)
            {
                if (i > 9 && i < 20)
                {
                   game.Score(match.Id, TableSide.Red);
                }
                else
                {
                    game.Score(match.Id, TableSide.Blue);
                }
            }
            var matchResult = game.Detail(match.Id);
            Assert.Equal(TableSide.Blue, matchResult.Winner);
        }
    }
}
