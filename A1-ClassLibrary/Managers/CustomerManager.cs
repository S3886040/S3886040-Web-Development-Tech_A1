
using A1_ClassLibrary.BusinessModels;
using Azure;
using System.Collections.Generic;

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

    public async Task<decimal> Deposit(Account account, decimal amount, string comment)
    {
        decimal balance = -1;
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
        decimal newBalance = -1;
        int index = 0;
        while (_accounts[index] != account) { index++; }
        var task = _accounts[index].Withdraw(amount, comment);
        if (_accounts[index].GetAvailableBalance() >= amount)
            newBalance = await task;

        return newBalance;
    }

    public bool CheckAccountExists(int accNum)
    {
        return _dbManager.CheckAccountExists(accNum);
    }

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
        if (_accounts[index].GetAvailableBalance() > amount)
        {
            var tasks = new List<Task>()
            {
                _accounts[index].Transfer(amount, destAccNum, comment),
                _dbManager.AddAmount(amount, destAccNum),
                _dbManager.AddTransaction(tran)
            };

            await Task.WhenAll(tasks);
            success = true;
        };

        return success;
    }

    public List<Transaction> GetTransactions(int accNum, int pageNum)
    {
        var list = _dbManager.GetTransactions(accNum, pageNum);
        return list;
    }
}
