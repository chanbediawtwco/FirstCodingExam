using AutoMapper;
using FirstCodingExam.Data;
using FirstCodingExam.Dto;
using FirstCodingExam.Models;
using FirstCodingExam.Services;
using Microsoft.EntityFrameworkCore;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using Xunit;

public class AccountServiceTests
{
    [Fact]
    public void IsValidUserInformation_ValidLogin_ReturnsTrue()
    {
        // Arrange
        var accountService = new AccountService(null); // Mapper is not needed for this test
        var login = new Login { Email = "christhian.bedia@wtwco.com", Password = "christhianBedia" };

        // Act
        bool result = accountService.IsValidUserInformation(login);

        // Assert
        Assert.True(result);
    }

    [Fact]
    public void IsValidUserInformation_InvalidLogin_ReturnsFalse()
    {
        // Arrange
        var accountService = new AccountService(null); // Mapper is not needed for this test
        var login = new Login { Email = null, Password = "password123" };

        // Act
        bool result = accountService.IsValidUserInformation(login);

        // Assert
        Assert.False(result);
    }

    [Fact]
    public void IsValidUserInformation_ValidUserRegistration_ReturnsTrue()
    {
        // Arrange
        var accountService = new AccountService(null); // Mapper is not needed for this test
        var userRegistration = new UserRegistration { Email = "test@example.com", Password = "password123", Firstname = "John", Lastname = "Doe" };

        // Act
        bool result = accountService.IsValidUserInformation(userRegistration);

        // Assert
        Assert.True(result);
    }

    [Fact]
    public void IsValidUserInformation_InvalidUserRegistration_ReturnsFalse()
    {
        // Arrange
        var accountService = new AccountService(null); // Mapper is not needed for this test
        var userRegistration = new UserRegistration { Email = null, Password = "password123", Firstname = null, Lastname = "Doe" };

        // Act
        bool result = accountService.IsValidUserInformation(userRegistration);

        // Assert
        Assert.False(result);
    }

    [Fact]
    public void GetUserProfile_InvalidUser_ReturnsNull()
    {
        // Arrange
        var email = "test@example.com";
        var password = "password123";

        var users = new List<User>().AsQueryable();
        var dbContextMock = new Mock<FirstCodingExamDbContext>();

        // Mocking DbSet<User> using IQueryable
        var dbSetMock = new Mock<DbSet<User>>();
        dbSetMock.As<IQueryable<User>>().Setup(m => m.Provider).Returns(users.Provider);
        dbSetMock.As<IQueryable<User>>().Setup(m => m.Expression).Returns(users.Expression);
        dbSetMock.As<IQueryable<User>>().Setup(m => m.ElementType).Returns(users.ElementType);
        dbSetMock.As<IQueryable<User>>().Setup(m => m.GetEnumerator()).Returns(() => users.GetEnumerator());

        dbContextMock.Setup(c => c.Users).Returns(dbSetMock.Object);

        var accountService = new AccountService(null);

        // Act
        var result = accountService.GetUserProfile(email, password, dbContextMock.Object);

        // Assert
        Assert.Null(result);
    }
}