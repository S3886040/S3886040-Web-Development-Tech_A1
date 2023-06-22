namespace A1_ClassLibrary.modelDTO;

public class Transaction
{
    public int TransactionID { get; set; }
    public char TransactionType { get; set; }
    public int AccountNumber { get; set; }
    public int DestinationAccountNumber { get; set; }
    public double Amount { get; set; }
    public string Comment { get; set; }
    public string TransactionTimeUtc { get; set; }

}

