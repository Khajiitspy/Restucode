namespace Restucode.Models.Category
{
    public class CategoryEditModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public IFormFile? Image { get; set; }

        public string? ViewImage { get; set; }
    }
}
