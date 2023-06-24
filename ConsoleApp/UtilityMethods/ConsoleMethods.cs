
namespace ConsoleApp.UtilityMethods;

public class ConsoleMethods
{
    static public string GetUserInput()
    {
        string line = Console.ReadLine();
        line.Trim();
        return line;
    }

}
