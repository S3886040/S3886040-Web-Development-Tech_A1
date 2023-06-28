
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
                    Console.WriteLine($"""{DepositOrSavings(accounts[inputInt -1].AccountType)}, Balance:{accounts[inputInt -1].Balance:C}, Available Balance:{accounts[inputInt -1].GetAvailableBalance():C} """);
                    decimal amount = GetSelection("Enter Amount: ");
                    if (amount != -1)
                    {
                        Console.WriteLine("Enter Comment (max length 30): ");
                        string comment = ConsoleMethods.GetUserInput();
                        Task<decimal> balance = _customerManager.Deposit(accounts[inputInt - 1], amount, comment);
                        if(balance.Result != -1)
                        {
                            Console.WriteLine($"""Deposit of {amount:C} was successful, balance is now {balance.Result:C}""");
                        } else
                        {
                            Console.WriteLine("Invalid Input");
                        }
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
                    Console.WriteLine($"""{DepositOrSavings(accounts[selection - 1].AccountType)}, Balance:{accounts[selection - 1].Balance:C}, Available Balance:{accounts[selection - 1].GetAvailableBalance():C} """);
                    decimal amount = GetSelection("Enter Amount: ");
                    if (amount != -1)
                    {
                        Console.WriteLine("Enter Comment (max length 30): ");
                        string comment = ConsoleMethods.GetUserInput();
                        Task<decimal> balance = _customerManager.Withdraw(accounts[selection - 1], amount, comment);
                        if(balance.Result != -1)
                        {
                            Console.WriteLine($"""Withdrawal of {amount:C} was successful, balance is now {balance.Result:C}""");
                        } else
                        {
                            Console.WriteLine("Invalid Input.");
                        }
                        
                    }
                }
            } 
            //Transfer Option
            if(option == 3)
            {
                Console.WriteLine("---Transfer---");
                List<Account> accounts = GetAndPrintAccounts();
                int selection = GetSelection("Select an Account: ");

                if(selection != -1)
                {
                    int destAccNum = GetSelection("Enter Dest. Account Number: ");
                    if (_customerManager.CheckAccountExists(destAccNum) && destAccNum != accounts[selection - 1].AccountNumber)
                    {
                        Console.WriteLine($"""{DepositOrSavings(accounts[selection - 1].AccountType)}, Balance:{accounts[selection - 1].Balance:C}, Available Balance:{accounts[selection - 1].GetAvailableBalance():C} """);
                        decimal amount = GetSelection("Enter Amount: ");
                        if (amount != -1) 
                        {
                            Console.WriteLine("Enter Comment (max length 30): ");
                            string comment = ConsoleMethods.GetUserInput();
                            Task<bool> done = _customerManager.Transfer(accounts[selection - 1], destAccNum, amount, comment);
                            if (done.Result)
                            {
                                Console.WriteLine($"""Transfer of {amount:C} was successful.""");
                            }
                            else
                            {
                                Console.WriteLine("There was a problem with the transfer.");
                            }
                        }
                    } else { Console.WriteLine("Account does not exist."); }

                }
            }
        }

    }
    // Returns either the string "Deposit" or "Savings"
    private static string DepositOrSavings(string type)
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

    private static int IntConverter(string value)
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

    private static int GetSelection(string request)
    {
        Console.WriteLine(request);
        string input = ConsoleMethods.GetUserInput();
        int inputInt = IntConverter(input);
        return inputInt;
    }
}

