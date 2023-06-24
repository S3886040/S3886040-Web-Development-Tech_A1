
using A1_ClassLibrary.BusinessModels;

namespace A1_ClassLibrary.Managers;

public class CustomerManager 
{
    private readonly CustomerDBManager _dbManager;
    private Customer _customer;
    private int _freeTransactions;
    public CustomerManager(String connectionString)
    {
 
        _dbManager = new CustomerDBManager(connectionString);
        _freeTransactions = 0;
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

    public decimal Deposit(Account account, decimal amount) 
    {
        decimal balance = _dbManager.GetBalance(account.AccountNumber);
        _dbManager.UpdateBalance(account, balance + amount);  
        return _dbManager.GetBalance(account.AccountNumber);
    }

}
