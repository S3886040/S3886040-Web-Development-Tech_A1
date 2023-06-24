
using A1_ClassLibrary.Managers;
using A1_ClassLibrary.BusinessModels;
using ConsoleApp.UtilityMethods;
using A1_ClassLibrary.Utilities;

namespace ConsoleApp.view;

internal class BankView
{
    private string _loggedInUser;
    readonly private CustomerManager _customerManager;

    public BankView(int loggedInUserID, CustomerManager customerManager)
    {

        _customerManager = customerManager;
        _loggedInUser = _customerManager.LoginUser(loggedInUserID);
        String menu = $"""
            --- {_loggedInUser} ---
            [1] Deposit
            [2] Withdraw
            [3] Transfer
            [4] My Statement
            [5] Logout
            [6] Exit
            """;
        bool runMenu = true;
        while (runMenu)
        {
            Console.WriteLine(menu);
            Console.WriteLine("Enter an Option:");
            var input = ConsoleMethods.GetUserInput();
            int option = IntConverter(input);
            if (option == 1 && option.IsInRange(1, 6))
            {
                int i = 1;
                List<Account> accounts;

                Console.WriteLine("---Deposit---");
                accounts = _customerManager.GetAccounts();
                accounts.ForEach(account => 
                Console.WriteLine(
                 $"""
                 {i++}.{"\t"}{DepositOrSavings(account.AccountType)}{"\t"}{account.AccountNumber}{"\t"}{account.Balance:C}
                 """
                 ));
                i = 1;
                Console.WriteLine("\nSelect an Account:");
                input = ConsoleMethods.GetUserInput();
                int inputInt = IntConverter(input);
                if (inputInt != -1)
                {
                    Console.WriteLine($"""{DepositOrSavings(accounts[inputInt -1].AccountType)}, Balance:{accounts[inputInt -1].Balance:C}, Available Balance:{accounts[inputInt -1].Balance:C} """);
                    Console.WriteLine("\nEnter Ammount:");
                    input = ConsoleMethods.GetUserInput();
                    decimal amount = IntConverter(input);
                    if (amount != -1)
                    {
                        decimal balance = _customerManager.Deposit(accounts[inputInt], amount);
                        Console.WriteLine($"""Deposit of {amount:C} was successful, balance is now {balance:C}""");
                    }

                }

            }
        }

    }
    // Returns either the string "Deposit" or "Savings"
    private string DepositOrSavings(string type)
    {
        string accType = "";
        if (type == "C")
        {
            accType = "Cheque";
        }
        else if (type == "S")
        {
            accType = "Savings";
        }
        else
        {
            accType = "Unknown";
        }
        return accType;
    }

    private int IntConverter(string value)
    {
        if (!int.TryParse(value, out var option))
        {
            Console.WriteLine("Invalid input.");
            option = -1;
        }
        return option;
    }
}

