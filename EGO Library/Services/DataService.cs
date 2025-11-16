using EGO_Library.Data;
using EGO_Library.Models;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EGO_Library.Services
{
    public class DataService
    {
        private readonly AppDbContext _context;

        public DataService()
        {
            _context = new AppDbContext();
        }

        // Получить все дары
        public async Task<List<EgoGift>> GetAllGiftsAsync()
        {
            return await _context.EgoGifts
                .OrderBy(g => g.Tier)
                .ThenBy(g => g.Name)
                .ToListAsync();
        }

        // Поиск по имени
        public async Task<List<EgoGift>> SearchGiftsAsync(string searchText)
        {
            return await _context.EgoGifts
                .Where(g => g.Name.Contains(searchText))
                .ToListAsync();
        }

        // Фильтрация по Tier и Status
        public async Task<List<EgoGift>> GetFilteredGiftsAsync(int? tier = null, string status = null)
        {
            var query = _context.EgoGifts.AsQueryable();

            if (tier.HasValue)
                query = query.Where(g => g.Tier == tier.Value);

            if (!string.IsNullOrEmpty(status))
                query = query.Where(g => g.Status == status);

            return await query.OrderBy(g => g.Name).ToListAsync();
        }

        // Получить дар по ID
        public async Task<EgoGift> GetGiftByIdAsync(int id)
        {
            return await _context.EgoGifts.FirstOrDefaultAsync(g => g.Id == id);
        }
    }
}