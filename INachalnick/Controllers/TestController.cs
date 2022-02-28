using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using CSharpExtensions.OpenSource.Mongo;
using INachalnicUtilities.Mongo;
using INachalnick.Models;

namespace INachalnick.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class TestController : ControllerBase
    {
        [HttpGet("GetAllDocs")]
        public async Task<string> GetAllDocs()
        {
            var collection = await INachalnickMongo.GetCollectionAsync<TestItem>(INachalnickMongoDb.TestData, "test");
            var tests = await collection.GetAllAsync();
            return $"OK {tests}";
        }
    }
}