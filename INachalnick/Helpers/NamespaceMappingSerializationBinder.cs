using Newtonsoft.Json.Serialization;
using System;
using System.Collections.Generic;
using System.Linq;

public class NamespaceMappingSerializationBinder : DefaultSerializationBinder
{
    public string? ForceNamespace { get; set; }
    public string? ForceAssemblyName { get; set; }

    public override Type BindToType(string? assemblyName, string typeName)
    {
        typeName = typeName.Split(".").Last();
        var types = new List<Type>();
        AppDomain.CurrentDomain.GetAssemblies().ToList().ForEach(x => types.AddRange(x.DefinedTypes));
        types = types.Where(x => x.Name == typeName).ToList();
        Type? type;

        if (!string.IsNullOrEmpty(ForceNamespace))
        {
            type = types.FirstOrDefault(t => t.Namespace == ForceNamespace);
            if (type != null) { return type; }
        }

        if (!string.IsNullOrEmpty(ForceAssemblyName))
        {
            type = types.FirstOrDefault(t => t.Assembly.GetName().Name == ForceAssemblyName);
            if (type != null) { return type; }
        }

        if (!string.IsNullOrEmpty(assemblyName))
        {
            type = types.FirstOrDefault(t => t.Assembly.GetName().Name == assemblyName);
            if (type != null) { return type; }
        }

        var defaultAssemblyName = System.Reflection.Assembly.GetExecutingAssembly()?.EntryPoint?.DeclaringType?.Assembly.GetName().Name ?? string.Empty;
        if (!string.IsNullOrEmpty(defaultAssemblyName))
        {
            type = types.FirstOrDefault(t => t.Assembly.GetName().Name == defaultAssemblyName);
            if (type != null) { return type; }
        }

        var defaultNamespace = System.Reflection.Assembly.GetExecutingAssembly()?.EntryPoint?.DeclaringType?.Namespace ?? string.Empty;
        if (!string.IsNullOrEmpty(defaultNamespace))
        {
            type = types.FirstOrDefault(t => t.Namespace == defaultNamespace);
            if (type != null) { return type; }
        }

        return types.First();
    }
}
