namespace culinary_crafts_dotnet_mvc.Models;

public class Recipes : Base
{
    public string Name { get; set; }
    
    public List<Ingredients> Ingredients { get; set; }
    public decimal? Rating { get; set; }
    public string? Difficulty { get; set; }
    
    public int? Category { get; set; }
    public string CategoryName { get; set; }
}