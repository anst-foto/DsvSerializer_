using System;
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
        members.ToList().ForEach(m => BuilderTitle(stringBuilder, m));
        /*foreach (var member in members)
        {
            BuilderTitle(stringBuilder, member);
        }*/
        var headerLine = string.Join(separator, stringBuilder);
        
        stringBuilder.Clear();
        members.ToList().ForEach(m => Builder(stringBuilder, m ,obj));
        /*foreach (var member in members)
        {
            Builder(stringBuilder, member, obj);
        }*/

        var valueLine = string.Join(separator, stringBuilder);
        
        return $"{headerLine}\n{valueLine}";
    }

    private static void BuilderTitle(List<string> stringBuilder, MemberInfo member)
    {
        var attributes = member.GetCustomAttributes();
        if (attributes.Any(x => x.ToString().Contains("DSVIgnore"))) return;
        if (member is PropertyInfo or FieldInfo)
        {
            stringBuilder.Add(member.Name);
        }
        
    }
    private static void Builder(List<string> stringBuilder, MemberInfo member, object obj)
    {
        var attributes = member.GetCustomAttributes();
        if (attributes.Any(x => x.ToString().Contains("DSVIgnore"))) return;
        
        switch (member)
        {
            case PropertyInfo property:
                stringBuilder.Add(property.GetValue(obj)?.ToString() ?? "");
                return;
            case FieldInfo field:
                stringBuilder.Add(field.GetValue(obj)?.ToString() ?? "");
                return;
        }
    }
    
}