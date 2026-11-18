namespace HorseClub.Models
{
    public class CatalogViewModel
    {
        public List<Service> Services { get; set; } = new();
        public List<string> Categories { get; set; } = new();

        // Выбранные фильтры
        public string? SelectedCategory { get; set; }
        public decimal? MinPrice { get; set; }
        public decimal? MaxPrice { get; set; }
    }
}