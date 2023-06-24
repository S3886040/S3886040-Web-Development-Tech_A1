// See https://aka.ms/new-console-template for more information
using A1_ClassLibrary.Services;
using Microsoft.Extensions.Configuration;

using A1_ClassLibrary.Managers;
using ConsoleApp.view;
using A1_ClassLibrary.BusinessModels;

public static class Program
{
    private static void Main()
    {
        var configuration = new ConfigurationBuilder().AddJsonFile("config.json").Build();
        var connectionString = configuration.GetConnectionString("ServerConnectionString");

        var DBManager = new DBManager(connectionString);
        var CustomerManager = new CustomerManager(connectionString);
        WebService.GetAndSave(DBManager);

        var login = new LoginView(DBManager, CustomerManager);

    }
}