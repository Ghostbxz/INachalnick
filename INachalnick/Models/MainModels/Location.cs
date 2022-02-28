using ProgramUtilities.Mongo;

namespace INachalnick.Models
{
    public class Location : MongoBaseDocument
    {
        public int Lot { get; set; }
        public int Parcel { get; set; }
        public string Field { get; set; }
    }
}