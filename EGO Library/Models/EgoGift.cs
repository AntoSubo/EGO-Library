using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EGO_Library.Models
{
    [Table("EgoGifts")]
    public class EgoGift
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [MaxLength(100)]
        public string Name { get; set; }

        public int Tier { get; set; }

        [MaxLength(50)]
        public string Status { get; set; }

        [MaxLength(10)]
        public string Icon { get; set; }

        public string Description { get; set; }

        // JSON строки для списков (SQLite не поддерживает массивы)
        public string SourcesJson { get; set; }
        public string FusionRecipesJson { get; set; }

        // Вычисляемые свойства для удобства
        [NotMapped]
        public List<string> Sources =>
            string.IsNullOrEmpty(SourcesJson)
                ? new List<string>()
                : System.Text.Json.JsonSerializer.Deserialize<List<string>>(SourcesJson);

        [NotMapped]
        public List<string> FusionRecipes =>
            string.IsNullOrEmpty(FusionRecipesJson)
                ? new List<string>()
                : System.Text.Json.JsonSerializer.Deserialize<List<string>>(FusionRecipesJson);
    }
}