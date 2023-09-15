using FirstCodingExam.Controllers;
using FirstCodingExam.Data;
using FirstCodingExam.Dto;
using FirstCodingExam.Models;
using FirstCodingExam.Services;
using FirstCodingExam.Services.Interface;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Moq;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace FirstCodingExam.Tests
{
    public class AccountControllerTests
    {
        private readonly Mock<IAccountService> _accountServiceMock;
        private readonly Mock<IJwtService> _jwtServiceMock;
        private readonly FirstCodingExamDbContext _dbContext;
        private readonly AccountController _controller;

        public AccountControllerTests()
        {
            _accountServiceMock = new Mock<IAccountService>();
            _jwtServiceMock = new Mock<IJwtService>();

            // Assign the mock of _dbContext to the class-level _dbContext field
            _dbContext = Mock.Of<FirstCodingExamDbContext>();

            _controller = new AccountController(_accountServiceMock.Object, _dbContext, _jwtServiceMock.Object);
        }

        private string GenerateTokenDynamically(User User)
        {
            // Generate Keys
            var SecurityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("qweasdD8NPqwferhergweukSr2sdfgerhqwdRPBclyG2sfqafqHsTsfgh8NKJGJOhqwad4Oc5KQIwqeHDJOAkpndojkfsrsXlfqwdXp"));
            // Add Credentials Using Generated Keys
            var Credentials = new SigningCredentials(SecurityKey, SecurityAlgorithms.HmacSha256);

            // Create User Claims
            var Claims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier,User.Id.ToString()),
                new Claim(ClaimTypes.Email, User.Email)
            };

            // Generate Token
            var token = new JwtSecurityToken(
                "https://localhost:7134",
                "https://localhost:7134",
                Claims,
                expires: DateTime.Now.AddHours(1),
                signingCredentials: Credentials);

            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        [Fact]
        public async Task Login_WithInvalidUser_ReturnsBadRequestResult()
        {
            // Arrange
            var userLogin = new Login
            {
                Email = "invalid@example.com",
                Password = "invalidpassword"
            };

            _accountServiceMock.Setup(service => service.IsValidUserInformation(userLogin)).Returns(false);

            // Act
            var result = await _controller.Login(userLogin);

            // Assert
            Assert.IsType<BadRequestResult>(result);
        }

        [Fact]
        public async Task Login_WithNonExistentUser_ReturnsNotFoundResult()
        {
            // Arrange
            var userLogin = new Login
            {
                Email = "nonexistent@example.com",
                Password = "nonexistentpassword"
            };

            _accountServiceMock.Setup(service => service.IsValidUserInformation(userLogin)).Returns(true);
            _accountServiceMock.Setup(service => service.GetUserProfile(userLogin.Email, userLogin.Password, _dbContext)).Returns((User)null);

            // Act
            var result = await _controller.Login(userLogin);

            // Assert
            Assert.IsType<NotFoundResult>(result);
        }

        [Fact]
        public void Registration_WithValidUser_ReturnsStatusCode201()
        {
            // Arrange
            // Mock the dependencies: IAccountService, FirstCodingExamDbContext, and IJwtService
            var accountServiceMock = new Mock<IAccountService>();
            var dbContextMock = new Mock<FirstCodingExamDbContext>();
            var jwtServiceMock = new Mock<IJwtService>();

            // Create a valid UserRegistration object
            var validUser = new UserRegistration
            {
                Email = "test@example.com",
                Password = "password"
                // Add any other necessary properties
            };

            // Configure the account service to validate the user as valid
            accountServiceMock.Setup(service => service.IsValidUserInformation(validUser)).Returns(true);

            // Configure the database context to return null for GetUserProfile
            dbContextMock.Setup(context => context.Users).Returns(Mock.Of<DbSet<User>>());

            // Create an instance of the AccountController with the mocked dependencies
            var controller = new AccountController(accountServiceMock.Object, dbContextMock.Object, jwtServiceMock.Object);

            // Act
            var result = controller.Registration(validUser);

            // Assert
            // Expect a StatusCodeResult with status code 201 (Created)
            Assert.IsType<StatusCodeResult>(result);
            Assert.Equal(StatusCodes.Status201Created, (result as StatusCodeResult).StatusCode);
        }

        [Fact]
        public void Registration_WithInvalidUser_ReturnsBadRequest()
        {
            // Arrange
            // Mock the dependencies: IAccountService, FirstCodingExamDbContext, and IJwtService
            var accountServiceMock = new Mock<IAccountService>();
            var dbContextMock = new Mock<FirstCodingExamDbContext>();
            var jwtServiceMock = new Mock<IJwtService>();

            // Create an invalid UserRegistration object
            var invalidUser = new UserRegistration
            {
                Email = "",
                Password = ""
                // Add any other necessary properties
            };

            // Configure the account service to validate the user as invalid
            accountServiceMock.Setup(service => service.IsValidUserInformation(invalidUser)).Returns(false);

            // Create an instance of the AccountController with the mocked dependencies
            var controller = new AccountController(accountServiceMock.Object, dbContextMock.Object, jwtServiceMock.Object);

            // Act
            var result = controller.Registration(invalidUser);

            // Assert
            // Expect a BadRequestResult
            Assert.IsType<BadRequestResult>(result);
        }


    }
}