using FirstCodingExam.Data;
using FirstCodingExam.Dto;
using FirstCodingExam.Models;
using FirstCodingExam.Services.Interface;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace FirstCodingExam.Controllers
{
    public class AccountController : Controller
    {
        private readonly IAccountService _accountService;
        private readonly FirstCodingExamDbContext _context;
        private readonly IJwtService _jwtService;

        public AccountController(IAccountService accountService, 
            FirstCodingExamDbContext context,
            IJwtService jwtService)
        {
            _accountService = accountService;
            _context = context;
            _jwtService = jwtService;

        }
        // POST: AccountController/Login
        [HttpPost]
        [Route("account/login")]
        //[ValidateAntiForgeryToken]
        public async Task<IActionResult> Login([FromBody] Login UserLogin)
        {
            if (!_accountService.IsValidUserInformation(UserLogin))
            {
                return BadRequest();
            }

            var DbUser = _accountService.GetUserProfile(UserLogin.Email, UserLogin.Password, _context);
            if (DbUser != null)
            {
                return Ok(await _jwtService.GenerateToken(DbUser, _context));
            }

            return NotFound();
        }

        [HttpPost]
        [Route("account/register")]
        public IActionResult Registration([FromBody] UserRegistration User)
        {
            if (!_accountService.IsValidUserInformation(User))
            {
                return BadRequest();
            }

            var IsUserExist = _accountService.GetUserProfile(User.Email, User.Password, _context) != null;
            if (!IsUserExist)
            {
                _accountService.SaveUserToDatabase(User, _context);
                // Send response 201 = Created response
                return StatusCode(201);
            }

            return Conflict();
        }
    }
}
