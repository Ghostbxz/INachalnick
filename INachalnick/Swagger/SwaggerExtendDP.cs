using CSharpExtensions.OpenSource;
using MongoDB.Bson.Serialization.Attributes;
using Newtonsoft.Json;
using NSwag.Generation.Processors;
using NSwag.Generation.Processors.Contexts;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Reflection;

public class SwaggerExtendDP : IDocumentProcessor
{
    public void Process(DocumentProcessorContext context)
    {
        var types = new List<Type>();
        AppDomain.CurrentDomain.GetAssemblies().ToList().ForEach(x => types.AddRange(x.DefinedTypes));
        types = types.Where(t => context.SchemaResolver.HasSchema(t, false)).ToList();
        foreach (var type in types)
        {
            object? instance = null;
            try
            {
                instance = Activator.CreateInstance(type);
            }
            catch { }
            var schema = context.SchemaResolver.GetSchema(type, false);
            var excludedProperties = new List<string>();
            var excludeAttributes = new Type[]
            {
                typeof(SwaggerExcludeAttribute),
                typeof(InversePropertyAttribute),
                typeof(ForeignKeyAttribute),
                typeof(JsonIgnoreAttribute),
                typeof(BsonIgnoreAttribute),
            };
            foreach (var att in excludeAttributes)
            {
                excludedProperties.AddRange(type.PowerfulGetProperties().Where(t => t.GetCustomAttribute(att) != null || t.PropertyType == typeof(MongoDB.Bson.ObjectId)).Select(x => x.Name.ToLower()));
                excludedProperties.AddRange(type.PowerfulGetFields().Where(t => t.GetCustomAttribute(att) != null || t.FieldType == typeof(MongoDB.Bson.ObjectId)).Select(x => x.Name.ToLower()));
            }
            excludedProperties = excludedProperties.Distinct().ToList();

            var requiredProperties = new List<string>();
            var requiredAttributes = new Type[]
            {
                typeof(RequiredAttribute),
                typeof(JsonRequiredAttribute),
                typeof(BsonRequiredAttribute),
            };
            foreach (var att in requiredAttributes)
            {
                requiredProperties.AddRange(type.PowerfulGetProperties().Where(t => t.GetCustomAttribute(att) != null).Select(x => x.Name.ToLower()));
                requiredProperties.AddRange(type.PowerfulGetFields().Where(t => t.GetCustomAttribute(att) != null).Select(x => x.Name.ToLower()));
            }
            requiredProperties = requiredProperties.Distinct().ToList();

            var uniqueItemsProperties = new List<string>();
            uniqueItemsProperties.AddRange(type.PowerfulGetProperties().Where(t => t.GetCustomAttribute(typeof(SwaggerUniqueItems)) != null).Select(t => t.Name.ToLower()));
            uniqueItemsProperties.AddRange(type.PowerfulGetFields().Where(t => t.GetCustomAttribute(typeof(SwaggerUniqueItems)) != null).Select(t => t.Name.ToLower()));

            var ignoreInheritProps = new List<string>();
            ignoreInheritProps.AddRange(type.PowerfulGetProperties().Where(t => t.GetCustomAttribute(typeof(SwaggerIgnoreInheritProps)) != null).Select(t => t.Name.ToLower()));
            ignoreInheritProps.AddRange(type.PowerfulGetFields().Where(t => t.GetCustomAttribute(typeof(SwaggerIgnoreInheritProps)) != null).Select(t => t.Name.ToLower()));

            var customDefaultValsProperties = new List<(string name, object? initVal, object? defaultVal)>();
            if (instance != null && !type.IsEnum)
            {
                customDefaultValsProperties.AddRange(type.PowerfulGetProperties().Select(t => (name: t.Name.ToLower(), initVal: t.GetValue(instance), defaultVal: GetDefault(t.PropertyType))).Where(t => t.defaultVal.ToJson() != t.initVal.ToJson()));
                customDefaultValsProperties.AddRange(type.PowerfulGetFields().Select(t => (name: t.Name.ToLower(), initVal: t.GetValue(instance), defaultVal: GetDefault(t.FieldType))).Where(t => t.defaultVal.ToJson() != t.initVal.ToJson()));
            }

            var schemasToProcess = new[] { schema }.Concat(schema.AllOf.GetEmptyIfNull()).RemoveNulls();
            foreach (var s in schemasToProcess)
            {
                s.Properties?.Keys.Where(x => uniqueItemsProperties.Any(y => y == x.ToLower())).ToList().ForEach(x => s.Properties[x].UniqueItems = true);
                s.Properties?.Keys.Where(x => excludedProperties.Any(y => y == x.ToLower())).ToList().ForEach(x => s.Properties.Remove(x));
                s.Properties?.Keys.Where(x => customDefaultValsProperties.Any(y => y.name == x.ToLower())).ToList().ForEach(x => s.Properties[x].Default = customDefaultValsProperties.First(y => y.name == x.ToLower()).initVal);
                s.Properties?.Keys.Where(x => ignoreInheritProps.Any(y => y == x.ToLower())).ToList().ForEach(x => (s.Properties[x].ExtensionData ??= new Dictionary<string, object>())["x-ignore-inherit"] = true);
                s.Properties?.Keys.Where(x => requiredProperties.Any(y => y == x.ToLower())).ToList().ForEach(x => { s.Properties[x].IsRequired = true; s.RequiredProperties.Add(x); });
            }
        }
    }
    public object? GetDefault(Type t) => GetType().GetMethod("GetDefaultGeneric", BindingFlags.Static | BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic)!.MakeGenericMethod(t).Invoke(this, null);
    private T? GetDefaultGeneric<T>() => default(T);
}

[AttributeUsage(AttributeTargets.Property | AttributeTargets.Field)]
public class SwaggerExcludeAttribute : Attribute
{
}

[AttributeUsage(AttributeTargets.Property | AttributeTargets.Field)]
public class SwaggerUniqueItems : Attribute
{
}

[AttributeUsage(AttributeTargets.Property | AttributeTargets.Field)]
public class SwaggerIgnoreInheritProps : Attribute
{
}
