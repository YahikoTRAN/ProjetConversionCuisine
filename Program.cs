using System;
using ProjetConversionCuisine.Services;

var service = new RecipeService();

// Load recipes
var recipes = service.LoadRecipes("recipes.json");
if (recipes.Count == 0)
{
    Console.WriteLine("Aucune recette chargee.");
    return;
}

Console.Write("Mot cle de recherche (laisser vide pour aucune) : ");
var keyword = Console.ReadLine() ?? string.Empty;
var results = service.Search(recipes, keyword);
results = service.SortByCalories(results);

service.DisplayRecipes(results);

Console.Write("Exporter vers Excel ? (o/n) : ");
var export = Console.ReadLine();
if (export?.Trim().ToLower() == "o")
{
    service.ExportToExcel(results, "export.xlsx");
    Console.WriteLine("Fichier export.xlsx cree.");
}