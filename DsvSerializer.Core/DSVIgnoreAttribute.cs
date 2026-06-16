using System;

namespace DsvSerializer.Core;

public class DSVIgnoreAttribute : Attribute
{
    public bool Ignore { get; set; } = false;
}