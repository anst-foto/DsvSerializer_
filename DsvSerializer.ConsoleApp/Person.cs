using DsvSerializer.Core;

namespace DsvSerializer.ConsoleApp;

public class Person
{
    public int id;
    
    public string LastName { get; set;}
    public string FirstName { get; set;}
    
    [DSVIgnore]
    public string FullName => $"{LastName} {FirstName}";
}