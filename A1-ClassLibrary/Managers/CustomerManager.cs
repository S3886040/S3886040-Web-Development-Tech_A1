
using A1_ClassLibrary.BusinessModels;
using System;
using System.Reflection;
using System.Transactions;

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

    public string LoginUser(int loginID)
    {

        _customer = _dbManager.GetCustomer(loginID);
        _accounts = _dbManager.GetAccounts(_customer.CustomerID);
        return _customer.Name;
    }
    public List<Account> GetAccounts()
    {
        return _accounts;
    }

    public decimal Deposit(Account account, decimal amount, string comment)
    {
        decimal balance = -1;
        if(amount != 0) 
        {
            int index = 0;
            while (_accounts[index] != account) { index++; }
            balance = _accounts[index].Deposit(amount, comment);
        }
        
        return balance;
    }

    public decimal Withdraw(Account account, decimal amount, string comment)
    {
        decimal newBalance = -1;
        int index = 0;
        while (_accounts[index] != account) { index++; }
        decimal balance = _accounts[index].GetBalance();
        if (balance - amount > 0)
        {

            if(_accounts[index].GetAvailableBalance() > amount)
                newBalance = _accounts[index].Withdraw(amount, comment);
        } 
   
        return newBalance;
    }

   
}
