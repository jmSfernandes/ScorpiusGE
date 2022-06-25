namespace Scorpius;

public static  class Utils
{
    
    
    public static string GetIsoTime()
    {
        return $"{DateTime.UtcNow:yyyy-MM-dd'T'HH:mm:ss'Z'}";
    }

    public static bool EvaluateExpression(string expression)
    {
        string[] strings;

        switch (expression.Trim().ToLower())
        {
            case "true":
                return true;
            case "false":
                return false;
        }

        if (expression.Contains("=="))
        {
            strings = expression.Split("==");
            if (strings.Length != 2)
                return false;
            return strings[0] == strings[1];
        }

        if (expression.Contains("!="))
        {
            strings = expression.Split("!=");
            if (strings.Length != 2)
                return false;
            
            return strings[0] != strings[1];
        }

        
        return false;
    }
}