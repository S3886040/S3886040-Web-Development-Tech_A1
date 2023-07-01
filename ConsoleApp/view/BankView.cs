
using A1_ClassLibrary.Managers;
using A1_ClassLibrary.BusinessModels;
using ConsoleApp.UtilityMethods;
using A1_ClassLibrary.Utilities;

namespace ConsoleApp.view;

internal class BankView
{
    private string _loggedInUser;
    readonly private CustomerManager _customerManager;

    public BankView(int loggedInUserID, CustomerManager customerManager, LoginView lView)
    {

        _customerManager = customerManager;
        _loggedInUser = _customerManager.LoginUser(loggedInUserID);
        string menu = $"""
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

                int selection = GetSelection("Select an Account: ");
                if ((selection != -1) && selection.IsInRange(1, accounts.Count))
                {
                    Console.WriteLine($"""{DepositOrSavings(accounts[selection - 1].AccountType)}, Balance:{accounts[selection - 1].Balance:C}, Available Balance:{accounts[selection - 1].GetAvailableBalance():C} """);
                    decimal amount = GetAmount("Enter Amount: ");
                    if (amount != -1)
                    {
                        Console.WriteLine("Enter Comment (max length 30): ");
                        string comment = ConsoleMethods.GetUserInput();
                        Task<decimal> balance = _customerManager.Deposit(accounts[selection - 1], amount, comment);
                        if (balance.Result != -1)
                        {
                            Console.WriteLine($"""Deposit of {amount:C} was successful, balance is now {balance.Result:C}""");
                        }
                        else
                        {
                            Console.WriteLine("Invalid Input");
                        }
                    }
                }
                else { Console.WriteLine("Account Doesn't exist"); }

            }

            //Withdraw Option
            if (option == 2)
            {
                Console.WriteLine("---Withdraw---");
                List<Account> accounts = GetAndPrintAccounts();

                int selection = GetSelection("Select an Account: ");
                if ((selection != -1) && selection.IsInRange(1, accounts.Count))
                {
                    Console.WriteLine($"""{DepositOrSavings(accounts[selection - 1].AccountType)}, Balance:{accounts[selection - 1].Balance:C}, Available Balance:{accounts[selection - 1].GetAvailableBalance():C} """);
                    decimal amount = GetAmount("Enter Amount: ");
                    if (amount != -1)
                    {
                        Console.WriteLine("Enter Comment (max length 30): ");
                        string comment = ConsoleMethods.GetUserInput();
                        Task<decimal> balance = _customerManager.Withdraw(accounts[selection - 1], amount, comment);
                        if (balance.Result != -1)
                        {
                            Console.WriteLine($"""Withdrawal of {amount:C} was successful, balance is now {balance.Result:C}""");
                        }
                        else
                        {
                            Console.WriteLine("Insufficient Funds.");
                        }

                    }
                }
                else { Console.WriteLine("Account Doesn't exist"); }
            }
            //Transfer Option
            if (option == 3)
            {
                Console.WriteLine("---Transfer---");
                List<Account> accounts = GetAndPrintAccounts();
                int selection = GetSelection("Select an Account: ");

                if (selection != -1 && selection.IsInRange(1, accounts.Count))
                {
                    int destAccNum = GetSelection("Enter Dest. Account Number: ");
                    if (destAccNum != -1 && _customerManager.CheckAccountExists(destAccNum) && destAccNum != accounts[selection - 1].AccountNumber)
                    {
                        Console.WriteLine($"""{DepositOrSavings(accounts[selection - 1].AccountType)}, Balance:{accounts[selection - 1].Balance:C}, Available Balance:{accounts[selection - 1].GetAvailableBalance():C} """);
                        decimal amount = GetAmount("Enter Amount: ");
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
                                Console.WriteLine("Insufficient Funds.");
                            }
                        }
                    }
                    else if (destAccNum == accounts[selection - 1].AccountNumber)
                    {
                        Console.WriteLine("Cannot Transfer to the same account.");
                    }
                    else { Console.WriteLine("Account Doesn't exist"); }

                }
                else { Console.WriteLine("Account Doesn't exist"); }
            }
            // My Statement
            if (option == 4)
            {
                Console.WriteLine("---My Statement---");
                List<Account> accounts = GetAndPrintAccounts();
                int selection = GetSelection("Select an Account: ");
                if (selection != -1 && selection.IsInRange(1, accounts.Count))
                {
                    int pageNum = 1;  
                    string input = "";

                    while(input != "q")
                    {
                        // Returns paginated list of transactions
                        List<Transaction> transactions = _customerManager.GetTransactions(accounts[selection - 1].AccountNumber, pageNum);
                        Console.WriteLine("{0,-5}{1,-20}{2,-15}{3,-15}{4,-15:C}{5,-25}{6,-20}", "ID", "Type", "Account", "Destination", "Amount", "Time", "Comment");
                        foreach (Transaction tran in transactions)
                        {
                            var dateTime = tran.TransactionTimeUtc.ToString("MM/dd/yyyy h:mm tt");
                            Console.WriteLine("{0,-5}{1,-20}{2,-15}{3,-15}{4,-15:C}{5,-25}{6,-20}",
                                tran.TransactionID, TransactionType(tran.TransactionType), tran.AccountNumber, tran.DestinationAccountNumber, 
                                tran.Amount, dateTime, tran.Comment);
                        }
                        Console.WriteLine("Options: n (next) | p (previous) | q (quit) ");
                        Console.WriteLine($"""PageNumber: {pageNum}""");
                        Console.WriteLine("Enter Option: ");
                        input = ConsoleMethods.GetUserInput();
                        switch (input)
                        {
                            case "n":
                                pageNum++;
                                break;
                            case "p":
                                if(pageNum != 1)
                                    pageNum--;
                                break;
                        }
                    }

                } else { Console.WriteLine("Account Doesn't exist"); }

            }
            // Logout
            if(option == 5)
            {
                Console.Clear();
                runMenu = false;
            }
            // Exit program
            if(option == 6)
            {
                Console.WriteLine("Program ending.");
                runMenu = false;
                lView.EndProgram();
            }
        }

    }

    // Returns either the string "Deposit" or "Savings"
    private static string DepositOrSavings(string type)
    {
        string accType;
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

    //Returns a transaction type string depending on the input type
    private static string TransactionType(string type)
    {
        string tranType = "";
        switch (type)
        {
            case "D":
                tranType = "Deposit";
                break;
            case "W":
                tranType = "Withdraw";
                break;
            case "T":
                tranType = "Transfer";
                break;
            case "S":
                tranType = "Service Charge";
                break;
        }
        return tranType;
    }
    // Converts string to int and -1 if parse error found
    private static int IntConverter(string value)
    {
        if (!int.TryParse(value, out var option))
        {
            Console.WriteLine("Invalid input.");
            option = -1;
        }
        return option;
    }

    // Gets accounts from the customer manager and prints to the console
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

    // Gets a int as a selection from the user, and returns -1 if parse error found
    private static int GetSelection(string request)
    {
        Console.WriteLine(request);
        string input = ConsoleMethods.GetUserInput();
        int inputInt = IntConverter(input);
        return inputInt;
    }

    // Gets a decimal as an amount from the user, and returns -1 if parse error found
    private static decimal GetAmount(string request)
    {
        Console.WriteLine(request);
        string input = ConsoleMethods.GetUserInput();

        if (!decimal.TryParse(input, out var decimalInput))
        {
            Console.WriteLine("Invalid input.");
            decimalInput = -1;
        }
        return decimalInput;
    }
}

