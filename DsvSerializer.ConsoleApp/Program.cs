using System;
using DsvSerializer.ConsoleApp;

var person = new Person()
{
    id = 1,
    LastName="Doe",
    FirstName ="John"
};

var dsv = DsvSerializer.Core.DSVSerializer.Serialize(person, '|');
Console.WriteLine(dsv);