using Microsoft.AspNetCore.Hosting;
using System;
using Microsoft.AspNetCore.TestHost;
using Xunit;
using System.Net.Http;

namespace Ecovadis.DL.Tests
{
    public class TestFixture<TStartup> : IDisposable where TStartup : class
    {
        public readonly TestServer Server;
        private readonly HttpClient Client;

        public TestFixture()
        {
            var builder = new WebHostBuilder()
                .UseContentRoot($"..\\..\\..\\..\\Ecovadis.API\\")
                .UseStartup<TStartup>();

            Server = new TestServer(builder);
            Client = new HttpClient();
        }


        public void Dispose()
        {
            Client.Dispose();
            Server.Dispose();
        }
    }
}
