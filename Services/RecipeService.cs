using System.Collections.Generic;
using System.Text.Json;
using ClosedXML.Excel;
using ProjetConversionCuisine.Models;

namespace ProjetConversionCuisine.Services;

public class RecipeService
{
    public List<Recipe> LoadRecipes(string path)
    {
        if (!File.Exists(path)) return new List<Recipe>();
        string json = File.ReadAllText(path);
        var options = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
        var recipes = JsonSerializer.Deserialize<List<Recipe>>(json, options);
        return recipes ?? new List<Recipe>();
    }

    public List<Recipe> Search(List<Recipe> recipes, string keyword)
    {
        if (string.IsNullOrWhiteSpace(keyword)) return recipes;
        keyword = keyword.ToLowerInvariant();
        return recipes.Where(r => r.Nom.ToLowerInvariant().Contains(keyword) ||
                                  r.Ingredients.Any(i => i.ToLowerInvariant().Contains(keyword)))
                      .ToList();
    }

    public List<Recipe> SortByCalories(List<Recipe> recipes)
    {
        return recipes.OrderBy(r => r.Calories).ToList();
    }

    public void DisplayRecipes(IEnumerable<Recipe> recipes)
    {
        foreach (var r in recipes)
        {
            Console.WriteLine($"Nom : {r.Nom}");
            Console.WriteLine($"Temps : {r.TempsPreparation} min");
            Console.WriteLine($"Difficulte : {r.Difficulte}");
            Console.WriteLine($"Ingredients : {string.Join(", ", r.Ingredients)}");
            Console.WriteLine($"Calories : {r.Calories}");
            Console.WriteLine(new string('-', 30));
        }
    }

    public void ExportToExcel(IEnumerable<Recipe> recipes, string path)
    {
        using var workbook = new XLWorkbook();
        var worksheet = workbook.AddWorksheet("Recettes");
        worksheet.Cell(1, 1).Value = "Nom";
        worksheet.Cell(1, 2).Value = "TempsPreparation";
        worksheet.Cell(1, 3).Value = "Difficulte";
        worksheet.Cell(1, 4).Value = "Ingredients";
        worksheet.Cell(1, 5).Value = "Calories";

        int row = 2;
        foreach (var r in recipes)
        {
            worksheet.Cell(row, 1).Value = r.Nom;
            worksheet.Cell(row, 2).Value = r.TempsPreparation;
            worksheet.Cell(row, 3).Value = r.Difficulte;
            worksheet.Cell(row, 4).Value = string.Join(", ", r.Ingredients);
            worksheet.Cell(row, 5).Value = r.Calories;
            row++;
        }
        workbook.SaveAs(path);
    }
}
