
using A1_ClassLibrary.Managers;
using ConsoleApp.UtilityMethods;

namespace ConsoleApp.view;
public class LoginView
{
    private readonly DBManager _dBManager;
    internal bool _running;
    public LoginView(DBManager dBManager, CustomerManager customerManager)
    {
        _dBManager = dBManager;
        _running = true;
        while (_running)
        {
            Console.WriteLine("Enter Login ID:");
            string login = ConsoleMethods.GetUserInput();

            Console.WriteLine("Enter Password:");
            string pass = ReadPassword();

            try
            {
                int loginInt = Int32.Parse(login);
                bool match = _dBManager.CheckLogin(loginInt, pass);
                if (match)
                {
                    var bankView = new BankView(loginInt, customerManager, this);
                }
            }
            catch (FormatException)
            {
                Console.WriteLine("Invalid Input, Try Again");
            }
        }


    }

    public void EndProgram()
    {
        _running = false;
    }

    private static string ReadPassword()
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

