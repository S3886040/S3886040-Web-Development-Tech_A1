
namespace ConsoleApp.view;

public class LoginView
{
/*    private Controller Controller { get; set; } 
    public LoginView(Controller _controller) 
    {
        this.Controller = _controller;
        Console.WriteLine("Enter Login ID:");
        string login = getUserInput();

        Console.WriteLine("Enter Password:");
        string pass = ReadPassword();
    }*/

    static private string getUserInput()
    {
        string line = Console.ReadLine();
        line.Trim();
        return line;
    }

    static private string ReadPassword()
    {
        string password = "";
        ConsoleKeyInfo key;

        do
        {
            key = Console.ReadKey(true);

            if (key.Key != ConsoleKey.Backspace && key.Key != ConsoleKey.Enter)
            {
                password += key.KeyChar;
                Console.Write("*");
            }
            else
            {
                if (key.Key == ConsoleKey.Backspace && password.Length > 0)
                {
                    password = password.Remove(password.Length - 1);
                    Console.Write("\b \b");
                }
            }
        }
        while (key.Key != ConsoleKey.Enter);

        Console.WriteLine();

        return password;
    }
}

