namespace A1_ClassLibrary.ModelDTO;

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

