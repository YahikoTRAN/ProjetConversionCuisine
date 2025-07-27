using System.Text.Json;
using System.Xml.Linq;
using ProjetConversionCuisine.Models;

namespace ProjetConversionCuisine.Services
{
    public class RecipeService
    {
        public List<Recipe> LoadRecipes(string path)
        {
            if (!File.Exists(path))
            {
                Console.WriteLine($"Fichier {path} introuvable.");
                return new List<Recipe>();
            }

            string json = File.ReadAllText(path);
            var options = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            };
            var recipes = JsonSerializer.Deserialize<List<Recipe>>(json, options);
            return recipes ?? new List<Recipe>();
        }

        public IEnumerable<Recipe> Search(IEnumerable<Recipe> recipes, string keyword)
        {
            if (string.IsNullOrWhiteSpace(keyword))
                return recipes;

            keyword = keyword.ToLowerInvariant();
            return recipes.Where(r => r.Nom.ToLowerInvariant().Contains(keyword) ||
                                      r.Ingredients.Any(i => i.ToLowerInvariant().Contains(keyword)));
        }

        public IEnumerable<Recipe> SortByCalories(IEnumerable<Recipe> recipes)
        {
            return recipes.OrderBy(r => r.Calories);
        }

        public void DisplayRecipes(IEnumerable<Recipe> recipes)
        {
            foreach (var r in recipes)
            {
                Console.WriteLine($"{r.Nom} - {r.Calories} calories - {r.TempsPreparation} min - {r.Difficulte}");
                Console.WriteLine("Ingredients: " + string.Join(", ", r.Ingredients));
                Console.WriteLine();
            }
        }

        public void ConvertXmlToJson(string xmlPath, string jsonPath)
        {
            if (!File.Exists(xmlPath))
            {
                Console.WriteLine($"Fichier {xmlPath} introuvable.");
                return;
            }

            var doc = XDocument.Load(xmlPath);
            var recipes = doc.Root?.Elements("Recette").Select(x => new Recipe
            {
                Nom = x.Element("Nom")?.Value ?? string.Empty,
                TempsPreparation = int.TryParse(x.Element("TempsPreparation")?.Value, out var tp) ? tp : 0,
                Difficulte = x.Element("Difficulte")?.Value ?? string.Empty,
                Ingredients = x.Element("Ingredients")?.Elements("Ingredient").Select(i => i.Value).ToList() ?? new List<string>(),
                Calories = int.TryParse(x.Element("Calories")?.Value, out var c) ? c : 0
            }).ToList();

            var options = new JsonSerializerOptions { WriteIndented = true };
            string json = JsonSerializer.Serialize(recipes, options);
            File.WriteAllText(jsonPath, json);
            Console.WriteLine($"Conversion realisee : {jsonPath}");
        }

        public void ExportToJson(IEnumerable<Recipe> recipes, string jsonPath)
        {
            var options = new JsonSerializerOptions { WriteIndented = true };
            string json = JsonSerializer.Serialize(recipes, options);
            File.WriteAllText(jsonPath, json);
            Console.WriteLine($"Recettes exportees dans {jsonPath}");
        }
    }
}