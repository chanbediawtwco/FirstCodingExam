using AutoMapper;
using FirstCodingExam.Data;
using FirstCodingExam.Dto;
using FirstCodingExam.Models;
using FirstCodingExam.Services.Interface;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Runtime.Intrinsics.Arm;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

namespace FirstCodingExam.Services
{
    public class AccountService: IAccountService
    {
        private readonly IMapper _mapper;
        public AccountService(IMapper mapper)
        {
            _mapper = mapper;
        }
        public bool IsValidUserInformation(Login UserLogin)
            => UserLogin != null
                && UserLogin.Email != string.Empty
                && UserLogin.Password != string.Empty
                && UserLogin.Email != null
                && UserLogin.Password != null;

        public bool IsValidUserInformation(UserRegistration User)
        {
            var LoginInformation = new Login {
                Email = User.Email, 
                Password = User.Password
            };

            return
                User != null
                && IsValidUserInformation(LoginInformation)
                && User.Firstname != string.Empty
                && User.Lastname != string.Empty
                && User.Firstname != null
                && User.Lastname != null;
        }

        // Change to check string for better reusability rather than overloading
        public User? GetUserProfile(string Email, string Password, FirstCodingExamDbContext _context)
            => _context.Users
                .Where(User => User.Email == Email && User.Password == Sha256(Password))
                .FirstOrDefault();

        // Hash password
        private string Sha256(string Password)
        {
            var SHA = SHA256.Create();
            var PasswordSalt = $"{Password}";
            var Bytes = SHA.ComputeHash(Encoding.UTF8.GetBytes(PasswordSalt));
            var BytesHash = SHA.ComputeHash(Bytes);
            var Hash = Convert.ToBase64String(BytesHash);
            return Hash;
        }

        // Save the user to database with hashed password
        public void SaveUserToDatabase(UserRegistration User, FirstCodingExamDbContext _context)
        {
            User NewUser = _mapper.Map<User>(User);
            NewUser.Password = Sha256(User.Password);
            _context.Users.Add(NewUser);
            _context.SaveChanges();
            _context.ChangeTracker.DetectChanges();
        }
    }
}
