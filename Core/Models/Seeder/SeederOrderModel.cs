namespace Core.Models.Seeder;

public class SeederOrderItemModel
{
    public long productVariantId { get; set; } = default!;
    public int count { get; set; }
    public decimal priceBuy { get; set; }
}

public class SeederOrderModel
{
    public string userEmail { get; set; } = "";
    public long status { get; set; } = 0;
    public List<SeederOrderItemModel> items { get; set; } = new();
}