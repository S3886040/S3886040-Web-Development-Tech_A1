
using A1_ClassLibrary.BusinessModels;

namespace A1_ClassLibrary.Managers;

public class CustomerManager
{
    private readonly CustomerDBManager _dbManager;
    private Customer _customer;
    private List<Account> _accounts;
    public CustomerManager(String connectionString)
    {
        _dbManager = new CustomerDBManager(connectionString);
    }

    // LoginUser will get the details of the user to store in the manager instance
    public string LoginUser(int loginID)
    {

        _customer = _dbManager.GetCustomer(loginID);
        _accounts = _dbManager.GetAccounts(_customer._customerID);
        return _customer._name;
    }
    // Getter method for accounts
    public List<Account> GetAccounts()
    {
        return _accounts;
    }
    // Returns a task for deposit, task holds decimal value of balnce to present to user
    public async Task<decimal> Deposit(Account account, decimal amount, string comment)
    {
        
        decimal balance = -1;
        // Business rules say amount of 0 cannot be deposited
        if (amount != 0)
        {
            int index = 0;
            while (_accounts[index] != account) { index++; }
            var task = _accounts[index].Deposit(amount, comment);
            balance = await task;
        }

        return balance;
    }

    public async Task<decimal> Withdraw(Account account, decimal amount, string comment)
    {
        // Default return value
        decimal newBalance = -1;
        //Manager version of account is found
        int index = 0;
        while (_accounts[index] != account) { index++; }
   
        var task = _accounts[index].Withdraw(amount, comment);
        if (_accounts[index].GetAvailableBalance() >= amount)
            newBalance = await task;

        return newBalance;
    }
    // Used for validation purposes by the BankView
    public bool CheckAccountExists(int accNum)
    {
        return _dbManager.CheckAccountExists(accNum);
    }

    // The service charge and balance reduction is handled by account instance, where as 
    // the transfer of funds to the other account is taken care of by the CustomerManager
    public async Task<bool> Transfer(Account account, int destAccNum, decimal amount, string comment)
    {
        bool success = false;
        int index = 0;
        while (_accounts[index] != account) { index++; }

        var tran = new Transaction()
        {
            TransactionType = "T",
            AccountNumber = destAccNum,
            DestinationAccountNumber = null,
            Amount = amount,
            Comment = comment,
            TransactionTimeUtc = DateTime.Now
        };
        // Sufficient funds are checked, then tasks are passed to a list for execution
        if (_accounts[index].GetAvailableBalance() > amount)
        {
            var tasks = new List<Task>()
            {
                _accounts[index].Transfer(amount, destAccNum, comment),
                _dbManager.AddAmount(amount, destAccNum),
                _dbManager.AddTransaction(tran)
            };
            // All tasks are called and dropped if an issue arises in one or more
            await Task.WhenAll(tasks);
            success = true;
        };
        // boolean return value as flag for calling classes
        return success;
    }

    // Returns a paginated list of transactions from the dbmanager class
    public List<Transaction> GetTransactions(int accNum, int pageNum)
    {
        var list = _dbManager.GetTransactions(accNum, pageNum);
        return list;
    }
}
