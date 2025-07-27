# ProjetConversionCuisine

Ce projet est une application console .NET 8 permettant de convertir des recettes depuis un fichier XML vers un fichier JSON, puis de les rechercher, filtrer, grouper et trier directement dans la console.

## Utilisation

1. Placer vos recettes dans le fichier `recipes.xml`.
2. Exécuter l'application depuis la console : `dotnet run`.
3. L'application vous propose de convertir `recipes.xml` en `recipes.json`.
4. Les recettes sont ensuite chargées depuis `recipes.json`.
5. Saisissez un mot-clé pour filtrer les recettes.
6. Indiquez éventuellement un ingrédient à exclure des résultats.
7. Filtrez par difficulté ou par calories maximales si souhaité.
8. Choisissez si vous souhaitez trier par temps de préparation ou par calories.
9. Optez éventuellement pour un affichage groupé par difficulté.
10. Les recettes correspondantes sont affichées dans la console.
11. Vous pouvez ensuite choisir d'exporter ces recettes vers un fichier `export.json`.

## Structure du projet

- **Models/Recipe.cs** : modèle représentant une recette.
- **Services/RecipeService.cs** : contient les méthodes de chargement, recherche, tri, affichage et conversion XML -> JSON.
- **Program.cs** : point d'entrée de l'application utilisant `RecipeService`.

La dépendance principale est `System.Text.Json` pour la lecture et l'écriture du JSON.