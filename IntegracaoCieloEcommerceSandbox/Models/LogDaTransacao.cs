namespace IntegracaoCieloEcommerceSandbox.Models
{
    public class Customer
    {
        public string? Name { get; set; }
    }

    public class CreditCard
    {
        public string? CardNumber { get; set; }
        public string? Holder { get; set; }
        public string? ExpirationDate { get; set; }
        public string? PaymentAccountReference { get; set; }
    }

    public class Link
    {
        public string? Method { get; set; }
        public string? Rel { get; set; }
        public string? Href { get; set; }
    }

    public class Payment
    {
        public string? Tid { get; set; }
        public string? ProofOfSale { get; set; }
        public string? AuthorizationCode { get; set; }
        public string? SoftDescriptor { get; set; }
        public string? Provider { get; set; }
        public bool IsQrCode { get; set; }
        public int Amount { get; set; }
        public string? ReceivedDate { get; set; }
        public int Status { get; set; }
        public bool IsSplitted { get; set; }
        public string ReturnMessage { get; set; } = null!;
        public string? ReturnCode { get; set; }
        public string PaymentId { get; set; } = null!;
        public string? Type { get; set; }
        public string? Currency { get; set; }
        public string? Country { get; set; }
        public List<Link>? Links { get; set; }
        public CreditCard? CreditCard { get; set; }
    }

    public class LogDaTransacao
    {
        public string? MerchantOrderId { get; set; }
        public Customer? Customer { get; set; }
        public Payment Payment { get; set; } = null!;
    
    }
}
