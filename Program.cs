using ProjetConversionCuisine.Services;

var service = new RecipeService();

// Proposer la conversion du XML en JSON avant de charger les recettes
Console.Write("Convertir le fichier XML en JSON ? (o/n): ");
var convertInput = Console.ReadLine();
if (!string.IsNullOrWhiteSpace(convertInput) && convertInput.Trim().ToLower() == "o")
{
    service.ConvertXmlToJson("recipes.xml", "recipes.json");
}

var recipes = service.LoadRecipes("recipes.json");

if (recipes.Count == 0)
{
    Console.WriteLine("Aucune recette trouvée.");
    return;
}

Console.Write("Mot cle de recherche (laisser vide pour tout afficher): ");
string? keyword = Console.ReadLine();
var result = service.Search(recipes, keyword ?? string.Empty);

Console.Write("Ingredient a exclure (laisser vide pour aucun): ");
string? exclude = Console.ReadLine();
result = service.ExcludeIngredient(result, exclude ?? string.Empty);

Console.Write("Filtrer par difficulte (laisser vide pour aucune): ");
string? diffInput = Console.ReadLine();
result = service.FilterByDifficulty(result, diffInput ?? string.Empty);

Console.Write("Calories maximales (laisser vide pour aucune limite): ");
string? maxCalInput = Console.ReadLine();
if (int.TryParse(maxCalInput, out var maxCal))
{
    result = service.FilterByMaxCalories(result, maxCal);
}

Console.Write("Trier par temps de preparation ? (o/n): ");
var sortTimeInput = Console.ReadLine();
if (!string.IsNullOrWhiteSpace(sortTimeInput) && sortTimeInput.Trim().ToLower() == "o")
{
    result = service.SortByTime(result).ToList();
}

Console.Write("Trier par calories ? (o/n): ");
var sortInput = Console.ReadLine();
if (!string.IsNullOrWhiteSpace(sortInput) && sortInput.Trim().ToLower() == "o")
{
    result = service.SortByCalories(result).ToList();
}

Console.Write("Grouper par difficulte ? (o/n): ");
var groupInput = Console.ReadLine();
if (!string.IsNullOrWhiteSpace(groupInput) && groupInput.Trim().ToLower() == "o")
{
    var groups = service.GroupByDifficulty(result);
    service.DisplayGroupedRecipes(groups);
}
else
{
    service.DisplayRecipes(result);
}

Console.Write("Exporter ces recettes vers un fichier JSON ? (o/n): ");
var exportInput = Console.ReadLine();
if (!string.IsNullOrWhiteSpace(exportInput) && exportInput.Trim().ToLower() == "o")
{
    service.ExportToJson(result, "export.json");
}
