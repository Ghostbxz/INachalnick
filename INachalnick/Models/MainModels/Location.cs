using ProgramUtilities.Mongo;

namespace INachalnick.Models
{
    public class Location : MongoBaseDocument
    {

        public int Lot { get; set; }
        public int Parcel { get; set; }
        public string Field { get; set; }
        public Location(int lot, int parcel, string field)
        {
            Lot = lot;
            Parcel = parcel;
            Field = field;
        }
    }
}