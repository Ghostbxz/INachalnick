using MongoDB.Bson;
using MongoDB.Bson.Serialization;
using MongoDB.Bson.Serialization.Conventions;
using MongoDB.Bson.Serialization.Serializers;
using System;
using System.Linq;

namespace INachalnickUtilities.Helpers
{
    public static class MongoHelper
    {
        public static void SetGlobalSettings()
        {
            ConventionRegistry.Register("camelCase", new ConventionPack { new CamelCaseElementNameConvention() }, t => true);
            ConventionRegistry.Register("EnumStringConvention", new ConventionPack { new EnumRepresentationConvention(BsonType.String) }, t => true);
            ConventionRegistry.Register("IgnoreExtraElements", new ConventionPack { new IgnoreExtraElementsConvention(true) }, type => true);
            try
            {
                var enums = AppDomain.CurrentDomain.GetAssemblies().SelectMany(x => x.DefinedTypes).Where(x => x.IsEnum).ToList();
                foreach (var enumType in enums)
                {
                    try
                    {
                        var enumSerializer = Activator.CreateInstance(typeof(EnumSerializer<>).MakeGenericType(enumType), new object[] { BsonType.String }) as IBsonSerializer;
                        BsonSerializer.RegisterSerializer(enumType, enumSerializer);
                    }
                    catch { }
                }
            }
            catch (Exception ex)
            {
                //Logger.Debug($"MongoHelper, SetGlobalSettings, {ex}", ex);
            }
        }
    }
}