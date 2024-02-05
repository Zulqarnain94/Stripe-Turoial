namespace Models;
public class PaymentRequest
{
    public int Amount { get; set; }
    // Add these properties to capture the customer's name and email
    public string CustomerName { get; set; }
    public string CustomerEmail { get; set; }
    public string ProductName { get; set; }
}
