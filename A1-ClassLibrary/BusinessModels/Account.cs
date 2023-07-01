
using A1_ClassLibrary.Managers;

namespace A1_ClassLibrary.BusinessModels;

public class Account
{
    public required int AccountNumber { get; set; }
    public required string AccountType { get; set; }
    internal int _customerID { get; set; }
    public decimal Balance { get; set; }
    internal int _freeTransactions { get; set; }

    internal CustomerDBManager _dbManager { get; set; }

    public Account() 
    {
        _freeTransactions = 2;
    }

    internal decimal GetBalance()
    {
        Balance = _dbManager.GetBalance(AccountNumber);
        return Balance;
    }

    internal void UpdateBalance(decimal balance)
    {
        _dbManager.UpdateBalance(this, balance);
        Balance = balance;
    }

    internal async Task<decimal> Deposit(decimal amount, string comment)
    {
        var tran = new BusinessModels.Transaction()
        {
            TransactionType = "D",
            AccountNumber = AccountNumber,
            DestinationAccountNumber = null,
            Amount = amount,
            Comment = comment,
            TransactionTimeUtc = DateTime.Now
        };
        await _dbManager.AddTransaction(tran);
        UpdateBalance(Balance + amount);
        return Balance;
    }

    internal async Task<decimal> Withdraw(decimal amount, string comment)
    {
        await _dbManager.AddTransaction(new BusinessModels.Transaction()
        {
            TransactionType = "W",
            AccountNumber = AccountNumber,
            DestinationAccountNumber = null,
            Amount = amount,
            Comment = comment,
            TransactionTimeUtc = DateTime.Now
        });
        UpdateBalance(Balance - amount);
        
        if (_freeTransactions == 0)
        {
            await AddServiceCharge(0.05M);
        } else
        {
            _freeTransactions -= 1;
        }
        return Balance;
    }

    internal async Task<decimal> Transfer(decimal amount, int destAcc, string comment)
    {
        await _dbManager.AddTransaction(new BusinessModels.Transaction()
        {
            TransactionType = "T",
            AccountNumber = AccountNumber,
            DestinationAccountNumber = destAcc,
            Amount = amount,
            Comment = comment,
            TransactionTimeUtc = DateTime.Now
        });
        UpdateBalance(Balance - amount);

        if (_freeTransactions == 0)
        {
            await AddServiceCharge(0.10M);
        }
        else
        {
            _freeTransactions -= 1;
        }
        return Balance;
    }

    public decimal GetAvailableBalance()
    {
        decimal availableBalance = 0;

        if (AccountType == "C" && Balance - 300 > 0)
            availableBalance = Balance - 300;
        if (AccountType == "S")
            availableBalance = Balance;

        return availableBalance;
    }

    private async Task AddServiceCharge(decimal charge)
    {
        await _dbManager.AddTransaction(new Transaction()
        {
            TransactionType = "S",
            AccountNumber = AccountNumber,
            DestinationAccountNumber = null,
            Amount = charge,
            Comment = "Service Charge",
            TransactionTimeUtc = DateTime.Now
        });
        UpdateBalance(Balance - 0.05M);
    }
}


