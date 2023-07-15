using System.ComponentModel.Design;
using System.Diagnostics.Metrics;

namespace Console;
public static class ArgumentConstants
{
    public const string ArgumentPrefix = "--";
    public  const string HelpArgument = "help";
    public  const int HelpRequested =1;
    public  const int ValidRequest =0;
    public  const int InValidRequest =-1;

    public static bool IsHelpCommand(this string source)
    {
        if (string.IsNullOrWhiteSpace(source))
        {
            return false;
        }
        return source.Equals($"{ArgumentPrefix}{HelpArgument}");
    }
    
    public static bool IsCommand(this string source)
    {
        if (string.IsNullOrWhiteSpace(source)|| !source.StartsWith(ArgumentPrefix))
        {
            return false;
        }

        var commandName = source.Replace(ArgumentPrefix, "");
        return commandName.Length > 0;//TODO check if the name actually applies here as well
    }
    
    public static bool IsStringCommandValue(this string source)
    {
        if (string.IsNullOrWhiteSpace(source))
        {
            return false;
        }

        return source.Length >= 3 && source.Length <= 10;
    }
    
    public static bool IsNumberCommandValue(this string source,string command)
    {
        int sourceValue = 0;
        if (!string.IsNullOrWhiteSpace(source) && int.TryParse(source,out sourceValue))
        {
            return sourceValue >= 10 && sourceValue <= 100;
        }


        return false;
    }
    public static bool IsValidCommandValue(this string source,string command)
    {
        if ( command.Equals($"{ArgumentPrefix}count"))
        {
            return source.IsNumberCommandValue(command);
        }
        return  source.IsStringCommandValue();
    }
}


    public class ValidateArguments
    {
        
        public int Validate(string[] args)
        {
            if (args.Length==0)
            {
                return ArgumentConstants.InValidRequest;
            }
            //this is for simplicity 
            var toLowerArguments = args.Select(a => a.ToLower()).ToList();
            if (args.Length==1)
            {
                if (toLowerArguments[0].IsHelpCommand())
                {
                    return ArgumentConstants.HelpRequested;
                }

                return ArgumentConstants.InValidRequest;
            }

            int i = 0;
            if (toLowerArguments[0].IsHelpCommand()&& toLowerArguments.Count>1)
            {
                i = 1;
            }

            while(i < toLowerArguments.Count)
            {
                if (args[i].IsCommand())
                {

                    if (i+1<toLowerArguments.Count)
                    {
                        if (!args[i+1].IsValidCommandValue(args[i]))
                        {
                            return ArgumentConstants.InValidRequest;
                        }
                        
                        i = i + 2;
                    }
                    else
                    {
                        return ArgumentConstants.InValidRequest;
                    }
                }
                else
                {
                    return ArgumentConstants.InValidRequest;
                }
            }

            return toLowerArguments[0].IsHelpCommand()?ArgumentConstants.HelpRequested:  ArgumentConstants.ValidRequest;
        }
        
}