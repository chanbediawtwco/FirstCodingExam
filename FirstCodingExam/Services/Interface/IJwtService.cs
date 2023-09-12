using FirstCodingExam.Data;
using FirstCodingExam.Models;

namespace FirstCodingExam.Services.Interface
{
    public interface IJwtService
    {
        public Task<string> GenerateToken(User User, FirstCodingExamDbContext _context);
        public int GetUserIdFromToken();
    }
}
