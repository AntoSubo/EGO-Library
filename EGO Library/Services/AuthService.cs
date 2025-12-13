using EGO_Library.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Windows;
using EGO_Library.Data;

namespace EGO_Library.Services
{
    public class AuthService : IAuthService
    {
        private bool _isAuthenticated;
        private User _currentUser;

        public bool IsAuthenticated
        {
            get => _isAuthenticated;
            private set
            {
                _isAuthenticated = value;
                AuthenticationChanged?.Invoke(this, EventArgs.Empty);
            }
        }

        public User CurrentUser => _currentUser;

        public event EventHandler AuthenticationChanged;

        public bool Login(string username, string password)
        {
            try
            {
                using var context = new AppDbContext();
                var user = context.Users.FirstOrDefault(u => u.Username == username);

                if (user != null && user.Password == password)
                {
                    _currentUser = user;
                    user.LastLogin = DateTime.Now;
                    context.SaveChanges();

                    IsAuthenticated = true;
                    return true;
                }
                return false;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка при входе: {ex.Message}", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                return false;
            }
        }

        public bool Register(string username, string password, string email)
        {
            try
            {
                using var context = new AppDbContext();

                if (context.Users.Any(u => u.Username == username))
                {
                    MessageBox.Show("Пользователь с таким именем уже существует", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                    return false;
                }

                var user = new User
                {
                    Username = username,
                    Password = password, 
                    Email = email,
                    CreatedDate = DateTime.Now,
                    LastLogin = DateTime.Now
                };

                context.Users.Add(user);
                context.SaveChanges();

                _currentUser = user;
                IsAuthenticated = true;
                return true;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка при регистрации: {ex.Message}", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                return false;
            }
        }

        public bool UserExists(string username)
        {
            try
            {
                using var context = new AppDbContext();
                return context.Users.Any(u => u.Username == username);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка при проверке пользователя: {ex.Message}", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                return false;
            }
        }
    }
}