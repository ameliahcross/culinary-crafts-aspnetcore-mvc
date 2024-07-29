using System.ComponentModel.DataAnnotations;

namespace culinary_crafts_dotnet_mvc.Models;

public class Recipes : Base
{
    [Required]
    public string Name { get; set; }
    [Required]
    public List<Ingredients> Ingredients { get; set; }  = new List<Ingredients>();
    public decimal? Rating { get; set; }
    public string? Difficulty { get; set; }
    public int? Category { get; set; }
    public string? CategoryName { get; set; }
}