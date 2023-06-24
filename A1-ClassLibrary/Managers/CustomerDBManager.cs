
using A1_ClassLibrary.BusinessModels;
using A1_ClassLibrary.Utilities;
using Microsoft.Data.SqlClient;
using System;
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

        using var adapter = new SqlDataAdapter(command);

        var table = new DataTable();
        adapter.Fill(table);
/*        foreach(DataRow row in table.Rows) 
        {
            char c = row["AccountType"].ToChar();
            if (row["AccountType"] is string)
            {
                Console.WriteLine(row["AccountType"]);
            }
        }*/
        var accounts = new List<Account>();
        return command.GetDataTable().Select().Select(x => new Account
        {
            AccountNumber = x.Field<int>("AccountNumber"),
            AccountType = x.Field<string>("AccountType"),
            Balance = x.Field<decimal>("Balance"),
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
}
