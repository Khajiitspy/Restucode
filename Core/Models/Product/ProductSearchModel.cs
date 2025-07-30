namespace Core.Models.Product;

public class ProductSearchModel{
	public string? Name {get; set;} 
	public long? CategoryId {get; set;} 
    public int Page { get; set; } = 1;
    public int PageSize { get; set; } = 10;
}
