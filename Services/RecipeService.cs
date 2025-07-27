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

        public IEnumerable<Recipe> SortByTime(IEnumerable<Recipe> recipes)
        {
            return recipes.OrderBy(r => r.TempsPreparation);
        }

        public IEnumerable<Recipe> FilterByDifficulty(IEnumerable<Recipe> recipes, string difficulty)
        {
            if (string.IsNullOrWhiteSpace(difficulty))
                return recipes;

            difficulty = difficulty.ToLowerInvariant();
            return recipes.Where(r => r.Difficulte.ToLowerInvariant().Contains(difficulty));
        }

        public IEnumerable<Recipe> FilterByMaxCalories(IEnumerable<Recipe> recipes, int maxCalories)
        {
            if (maxCalories <= 0)
                return recipes;

            return recipes.Where(r => r.Calories <= maxCalories);
        }

        public IEnumerable<IGrouping<string, Recipe>> GroupByDifficulty(IEnumerable<Recipe> recipes)
        {
            return recipes.GroupBy(r => r.Difficulte);
        }

        public void DisplayGroupedRecipes(IEnumerable<IGrouping<string, Recipe>> groups)
        {
            foreach (var group in groups)
            {
                Console.WriteLine($"--- {group.Key} ---");
                DisplayRecipes(group);
            }
        }

        public IEnumerable<Recipe> ExcludeIngredient(IEnumerable<Recipe> recipes, string ingredient)
        {
            if (string.IsNullOrWhiteSpace(ingredient))
                return recipes;

            ingredient = ingredient.ToLowerInvariant();
            return recipes.Where(r => !r.Ingredients.Any(i => i.ToLowerInvariant().Contains(ingredient)));
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

        public void ConvertJsonToXml(string jsonPath, string xmlPath)
        {
            if (!File.Exists(jsonPath))
            {
                Console.WriteLine($"Fichier {jsonPath} introuvable.");
                return;
            }

            string json = File.ReadAllText(jsonPath);
            var options = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            };
            var recipes = JsonSerializer.Deserialize<List<Recipe>>(json, options) ?? new List<Recipe>();

            var doc = new XDocument(
                new XElement("Recettes",
                    recipes.Select(r =>
                        new XElement("Recette",
                            new XElement("Nom", r.Nom),
                            new XElement("TempsPreparation", r.TempsPreparation),
                            new XElement("Difficulte", r.Difficulte),
                            new XElement("Ingredients", r.Ingredients.Select(i => new XElement("Ingredient", i))),
                            new XElement("Calories", r.Calories)
                        ))));

            doc.Save(xmlPath);
            Console.WriteLine($"Conversion realisee : {xmlPath}");
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