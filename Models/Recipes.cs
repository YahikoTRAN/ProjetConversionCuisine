namespace ProjetConversionCuisine.Models
{
    public class Recipe
    {
        public string Nom { get; set; } = string.Empty;
        public int TempsPreparation { get; set; }
        public string Difficulte { get; set; } = string.Empty;
        public List<string> Ingredients { get; set; } = new();
        public int Calories { get; set; }
    }
}