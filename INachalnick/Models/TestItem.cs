using ProgramUtilities.Mongo;

namespace INachalnick.Models
{
    public class TestItem : MongoBaseDocument
    {
        public string Name { get; set; } = string.Empty;
    }
}