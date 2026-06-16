using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace DsvSerializer.Core;

public static class DSVSerializer
{
    private const char Delimiter = ';';

    public static string? Serialize(object obj, char? delimiter = null)
    {
        var separator = delimiter ?? Delimiter;
        
        var type = obj.GetType();

        if (type.IsArray || typeof(IEnumerable).IsAssignableFrom(type))
        {
            return null;
        }

        var members = type.GetMembers();
        
        var stringBuilder = new List<string>();
        foreach (var member in members)
        {
            switch (member)
            {
                case FieldInfo field:
                {
                    var attributes = field.GetCustomAttributes();
                    if (!attributes.Any(x => x.ToString().Contains("DSVIgnore")))
                    {
                        stringBuilder.Add(field.Name);
                    }
                }
                    
                    break;
                case PropertyInfo property:
                {
                    var attributes = property.GetCustomAttributes();
                    if (!attributes.Any(x => x.ToString().Contains("DSVIgnore")))
                    {
                        stringBuilder.Add(property.Name);
                    }
                }
                    break;
            }
        }
        var headerLine = string.Join(separator, stringBuilder);
        
        stringBuilder.Clear();
        foreach (var member in members)
        {
            switch (member)
            {
                case FieldInfo field:
                {
                    var attributes = field.GetCustomAttributes();
                    if (!attributes.Any(x => x.ToString().Contains("DSVIgnore")))
                    {
                        stringBuilder.Add(field.GetValue(obj)?.ToString() ?? "");
                    }

                    break;
                }
                case PropertyInfo property:
                {
                    var attributes = property.GetCustomAttributes();
                    if (!attributes.Any(x => x.ToString().Contains("DSVIgnore")))
                    {
                        stringBuilder.Add(property.GetValue(obj)?.ToString() ?? "");
                    }

                    break;
                }
            }
        }

        var valueLine = string.Join(separator, stringBuilder);
        
        return $"{headerLine}\n{valueLine}";
    }
}