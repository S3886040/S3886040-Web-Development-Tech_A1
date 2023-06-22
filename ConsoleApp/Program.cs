﻿// See https://aka.ms/new-console-template for more information
using A1_ClassLibrary.Services;
using Microsoft.Extensions.Configuration;

using A1_ClassLibrary.Managers;
using ConsoleApp.view;

public static class Program
{
    private static void Main()
    {
        var configuration = new ConfigurationBuilder().AddJsonFile("config.json").Build();
        var connectionString = configuration.GetConnectionString("ServerConnectionString");

        var DBManager = new DBManager(connectionString);
        WebService.GetAndSave(DBManager);

        var login = new LoginView(DBManager);

    }
}