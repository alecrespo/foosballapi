using AutoMapper;
using Ecovadis.API;
using Ecovadis.API.Controllers;
using Ecovadis.API.Services;
using Ecovadis.API.Tests;
using Ecovadis.DAL.Contexts;
using Ecovadis.DL.Infrastructures;
using Ecovadis.DL.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Moq;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace Mash.API.Tests.Controllers
{
    public class MatchControllerTests: IClassFixture<TestFixture<Startup>>
    {

        private MatchController controller { get; set; }
        private TestFixture<Startup> fixture { get; set; }
        public MatchControllerTests(TestFixture<Startup> fixture)
        {
            this.fixture = fixture;
        }
        private MatchController CreateController()
        {
            var imapper = (IMapper)fixture.Server.Host.Services.GetService(typeof(IMapper));
            var ierrorHandler = (IErrorHandler)fixture.Server.Host.Services.GetService(typeof(IErrorHandler));
            var configurationBuilder = new ConfigurationBuilder();
            var path = Path.Combine(Directory.GetCurrentDirectory(), "appsettings.json");
            configurationBuilder.AddJsonFile(path, false);

            var root = configurationBuilder.Build();
            var sqlConnection = root.GetConnectionString("DefaultConnection");

            var optionsBuilder = new DbContextOptionsBuilder<EcovadisContext>();
            optionsBuilder.UseSqlServer<EcovadisContext>(sqlConnection);

            var icontext = new EcovadisContext(optionsBuilder.Options);

            var logic = new LogicService();
            var error = new ErrorHandler();
            var game = new GameService(logic, imapper, error, icontext);
            return new MatchController(ierrorHandler, game);
        }
        [Fact]
        public void GetAll()
        {
            controller = CreateController();
            var result = (ObjectResult)controller.Get().Result;
            var val = result.Value;
            Assert.Equal(result.StatusCode,200);
        }

        [Fact]
        public void Create()
        {
            controller = CreateController();
            var result = (ObjectResult)controller.Create().Result;
            Assert.Equal(result.StatusCode, 200);
        }
        

    }
}
