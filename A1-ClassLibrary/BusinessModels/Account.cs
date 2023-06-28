
using A1_ClassLibrary.Managers;

namespace A1_ClassLibrary.BusinessModels;

public class Account
{
    public int AccountNumber { get; set; }
    public string AccountType { get; set; }
    internal int CustomerID { get; set; }
    public decimal Balance { get; set; }
    internal int FreeTransactions { get; set; } = 2;

    internal CustomerDBManager _dbManager { get; set; }

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
            TransactionType = 'D',
            AccountNumber = AccountNumber,
            DestinationAccountNumber = AccountNumber,
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
            TransactionType = 'W',
            AccountNumber = AccountNumber,
            DestinationAccountNumber = null,
            Amount = amount,
            Comment = comment,
            TransactionTimeUtc = DateTime.Now
        });
        UpdateBalance(Balance - amount);

        if (FreeTransactions == 0)
        {
            await AddServiceCharge();
            Balance -= amount;
        } else
        {
            FreeTransactions -= 1;
        }
        return Balance;
    }

    internal async Task<decimal> Transfer(decimal amount, int destAcc, string comment)
    {
        await _dbManager.AddTransaction(new BusinessModels.Transaction()
        {
            TransactionType = 'T',
            AccountNumber = AccountNumber,
            DestinationAccountNumber = destAcc,
            Amount = amount,
            Comment = comment,
            TransactionTimeUtc = DateTime.Now
        });
        UpdateBalance(Balance - amount);

        if (FreeTransactions == 0)
        {
            await AddServiceCharge();
            Balance -= amount;
        }
        else
        {
            FreeTransactions -= 1;
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

    private async Task AddServiceCharge()
    {
        await _dbManager.AddTransaction(new Transaction()
        {
            TransactionType = 'S',
            AccountNumber = AccountNumber,
            DestinationAccountNumber = AccountNumber,
            Amount = 0.05M,
            Comment = "Service Charge",
            TransactionTimeUtc = DateTime.Now
        });
        UpdateBalance(Balance - 0.05M);
    }
}


