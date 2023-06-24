
using A1_ClassLibrary.BusinessModels;
using System.Transactions;

namespace A1_ClassLibrary.Managers;

public class CustomerManager 
{
    private readonly CustomerDBManager _dbManager;
    private Customer _customer;
    private int _freeTransactions;
    public CustomerManager(String connectionString)
    {
 
        _dbManager = new CustomerDBManager(connectionString);
        _freeTransactions = 2;
    }

    public string LoginUser(int loginID)
    {

        _customer = _dbManager.GetCustomer(loginID);
        return _customer.Name;
    }
    public List<Account> GetAccounts()
    {

        return _dbManager.GetAccounts(_customer.CustomerID);
    }

    public decimal Deposit(Account account, decimal amount, string comment) 
    {
        decimal balance = _dbManager.GetBalance(account.AccountNumber);

        var tran = new BusinessModels.Transaction()
        {
            TransactionType = 'D',
            AccountNumber = account.AccountNumber,
            DestinationAccountNumber = account.AccountNumber,
            Amount = amount,
            Comment = comment,
            TransactionTimeUtc = DateTime.Now
        };
        _dbManager.AddTransaction(tran);
        _dbManager.UpdateBalance(account, balance + amount);  
        return _dbManager.GetBalance(account.AccountNumber);
    }

    public decimal Withdraw(Account account, decimal amount, string comment)
    {
        decimal balance = _dbManager.GetBalance(account.AccountNumber);

        var tran = new BusinessModels.Transaction()
        {
            TransactionType = 'W',
            AccountNumber = account.AccountNumber,
            DestinationAccountNumber = account.AccountNumber,
            Amount = amount,
            Comment = comment,
            TransactionTimeUtc = DateTime.Now
        };
        _dbManager.AddTransaction(tran);
        _dbManager.UpdateBalance(account, balance - amount);
        return _dbManager.GetBalance(account.AccountNumber);
    }
}
