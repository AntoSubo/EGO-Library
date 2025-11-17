using EGO_Library.Data;
using EGO_Library.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EGO_Library.Services
{
    public class DataService : IDisposable
    {
        private readonly AppDbContext _context;
        private bool _disposed = false;    

        // Получить все дары
        public async Task<List<EgoGift>> GetAllGiftsAsync()
        {
            return await _context.EgoGifts
                .Include(g => g.Sources)
                .OrderBy(g => g.Tier)
                .ThenBy(g => g.Name)
                .ToListAsync();
        }

        // Поиск по имени
        public async Task<List<EgoGift>> SearchGiftsAsync(string searchText)
        {
            if (string.IsNullOrWhiteSpace(searchText))
                return await GetAllGiftsAsync();

            return await _context.EgoGifts
                .Include(g => g.Sources)
                .Where(g => g.Name.Contains(searchText) ||
                           g.Description.Contains(searchText) ||
                           g.Status.Contains(searchText))
                .ToListAsync();
        }

        // Фильтрация по Tier и Status
        public async Task<List<EgoGift>> GetFilteredGiftsAsync(int? tier = null, string status = null)
        {
            var query = _context.EgoGifts.Include(g => g.Sources).AsQueryable();

            if (tier.HasValue)
                query = query.Where(g => g.Tier == tier.Value);

            if (!string.IsNullOrEmpty(status))
                query = query.Where(g => g.Status == status);

            return await query.OrderBy(g => g.Tier).ThenBy(g => g.Name).ToListAsync();
        }

        // Получить дар по ID
        public async Task<EgoGift> GetGiftByIdAsync(int id)
        {
            return await _context.EgoGifts
                .Include(g => g.Sources)
                .FirstOrDefaultAsync(g => g.Id == id);
        }

        // Добавить новый дар
        public async Task AddGiftAsync(EgoGift gift)
        {
            await _context.EgoGifts.AddAsync(gift);
            await _context.SaveChangesAsync();
        }

        // Обновить дар
        public async Task UpdateGiftAsync(EgoGift gift)
        {
            _context.EgoGifts.Update(gift);
            await _context.SaveChangesAsync();
        }

        // Удалить дар
        public async Task DeleteGiftAsync(int id)
        {
            var gift = await GetGiftByIdAsync(id);
            if (gift != null)
            {
                _context.EgoGifts.Remove(gift);
                await _context.SaveChangesAsync();
            }
        }

        // Проверить существует ли дар с таким именем
        public async Task<bool> GiftExistsAsync(string name)
        {
            return await _context.EgoGifts.AnyAsync(g => g.Name == name);
        }

        public void Dispose()
        {
            if (!_disposed)
            {
                _context?.Dispose();
                _disposed = true;
            }
        }
    }
}