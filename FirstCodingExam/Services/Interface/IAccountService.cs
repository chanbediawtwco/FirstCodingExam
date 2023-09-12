using FirstCodingExam.Data;
using FirstCodingExam.Dto;
using FirstCodingExam.Models;

namespace FirstCodingExam.Services.Interface
{
    public interface IAccountService
    {
        public User? GetUserProfile(string Email, string Password, FirstCodingExamDbContext _context);
        public bool IsValidUserInformation(Login UserLogin);
        public bool IsValidUserInformation(UserRegistration User);
        public void SaveUserToDatabase(UserRegistration User, FirstCodingExamDbContext _context);
    }
}
