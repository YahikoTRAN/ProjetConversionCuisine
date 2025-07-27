using ProjetConversionCuisine.Services;

var service = new RecipeService();

// Convertir le fichier XML en JSON puis charger le résultat
service.ConvertXmlToJson("recipes.xml", "recipes.json");
var recipes = service.LoadRecipes("recipes.json");

if (recipes.Count == 0)
{
    Console.WriteLine("Aucune recette trouvée.");
    return;
}

Console.Write("Mot cle de recherche (laisser vide pour tout afficher): ");
string? keyword = Console.ReadLine();
var result = service.Search(recipes, keyword ?? string.Empty);

Console.Write("Trier par calories ? (o/n): ");
var sortInput = Console.ReadLine();
if (!string.IsNullOrWhiteSpace(sortInput) && sortInput.Trim().ToLower() == "o")
{
    result = service.SortByCalories(result).ToList();
}

service.DisplayRecipes(result);

Console.Write("Exporter ces recettes vers un fichier JSON ? (o/n): ");
var exportInput = Console.ReadLine();
if (!string.IsNullOrWhiteSpace(exportInput) && exportInput.Trim().ToLower() == "o")
{
    service.ExportToJson(result, "export.json");
}
