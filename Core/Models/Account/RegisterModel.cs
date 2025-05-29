namespace Core.Models.Account
{
	public class RegisterModel
	{
		/// <summary>
		/// User Email
		/// </summary>
		/// <example>JohnSkyrim@example.com</example>
		public string Email {get; set;} = string.Empty;
		/// <summary>
		/// User First-Name
		/// </summary>
		/// <example>John</example>
		public string FirstName {get; set;} = string.Empty;
		/// <summary>
		/// User Last-Name
		/// </summary>
		/// <example>Skyrim</example>
		public string LastName {get; set;} = string.Empty;
		/// <summary>
		/// User Image
		/// </summary>
		public IFormFile Image {get; set;} = null;
		/// <summary>
		/// User Password
		/// </summary>
		/// <example>Skyrim23</example>
		public string Password {get; set;} = string.Empty;
	}
}
