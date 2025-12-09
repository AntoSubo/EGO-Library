using EGO_Library.Models;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using EGO_Library.Data;
using System;

namespace EGO_Library.Services
{
    public class DataService
    {
        public async Task<List<EgoGift>> GetGiftsAsync(string searchText = null, int? tier = null, string status = null)
        {
            using var context = new AppDbContext();
            var query = context.EgoGifts.Include(g => g.Sources).AsQueryable();

            if (!string.IsNullOrWhiteSpace(searchText))
            {
                var searchLower = searchText.Trim().ToLowerInvariant();

                query = query.Where(g =>
                    EF.Functions.Like(g.Name, $"%{searchText}%") ||
                    EF.Functions.Like(g.Description, $"%{searchText}%") ||
                    EF.Functions.Like(g.Status, $"%{searchText}%") ||
                    EF.Functions.Like(g.Keywords, $"%{searchText}%") ||
                    EF.Functions.Like(g.Effect, $"%{searchText}%") ||
                    EF.Functions.Like(g.Acquisition, $"%{searchText}%")
                );
            }

            if (tier.HasValue)
            {
                query = query.Where(g => g.Tier == tier.Value);
            }

            if (!string.IsNullOrWhiteSpace(status) && status != "All")
            {
                query = query.Where(g => g.Status == status);
            }

            return await query.OrderBy(g => g.Tier).ThenBy(g => g.Name).ToListAsync();
        }

        // Получить все рецепты
        public async Task<List<Recipes>> GetAllRecipesAsync()
        {
            try
            {
                using var context = new AppDbContext();

                var recipes = await context.Recipes
                    .Include(r => r.ResultGift)
                    .Include(r => r.RequiredGifts)
                    .ToListAsync();

                Debug.WriteLine($"DataService: Загружено {recipes.Count} рецептов");

                return recipes;
            }
            catch (Exception ex)
            {
                return new List<Recipes>();
            }
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