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
        // Получить все дары
        public async Task<List<EgoGift>> GetAllGiftsAsync()
        {
            using var context = new AppDbContext();
            return await context.EgoGifts
                .Include(g => g.Sources)
                .OrderBy(g => g.Tier)
                .ThenBy(g => g.Name)
                .ToListAsync();
        }

        // Получить дар по ID
        public async Task<EgoGift> GetGiftByIdAsync(int id)
        {
            using var context = new AppDbContext();
            return await context.EgoGifts
                .Include(g => g.Sources)
                .FirstOrDefaultAsync(g => g.Id == id);
        }

        // Поиск и фильтрация - ЗАМЕНИТЕ этот метод
        public async Task<List<EgoGift>> GetGiftsAsync(string searchText = null, int? tier = null, string status = null)
        {
            using var context = new AppDbContext();
            var query = context.EgoGifts.Include(g => g.Sources).AsQueryable();

            // Применяем фильтры
            if (!string.IsNullOrWhiteSpace(searchText))
            {
                query = query.Where(g => g.Name.Contains(searchText) ||
                                       g.Description.Contains(searchText) ||
                                       g.Status.Contains(searchText));
            }

            if (tier.HasValue)
            {
                query = query.Where(g => g.Tier == tier.Value);
            }

            if (!string.IsNullOrWhiteSpace(status))
            {
                query = query.Where(g => g.Status == status);
            }

            return await query.OrderBy(g => g.Tier).ThenBy(g => g.Name).ToListAsync();
        }

        // Получить уникальные Tier для фильтров
        public async Task<List<int>> GetAvailableTiersAsync()
        {
            using var context = new AppDbContext();
            return await context.EgoGifts
                .Select(g => g.Tier)
                .Distinct()
                .OrderBy(t => t)
                .ToListAsync();
        }

        // Получить уникальные Status для фильтров
        public async Task<List<string>> GetAvailableStatusesAsync()
        {
            using var context = new AppDbContext();
            return await context.EgoGifts
                .Select(g => g.Status)
                .Distinct()
                .OrderBy(s => s)
                .ToListAsync();
        }
    }
}