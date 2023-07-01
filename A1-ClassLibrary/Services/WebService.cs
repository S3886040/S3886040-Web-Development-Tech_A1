namespace A1_ClassLibrary.Services;

using A1_ClassLibrary.Managers;
using A1_ClassLibrary.ModelDTO;
using Newtonsoft.Json;

public class WebService
{
    public static void GetAndSave(DBManager DBManager)
    {

        // Check if any people already exist and if they do stop.
        if (DBManager.Any())
            return;

        const string Url = "https://coreteaching01.csit.rmit.edu.au/~e103884/wdt/services/customers/";

        // Contact webservice.
        using var client = new HttpClient();
        var json = client.GetStringAsync(Url).Result;

        // Convert JSON into objects.
        var customers = JsonConvert.DeserializeObject<List<Customer>>(json, new JsonSerializerSettings
        {
            // See here for DateTime format string documentation:
            // https://learn.microsoft.com/en-au/dotnet/standard/base-types/custom-date-and-time-format-strings
            DateFormatString = "dd/MM/yyyy"
        });

        // Insert into database.
        foreach (var customer in customers)
        {
            DBManager.InsertCustomer(customer);
            DBManager.InsertLogin(customer.Login, customer.CustomerID);
            foreach (var account in customer.Accounts)
            {
                DBManager.InsertAccount(account);
                double balance = 0;
                foreach (var tran in account.Transactions)
                {
                    balance += tran.Amount;
                    // Prefilling blank transaction items eg D fro deposit
                    tran.TransactionType = 'D';
                    tran.AccountNumber = account.AccountNumber;  
                    tran.DestinationAccountNumber = account.AccountNumber;
                    DBManager.InsertTransaction(tran);
                }
                account.Balance = balance;
                DBManager.UpdateBalance(account, balance);
            }

        }
    }
}
