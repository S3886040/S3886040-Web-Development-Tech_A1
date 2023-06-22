namespace A1_ClassLibrary.ModelDTO;

public class Account
{
    public int AccountNumber { get; set; }
    public char AccountType { get; set; }
    public int CustomerID { get; set; }
    public double Balance { get; set; }
    public List<Transaction> Transactions { get; set; }

}
