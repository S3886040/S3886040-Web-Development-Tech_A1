Justin Healy
s3886040
https://github.com/rmit-wdt-sp2-2023/s3886040-a1/tree/master

Part 4: Design And Implementation

- Dependancy Injection:

- Dependancy Injection is design technique which allows construction classes to pass values or objects to other objects which are required for the general function of the recieving class. The objhects passed can be classes structs or interfaces. The advantage of this is to create cohesion between objects and promote the object oriented approach.

- Dependancy injection os displayed in my implementation in two key areas. 
	The first being the entry point to the application which is the program.cs file. This file creates the initial DBManager, which inserts the json recieved data from the supplied webservice, and the customerManager. Both of these objects are then passed to the loginView.cs file which uses the dbManager to confirm the login credentials and then passes the singular customer manager to the bankview.cs. The bankview requires this manager to access the model instance which holds the loggedin users details and performs actions based on the users interactions.

	The second instance of dependancy injection is shown from the customer manager. This class creates a new dbmanager from the CustomerDBManager.cs file which all loggedin db calls run through. This dependance is then passed on the the account class which uses the access to the db to update all altered values to its object members.

- Facade:

- A facade is a frontfacing object which acts as a facilitator between independant objects and brings them togehter in a cohesive fashion.

- The facade is displayed throughout my implementation by using the customerManager as the frontward facing public object which is then accessed by the bankview. The customerManager then manages the relations bewteen the customer, Account, Transaction and dependancy injection the programs requires.


The Required Keyword:

- The required keyword develops a hierachy for calling objects to adhere to when using objects using the required keyword. This assists in code consistency across classes and minimises misuse of objects.

- The required keyword is in the business model data type objects namely the Account class in the businessModels folder. The addition enforces the required values to be instatiated before use. This brings the code in line with the business rules from the customer and database enforcement of non null values.


Async and Await:

Async and await offer the programmer the possibility to easily access multithreading. Which increases the speed and efficiency of the program.

The transfer function requires mulitple calls to the database, to update the depositors balance, increase the receivers balance and then create the transactions to documetn the event. Using the async and await keywords we can use the multithreading capability to consume more of the cpu's power and complete these tasks faster and more efficiently. 

This method also has the added bonus of having the "complete all, or none" philosohpy. If there is an error on any of the tasks during the operation executed by the WhenAll method. Then none of the tasks will be completed.

Please see The transfer method in the CustomerManager.cs file for the implementation.










   