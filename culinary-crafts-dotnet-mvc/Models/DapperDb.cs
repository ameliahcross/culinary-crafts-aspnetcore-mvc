using Microsoft.Data.SqlClient;
using Dapper;

namespace culinary_crafts_dotnet_mvc.Models;

public class DapperDb
{
    SqlConnection conn = new SqlConnection("Database=culinarycrafts__db; User Id=sa;Password=SqlServer1234!; Trusted_Connection=False; TrustServerCertificate=True;");

    public async Task<List<Recipes>> GetAllRecipesWithIngredients()
    {
        var sql = @"
                   SELECT Recipes.Id, Recipes.Name, Recipes.Rating, Recipes.Difficulty, Recipes.Category,
                        Ingredients.Id, Ingredients.Name as Name,
                        Categories.Name as CategoryName
                    FROM Recipes 
                    LEFT JOIN RecipeIngredient ON Recipes.Id = RecipeIngredient.RecipeId
                    LEFT JOIN Ingredients ON RecipeIngredient.IngredientId = Ingredients.Id
                    LEFT JOIN Categories ON Recipes.Category = Categories.Id;
                ";

        var recipes = new List<Recipes>();
        Recipes currentRecipe = null;

        var result = await conn.QueryAsync<Recipes, Ingredients, string, Recipes>(
            sql,
            (recipe, ingredient, categoryName) => {
                if (currentRecipe == null || currentRecipe.Id != recipe.Id) {
                    currentRecipe = recipe;
                    currentRecipe.Ingredients = new List<Ingredients>();
                    recipes.Add(currentRecipe);
                }
                if (ingredient != null) {
                    currentRecipe.Ingredients.Add(ingredient);
                }

                currentRecipe.CategoryName = categoryName;
                return currentRecipe;
            },
            splitOn: "Id, CategoryName"
        );

        return recipes;
    }

    public async Task<int> GetRecipeById(int Id)
    {
        var recipeId = await conn.QuerySingleOrDefaultAsync<int>($"select * from Recipes where Id={Id}");
        return recipeId;
    }
    
    public async Task<Recipes> IncreaseRating(int Id)
    {
        var idRecipe = await GetRecipeById(Id);
        var recipeRated = await conn.QuerySingleAsync<Recipes>($"update Recipes set rating = rating + 0.5 where Id={idRecipe};" +
                                                                 $"select * from Recipes where Id={idRecipe}");
        if (recipeRated.Rating > 5)
        {
            await conn.ExecuteAsync($"update Recipes set rating = 5 where Id={idRecipe}");
        }
        return recipeRated;
    }
    
    public async Task<Recipes> DecreaseRating(int Id)
    {
        var idRecipe = await GetRecipeById(Id);
        var recipeRated = await conn.QuerySingleAsync<Recipes>($"update Recipes set rating = rating - 0.5 where Id={idRecipe};" +
                                                               $"select * from Recipes where Id={idRecipe}");
        if (recipeRated.Rating <=1 )
        {
            await conn.ExecuteAsync($"update Recipes set rating = 1 where Id={idRecipe}");
        }
        return recipeRated;
    }
}