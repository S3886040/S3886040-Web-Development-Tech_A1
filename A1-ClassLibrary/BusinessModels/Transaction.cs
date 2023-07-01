namespace A1_ClassLibrary.BusinessModels;

public class Transaction
{
    public int TransactionID { get; set; }
    public required string TransactionType { get; set; }
    public required int AccountNumber { get; set; }
    public int? DestinationAccountNumber { get; set; }
    public required decimal Amount { get; set; }
    public string Comment { get; set; }
    public required DateTime TransactionTimeUtc { get; set; }

}

