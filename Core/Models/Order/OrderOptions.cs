using Core.Models.General;

namespace Core.Models.Order;

public class OrderOptions {
	public List<SimpleModel> Cities {get; set;}
	public List<SimpleModel> PostDepartments {get; set;}
	public List<SimpleModel> PaymentTypes {get; set;}
}
