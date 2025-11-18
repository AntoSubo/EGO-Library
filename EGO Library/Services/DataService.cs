using EGO_Library.Data;
using EGO_Library.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;

namespace EGO_Library.Services
{
    public class DataService : IDisposable
    {
        private readonly AppDbContext _context;
        private bool _disposed = false;

        // Конструктор с контекстом БД
        public DataService(AppDbContext context)
        {
            _context = context;
        }

        // === СИНХРОННЫЕ МЕТОДЫ ДЛЯ ViewModel ===

        // Получить все дары
        public List<EgoGift> GetAllGifts()
        {
            return _context.EgoGifts
                .Include(g => g.Sources)
                .OrderBy(g => g.Tier)
                .ThenBy(g => g.Name)
                .ToList();
        }

        // Поиск по имени
        public List<EgoGift> SearchGifts(string searchText)
        {
            if (string.IsNullOrWhiteSpace(searchText))
                return GetAllGifts();

            return _context.EgoGifts
                .Include(g => g.Sources)
                .Where(g => g.Name.Contains(searchText) ||
                           g.Description.Contains(searchText) ||
                           g.Status.Contains(searchText))
                .ToList();
        }

        // Фильтрация по Tier и Status
        public List<EgoGift> GetFilteredGifts(int? tier = null, string status = null)
        {
            var query = _context.EgoGifts.Include(g => g.Sources).AsQueryable();

            if (tier.HasValue)
                query = query.Where(g => g.Tier == tier.Value);

            if (!string.IsNullOrEmpty(status))
                query = query.Where(g => g.Status == status);

            return query.OrderBy(g => g.Tier).ThenBy(g => g.Name).ToList();
        }

        // Получить дар по ID
        public EgoGift GetGiftById(int id)
        {
            return _context.EgoGifts
                .Include(g => g.Sources)
                .FirstOrDefault(g => g.Id == id);
        }

        // Добавить новый дар
        public void AddGift(EgoGift gift)
        {
            _context.EgoGifts.Add(gift);
            _context.SaveChanges();
        }

        // Обновить дар
        public void UpdateGift(EgoGift gift)
        {
            _context.EgoGifts.Update(gift);
            _context.SaveChanges();
        }

        // Удалить дар
        public void DeleteGift(int id)
        {
            var gift = GetGiftById(id);
            if (gift != null)
            {
                _context.EgoGifts.Remove(gift);
                _context.SaveChanges();
            }
        }

        // Проверить существует ли дар с таким именем
        public bool GiftExists(string name)
        {
            return _context.EgoGifts.Any(g => g.Name == name);
        }

        // === МЕТОДЫ С ПАГИНАЦИЕЙ ===

        // Получить дары с пагинацией
        public List<EgoGift> GetGiftsPaged(int page, int pageSize)
        {
            return _context.EgoGifts
                .Include(g => g.Sources)
                .OrderBy(g => g.Tier)
                .ThenBy(g => g.Name)
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToList();
        }

        // Поиск с пагинацией
        public List<EgoGift> SearchGiftsPaged(string searchText, int page, int pageSize)
        {
            var query = _context.EgoGifts
                .Include(g => g.Sources)
                .Where(g => g.Name.Contains(searchText) ||
                           g.Description.Contains(searchText) ||
                           g.Status.Contains(searchText));

            return query
                .OrderBy(g => g.Tier)
                .ThenBy(g => g.Name)
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToList();
        }

        // Получить общее количество даров
        public int GetTotalGiftsCount()
        {
            return _context.EgoGifts.Count();
        }

        // Получить уникальные статусы для фильтров
        public List<string> GetUniqueStatuses()
        {
            return _context.EgoGifts
                .Select(g => g.Status)
                .Distinct()
                .OrderBy(s => s)
                .ToList();
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