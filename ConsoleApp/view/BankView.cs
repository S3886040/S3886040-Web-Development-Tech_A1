
using A1_ClassLibrary.Managers;
using A1_ClassLibrary.BusinessModels;
using ConsoleApp.UtilityMethods;
using A1_ClassLibrary.Utilities;
using Azure.Core;

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
            // Prints menu and takes in selection
            Console.WriteLine(menu);
            int option = GetSelection("Enter an option: ");
            // Deposit Option
            if (option == 1 && option.IsInRange(1, 6))
            {
                Console.WriteLine("---Deposit---");
                List<Account> accounts = GetAndPrintAccounts();

                int inputInt = GetSelection("Select an Account: ");
                if (inputInt != -1)
                {
                    Console.WriteLine($"""{DepositOrSavings(accounts[inputInt -1].AccountType)}, Balance:{accounts[inputInt -1].Balance:C}, Available Balance:{accounts[inputInt -1].Balance:C} """);
                    decimal amount = GetSelection("Enter Amount: ");
                    if (amount != -1)
                    {
                        Console.WriteLine("Enter Comment (max length 30): ");
                        string comment = ConsoleMethods.GetUserInput();
                        decimal balance = _customerManager.Deposit(accounts[inputInt - 1], amount, comment);
                        Console.WriteLine($"""Deposit of {amount:C} was successful, balance is now {balance:C}""");
                    }

                }

            }

            //Withdraw Option
            if(option == 2)
            {
                Console.WriteLine("---Withdraw---");
                List<Account> accounts = GetAndPrintAccounts();

                int selection = GetSelection("Select an Account: ");
                if (selection != -1)
                {
                    Console.WriteLine($"""{DepositOrSavings(accounts[selection - 1].AccountType)}, Balance:{accounts[selection - 1].Balance:C}, Available Balance:{accounts[selection - 1].Balance:C} """);
                    decimal amount = GetSelection("Enter Amount: ");
                    if (amount != -1)
                    {
                        Console.WriteLine("Enter Comment (max length 30): ");
                        string comment = ConsoleMethods.GetUserInput();
                        decimal balance = _customerManager.Withdraw(accounts[selection - 1], amount, comment);
                        Console.WriteLine($"""Withdrawal of {amount:C} was successful, balance is now {balance:C}""");
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

    private List<Account> GetAndPrintAccounts()
    {
        int i = 1;
        List<Account> accounts;

        accounts = _customerManager.GetAccounts();
        accounts.ForEach(account =>
        Console.WriteLine(
         $"""
         {i++}.{"\t"}{DepositOrSavings(account.AccountType)}{"\t"}{account.AccountNumber}{"\t"}{account.Balance:C}
         """
         ));

        return accounts;
    }

    private int GetSelection(string request)
    {
        Console.WriteLine(request);
        string input = ConsoleMethods.GetUserInput();
        int inputInt = IntConverter(input);
        return inputInt;
    }
}

