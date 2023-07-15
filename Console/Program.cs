// See https://aka.ms/new-console-template for more information

using Console;

var testing = new ValidateArguments();
var argsValue=new List<string>(){"--count", "name"};
var result=testing.Validate(argsValue.ToArray());
System.Console.WriteLine(result);