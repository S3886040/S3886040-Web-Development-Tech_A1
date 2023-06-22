namespace A1_ClassLibrary.Managers;

using A1_ClassLibrary.modelDTO;
using A1_ClassLibrary.ModelDTO;
using Microsoft.Data.SqlClient;
using System;

public class DBManager 
{
    private readonly String _connectionString;
    public DBManager(String connectionString)
    {
        _connectionString = connectionString;
    }

    public bool Any()
    {
        using var connection = new SqlConnection(_connectionString);
        connection.Open();

        using var command = connection.CreateCommand();
        command.CommandText = "select count(*) from Customer";

        var count = (int)command.ExecuteScalar();

        return count > 0;
    }

    internal void InsertCustomer(Customer customer)
    {
        using var connection = new SqlConnection(_connectionString);
        connection.Open();
        Console.WriteLine(customer.CustomerID);
        using var command = connection.CreateCommand();
        command.CommandText =
            "insert into Customer (CustomerID, Name, Address, City, Postcode) values (@customerID, @name, @address, @city, @postcode)";
        command.Parameters.AddWithValue("customerID", customer.CustomerID);
        command.Parameters.AddWithValue("name", customer.Name);
        if(customer.Address != null) 
            command.Parameters.AddWithValue("address", customer.Address);
        else
            command.Parameters.AddWithValue("address", DBNull.Value);
        if( customer.City != null)
            command.Parameters.AddWithValue("city", customer.City);
        else
            command.Parameters.AddWithValue("city", DBNull.Value);
        if(customer.Postcode != null)
            command.Parameters.AddWithValue("postcode", customer.Postcode);
        else
            command.Parameters.AddWithValue("postcode", DBNull.Value);
        command.ExecuteNonQuery();
    }

    internal void InsertAccount(Account account)
    {
        using var connection = new SqlConnection(_connectionString);
        connection.Open();

        using var command = connection.CreateCommand();
        command.CommandText =
            "insert into Account (AccountNumber, AccountType, CustomerID, Balance) values (@accountNumber, @accountType, @customerID, @balance)";
        command.Parameters.AddWithValue("accountNumber", account.AccountNumber);
        command.Parameters.AddWithValue("accountType", account.AccountType);
        command.Parameters.AddWithValue("customerID", account.CustomerID);
        command.Parameters.AddWithValue("balance", account.Balance);

        command.ExecuteNonQuery();
    }

    internal void InsertTransaction(Transaction tran)
    {
        using var connection = new SqlConnection(_connectionString);
        connection.Open();

        using var command = connection.CreateCommand();
        command.CommandText =
            "insert into [Transaction] (TransactionType, AccountNumber, DestinationAccountNumber, Amount, Comment, TransactionTimeUtc) " +
            "values (@transactionType, @accountNumber, @destinationAccountNumber, @amount, @comment, @transactionTimeUtc)";
        command.Parameters.AddWithValue("transactionType", tran.TransactionType);
        command.Parameters.AddWithValue("accountNumber", tran.AccountNumber);
        command.Parameters.AddWithValue("destinationAccountNumber", tran.DestinationAccountNumber);
        command.Parameters.AddWithValue("amount", tran.Amount);
        if(tran.Comment != null)
            command.Parameters.AddWithValue("comment", tran.Comment);
        else
            command.Parameters.AddWithValue("comment", DBNull.Value);
        command.Parameters.AddWithValue("transactionTimeUtc", tran.TransactionTimeUtc);

        command.ExecuteNonQuery();
    }

    internal void InsertLogin(Login login, int custID)
    {
        using var connection = new SqlConnection(_connectionString);
        connection.Open();

        using var command = connection.CreateCommand();
        command.CommandText =
            "insert into Login (LoginID, CustomerID, PasswordHash) " +
            "values (@loginID, @customerID, @passwordHash)";
        command.Parameters.AddWithValue("loginID", login.LoginId);
        command.Parameters.AddWithValue("customerID", custID);
        command.Parameters.AddWithValue("passwordHash", login.PasswordHash);

        command.ExecuteNonQuery();
    }

    internal void UpdateBalance(Account account, double balance)
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
}
