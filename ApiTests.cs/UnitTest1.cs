using NUnit.Framework;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;

namespace ApiTestingDemo
{
    [TestFixture]
    public class ApiTests
    {
        private HttpClient client;

        [OneTimeSetUp]
        public void Init()
        {
            client = new HttpClient();
        }

        [OneTimeTearDown]
        public void Cleanup()
        {
            client.Dispose(); 
        }

        [Test]
        public async Task SuccessfulLogin_ReturnsToken()
        {
            var url = "https://reqres.in/api/login";
            var data = new StringContent(
                "{\"email\":\"eve.holt@reqres.in\", \"password\":\"cityslicka\"}",
                Encoding.UTF8, "application/json");

            var response = await client.PostAsync(url, data);
            var content = await response.Content.ReadAsStringAsync();

            Assert.AreEqual(200, (int)response.StatusCode);
            Assert.IsTrue(JObject.Parse(content).ContainsKey("token"));
        }

        [Test]
        public async Task MissingPassword_ReturnsError()
        {
            var url = "https://reqres.in/api/login";
            var data = new StringContent(
                "{\"email\":\"eve.holt@reqres.in\"}",
                Encoding.UTF8, "application/json");

            var response = await client.PostAsync(url, data);
            var content = await response.Content.ReadAsStringAsync();

            Assert.AreEqual(400, (int)response.StatusCode);
            Assert.IsTrue(content.Contains("Missing password"));
        }
    }
}
