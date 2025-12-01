using EGO_Library.Models;
using System;

namespace EGO_Library.Services
{
    public interface IAuthService
    {
        // Методы авторизации
        bool Login(string username, string password);
        bool Register(string username, string password, string email);
        //void Logout();

        // Свойства
        bool IsAuthenticated { get; }
        User CurrentUser { get; }
        bool UserExists(string username);

        // События
        event EventHandler AuthenticationChanged;
    }
}