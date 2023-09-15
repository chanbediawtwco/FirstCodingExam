using FirstCodingExam.Controllers;
using FirstCodingExam.Data;
using FirstCodingExam.Dto;
using FirstCodingExam.Models;
using FirstCodingExam.Services.Interface;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Primitives;
using Microsoft.IdentityModel.Tokens;
using Moq;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace FirstCodingExam.Tests
{
    public class RecordControllerTests
    {
        [Fact]
        public async Task GetRecords_WithValidUser_ReturnsOkResult()
        {
            // Arrange
            var userId = 1; // The user ID to be used in the test
            var email = "christhian.bedia@wtwco.com";
            User UserLogin = new User
            {
                Id = userId,
                Email = email
            };
            var jwtToken = CreateJwtToken(UserLogin); // Create a JWT token with the user's ID

            var jwtServiceMock = new Mock<IJwtService>();
            jwtServiceMock.Setup(service => service.GetUserIdFromToken()).Returns(userId);

            var records = new List<Models.Record>
            {
                new Models.Record { UserId = userId },
                new Models.Record { UserId = userId }
            };

            var recordServiceMock = new Mock<IRecordService>();
            var dbContextMock = new Mock<FirstCodingExamDbContext>();

            var controller = new RecordController(jwtServiceMock.Object, dbContextMock.Object, recordServiceMock.Object);

            // Set up authentication
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, UserLogin.Id.ToString()),
                new Claim(ClaimTypes.Email, UserLogin.Email)
            };

            var identity = new ClaimsIdentity(claims, JwtBearerDefaults.AuthenticationScheme);
            var claimsPrincipal = new ClaimsPrincipal(identity);

            var httpContext = new DefaultHttpContext
            {
                User = claimsPrincipal
            };

            httpContext.Request.Headers.Add("Authorization", new StringValues($"Bearer {jwtToken}"));

            controller.ControllerContext = new ControllerContext
            {
                HttpContext = httpContext
            };

            var IdFromToken = Convert.ToInt32(identity.Claims.First(x => x.Type == Constants.NameIdentifier).Value);
            // Set up the behavior of the record service mock
            recordServiceMock.Setup(service =>
                service.GetRecordsWithHistory(IdFromToken, dbContextMock.Object))
                .Returns((int userId, FirstCodingExamDbContext dbContext) =>
                {
                    return records
                        .Where(record => record.UserId == userId)
                        .ToList(); // Convert to a list if the return type is List<Models.Record>
                });

            // Act
            var result = controller.GetRecords();

            // Assert
            Assert.IsType<OkObjectResult>(result);
            var okResult = (OkObjectResult)result;
            Assert.IsType<List<Models.Record>>(okResult.Value);
            var recordList = (List<Models.Record>)okResult.Value;
            Assert.Equal(2, recordList.Count);
        }

        private string CreateJwtToken(User User)
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
        public async Task SaveRecord_WithValidRecord_ReturnsStatusCode201()
        {
            // Arrange
            var jwtServiceMock = new Mock<IJwtService>();
            jwtServiceMock.Setup(service => service.GetUserIdFromToken()).Returns(1);

            var recordServiceMock = new Mock<IRecordService>();
            recordServiceMock.Setup(service => service.IsValidRecordInput(It.IsAny<NewRecord>())).Returns(true);

            var dbContextMock = new Mock<FirstCodingExamDbContext>();

            var controller = new RecordController(jwtServiceMock.Object, dbContextMock.Object, recordServiceMock.Object);

            // Set up authentication
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, "1"),
                // Add any other necessary claims
            };

            var identity = new ClaimsIdentity(claims, JwtBearerDefaults.AuthenticationScheme);
            var claimsPrincipal = new ClaimsPrincipal(identity);
            var authProperties = new AuthenticationProperties();

            var httpContext = new DefaultHttpContext();
            httpContext.User = claimsPrincipal;
            httpContext.Request.Headers.Add("Authorization", new StringValues($"Bearer {CreateJwtToken()}"));

            controller.ControllerContext = new ControllerContext
            {
                HttpContext = httpContext
            };

            var newRecord = new NewRecord 
            { 
                UserId = 1,
                Amount = 1000,
                LowerBoundInterestRate = 10,
                UpperBoundInterestRate = 50,
                IncrementalRate = 20,
                MaturityYears = 4,
            };

            // Act
            var result = controller.SaveRecord(newRecord);

            // Assert
            Assert.IsType<StatusCodeResult>(result);
            var statusCodeResult = (StatusCodeResult)result;
            Assert.Equal(StatusCodes.Status201Created, statusCodeResult.StatusCode);
        }

        private string CreateJwtToken()
        {
            // Implement your JWT token creation logic here for testing purposes
            // You can create a valid JWT token with the required claims for authentication
            // Return the JWT token as a string
            return "your-jwt-token";
        }

        // Similar tests for UpdateRecord and DeleteRecord can be added as needed.
    }
}