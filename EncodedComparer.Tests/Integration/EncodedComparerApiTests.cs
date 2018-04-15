using EncodedComparer.Domain.Entities;
using EncodedComparer.Infra.DataContexts;
using EncodedComparer.Infra.Repositories;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.Configuration;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace EncodedComparer.Tests.Integration.Entities
{
    [TestClass]
    public class EncodedComparerApiTests
    {
        private readonly TestServer _server;
        private readonly HttpClient _client;
        private const int TwoDifferentDataId = 1000;
        private const int EmptyId = 1001;

        [TestInitialize]
        public async Task Initialize()
        {
            await PrepareData();
        }

        public EncodedComparerApiTests()
        {
            _server = new TestServer(
                   new WebHostBuilder()
                   .UseConfiguration(new ConfigurationBuilder()
                       .AddJsonFile("appsettings.json")
                       .Build())
                   .UseStartup<Startup>());
            _client = _server.CreateClient();
        }

        private static async Task PrepareData()
        {
            using (var context = new EncodedComparerContext(Startup.ConnectionString))
            {
                var originalData = new Base64Data(TwoDifferentDataId, "ew0KIm5hbWUiOiJNYXJ5IiwNCiJhZ2UiOjMwLA0KImNhcnMiOlsgIkZvcmQiLCAiQk1XIiwgIk5pYXQiIF0NCn0=");
                var twoChangesData = new Base64Data(TwoDifferentDataId, "ew0KIm5hbWUiOiJKb2huIiwNCiJhZ2UiOjMwLA0KImNhcnMiOlsgIkZvcmQiLCAiQk1XIiwgIkZpYXQiIF0NCn0=");

                var encodedPairRepository = new EncodedPairRepository(context);
                await encodedPairRepository.DeleteById(TwoDifferentDataId);
                await encodedPairRepository.DeleteById(EmptyId);
                await encodedPairRepository.CreateLeft(originalData);
                await encodedPairRepository.CreateRight(twoChangesData);
            }
        }

        [TestMethod]
        public async Task ShouldGetDifferencesFromExistingId()
        {
            var jsonResult = await _client.GetAsync($"/v1/diff/{TwoDifferentDataId}");
            jsonResult.EnsureSuccessStatusCode();
            var responseString = await jsonResult.Content.ReadAsStringAsync();
            string expectedResult = "{\"success\":true,\"message\":\"Left and Right are same size but have differences. See the differences list.\",\"data\":[{\"startingIndex\":15,\"length\":5},{\"startingIndex\":74,\"length\":1}],\"notifications\":[]}";

            Assert.IsTrue(jsonResult.IsSuccessStatusCode);
            Assert.AreEqual(expectedResult, responseString);
        }

        [TestMethod]
        public async Task ShouldAddLeftDataOAnAvaliableId()
        {
            string jsonInString = "{\"base64EncodedData\": \"ew0KIm5hbWUiOiJKb2huIiwNCiJhZ2UiOjMwLA0KImNhcnMiOlsgIkZvcmQiLCAiQk1XIiwgIkZpYXQiIF0NCn0=\"}";
            var jsonResult = await _client.PostAsync($"/v1/diff/{EmptyId}/left", new StringContent(jsonInString, Encoding.UTF8, "application/json"));
            jsonResult.EnsureSuccessStatusCode();
            var responseString = await jsonResult.Content.ReadAsStringAsync();

            string expectedResult = "{\"success\":true,\"message\":\"Left data was successfully added.\",\"data\":null,\"notifications\":[]}";
            Assert.IsTrue(jsonResult.IsSuccessStatusCode);
            Assert.AreEqual(expectedResult, responseString);
        }

        [TestMethod]
        public async Task ShouldAddRightDataOAnAvaliableId()
        {
            string jsonInString = "{\"base64EncodedData\": \"ew0KIm5hbWUiOiJKb2huIiwNCiJhZ2UiOjMwLA0KImNhcnMiOlsgIkZvcmQiLCAiQk1XIiwgIkZpYXQiIF0NCn0=\"}";
            var jsonResult = await _client.PostAsync($"/v1/diff/{EmptyId}/right", new StringContent(jsonInString, Encoding.UTF8, "application/json"));
            jsonResult.EnsureSuccessStatusCode();
            var responseString = await jsonResult.Content.ReadAsStringAsync();

            string expectedResult = "{\"success\":true,\"message\":\"Right data was successfully added.\",\"data\":null,\"notifications\":[]}";
            Assert.IsTrue(jsonResult.IsSuccessStatusCode);
            Assert.AreEqual(expectedResult, responseString);
        }
    }
}
