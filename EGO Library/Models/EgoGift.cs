using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace EGO_Library.Models
{
    public class EgoGift
    {
        public int Id { get; set; }

        [Required]
        [MaxLength(100)]
        public string Name { get; set; } = string.Empty;

        public int Tier { get; set; }

        [MaxLength(50)]
        public string Status { get; set; } = string.Empty;

        [MaxLength(10)]
        public string Icon { get; set; } = string.Empty;

        // Основной эффект дара
        public string Effect { get; set; } = string.Empty;

        // Дополнительные свойства - ДОБАВЬ ЭТИ ТРИ СТРОКИ:
        public int? Cost { get; set; } // ЭГО Дальц
        public int? EXPaint { get; set; } // EX Paint
        public int? SellPrice { get; set; } // Стоимость продажи

        // Условия получения
        public string AcquisitionCondition { get; set; } = string.Empty;

        // Ключевые слова (для поиска и фильтрации)
        public string Keywords { get; set; } = string.Empty;

        public string Description { get; set; } = string.Empty;

        public virtual ICollection<Sources> Sources { get; set; } = new List<Sources>();
        public virtual ICollection<Recipe> ResultRecipes { get; set; } = new List<Recipe>();
        public virtual ICollection<Recipe> RequiredInRecipes { get; set; } = new List<Recipe>();

        public DateTime CreatedDate { get; set; } = DateTime.Now;
    }
}