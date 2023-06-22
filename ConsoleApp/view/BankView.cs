
using A1_ClassLibrary.Managers;
using A1_ClassLibrary.BusinessModels;

namespace ConsoleApp1.view;

internal class BankView
{
    private Customer _loggedInUser;

    public BankView(Customer loggedInUser)
    {
        _loggedInUser = loggedInUser;
        Console.WriteLine("""
            --- {user} ---
            [1] Deposit
            [2] Withdraw
            [3] Transfer
            [4] My Statement
            [5] Logout
            [6] Exit
            """);

    }
}

