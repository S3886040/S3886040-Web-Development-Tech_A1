
using A1_ClassLibrary.BusinessModels;
using A1_ClassLibrary.Utilities;
using Microsoft.Data.SqlClient;
using System.Data;


namespace A1_ClassLibrary.Managers;

internal class CustomerDBManager
{
    private readonly String _connectionString;
    internal CustomerDBManager(String connectionString)
    {
        _connectionString = connectionString;
    }

    internal Customer GetCustomer(int loginID)
    {
        using var connection = new SqlConnection(_connectionString);

        using var command = connection.CreateCommand();
        command.CommandText = "SELECT * FROM Customer INNER JOIN Login ON (Login.CustomerID = Customer.CustomerID) WHERE LoginID = @loginID";
        command.Parameters.AddWithValue("loginID", loginID);

        using var adapter = new SqlDataAdapter(command);

        var table = new DataTable();
        adapter.Fill(table);

        var customer = new Customer();
        foreach (DataRow row in table.Rows)
        {
            customer.CustomerID = row.Field<int>("CustomerID");
            customer.Name = row.Field<string>("Name");
            customer.Address = row.Field<string>("Address");
            customer.City = row.Field<string>("City");
            customer.Postcode = row.Field<string>("Postcode");
        }
        return customer;
    }

    internal List<Account> GetAccounts(int customerID)
    {
        using var connection = new SqlConnection(_connectionString);

        using var command = connection.CreateCommand();
        command.CommandText = "SELECT * FROM Account WHERE CustomerID = @custID";
        command.Parameters.AddWithValue("custID", customerID);

        return command.GetDataTable().Select().Select(x => new Account
        {
            AccountNumber = x.Field<int>("AccountNumber"),
            AccountType = x.Field<string>("AccountType"),
            Balance = x.Field<decimal>("Balance"),
            _dbManager = this
        }).ToList();

    }

    internal void UpdateBalance(Account account, decimal balance)
    {
        using var connection = new SqlConnection(_connectionString);
        connection.Open();

        using var command = connection.CreateCommand();
        command.CommandText =
            "update Account set Balance = @balance where AccountNumber = @accountNumber ";
        command.Parameters.AddWithValue("accountNumber", account.AccountNumber);
        command.Parameters.AddWithValue("balance", balance);

        command.ExecuteNonQuery();
    }
    internal decimal GetBalance(int accountNumber) 
    {
        using var connection = new SqlConnection(_connectionString);
        connection.Open();

        using var command = connection.CreateCommand();
        command.CommandText = "select balance from Account where AccountNumber = @accountNumber";
        command.Parameters.AddWithValue("accountNumber", accountNumber);
        decimal amount = (decimal) command.ExecuteScalar();

        return amount;
    }

    internal async Task AddTransaction(BusinessModels.Transaction tran)
    {
        using var connection = new SqlConnection(_connectionString);
        connection.Open();

        using var command = connection.CreateCommand();
        command.CommandText =
            "insert into [Transaction] values (@type, @accNum, @desAccNum, @amount, @comment, @time ) ";
        command.Parameters.AddWithValue("type", tran.TransactionType);
        command.Parameters.AddWithValue("accNum", tran.AccountNumber);
        command.Parameters.AddWithValue("desAccNum", tran.DestinationAccountNumber.GetObjectOrDbNull());
        command.Parameters.AddWithValue("amount", tran.Amount);
        command.Parameters.AddWithValue("comment", tran.Comment);
        command.Parameters.AddWithValue("time", tran.TransactionTimeUtc);


        await command.ExecuteNonQueryAsync();
    }

    internal bool CheckAccountExists(int accNum)
    {
        using var connection = new SqlConnection(_connectionString);
        connection.Open();

        using var command = connection.CreateCommand();
        command.CommandText = "select count(*) from Account where AccountNumber = @accNum";
        command.Parameters.AddWithValue("accNum", accNum);

        var count = (int)command.ExecuteScalar();

        return count > 0;
    }

    internal async Task AddAmount(decimal amount, int accNum) 
    {
        using var connection = new SqlConnection(_connectionString);
        connection.Open();

        using var command = connection.CreateCommand();
        command.CommandText = "update Account set Balance = Balance + @amount where AccountNumber = @accNum";
        command.Parameters.AddWithValue("accNum", accNum);
        command.Parameters.AddWithValue("amount", amount);

        await command.ExecuteNonQueryAsync();

    }

    internal List<BusinessModels.Transaction> GetTransactions(int accNum, int pageNum)
    {
        using var connection = new SqlConnection(_connectionString);

        using var command = connection.CreateCommand();
        command.CommandText = 
            "DECLARE @PageNumber AS INT " +
            "DECLARE @RowsOfPage AS INT " +
            "SET @PageNumber=@pageNum " +
            "SET @RowsOfPage=4 " +
            "SELECT * FROM [Transaction] " +
            "WHERE AccountNumber = @accNum " +
            "ORDER BY TransactionTimeUtc " +
            "OFFSET (@PageNumber-1)*@RowsOfPage ROWS " +
            "FETCH NEXT @RowsOfPage ROWS ONLY";
        command.Parameters.AddWithValue("accNum", accNum);
        command.Parameters.AddWithValue("pageNum", pageNum);

        return command.GetDataTable().Select().Select(x => new BusinessModels.Transaction
        {
            TransactionID = x.Field<int>("TransactionID"),
            TransactionType = x.Field<string>("TransactionType"),
            AccountNumber = x.Field<int>("AccountNumber"),
            DestinationAccountNumber = x.Field<int?>("DestinationAccountNumber"),
            Amount = x.Field<decimal>("Amount"),
            TransactionTimeUtc = x.Field<DateTime>("TransactionTimeUtc"),
            Comment = x.Field<string>("Comment")
        }).ToList();
    }
}
