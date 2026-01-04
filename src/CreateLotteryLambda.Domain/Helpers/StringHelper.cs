using System.Text.RegularExpressions;

namespace CreateLotteryLambda.Domain.Helpers;

public class StringHelper
{
    private static readonly Regex PascalCaseRegex = new Regex("(?<!^)([A-Z])", RegexOptions.Compiled);

    public static string PascalToSnakeCase(string pascalCase)
    {
        string spacedString = PascalCaseRegex.Replace(pascalCase, " $1");
        string snakeCase = spacedString.Replace(" ", "_").ToLower();
        return snakeCase;
    }
}
