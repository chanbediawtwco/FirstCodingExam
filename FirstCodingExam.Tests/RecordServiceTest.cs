using System;
using Xunit;
using Moq;
using AutoMapper;
using FirstCodingExam.Dto;
using FirstCodingExam.Models;
using FirstCodingExam.Services;
using FirstCodingExam.Services.Interface;

public class RecordServiceTests
{
    [Fact]
    public void IsValidRecordInput_ValidInput_ReturnsTrue()
    {
        // Arrange
        var recordService = new RecordService(null); // Mocked IMapper not needed for this test
        var newRecord = new NewRecord
        {
            Amount = 100,
            LowerBoundInterestRate = 5,
            UpperBoundInterestRate = 10,
            IncrementalRate = 1,
            MaturityYears = 5
        };

        // Act
        bool isValid = recordService.IsValidRecordInput(newRecord);

        // Assert
        Assert.True(isValid);
    }

    [Fact]
    public void IsValidRecordInput_NullInput_ReturnsFalse()
    {
        // Arrange
        var recordService = new RecordService(null); // Mocked IMapper not needed for this test
        NewRecord newRecord = null;

        // Act
        bool isValid = recordService.IsValidRecordInput(newRecord);

        // Assert
        Assert.False(isValid);
    }

    [Fact]
    public void HasChanges_RecordPropertiesChanged_ReturnsTrue()
    {
        // Arrange
        var recordService = new RecordService(null); // Mocked IMapper not needed for this test
        var dbRecord = new FirstCodingExam.Models.Record
        {
            Amount = 100,
            LowerBoundInterestRate = 5,
            UpperBoundInterestRate = 10,
            IncrementalRate = 1,
            MaturityYears = 5
        };
        var updatedRecord = new NewRecord
        {
            Amount = 200,
            LowerBoundInterestRate = 6,
            UpperBoundInterestRate = 11,
            IncrementalRate = 2,
            MaturityYears = 6
        };

        // Act
        bool hasChanges = recordService.HasChanges(updatedRecord, dbRecord);

        // Assert
        Assert.True(hasChanges);
    }

    [Fact]
    public void HasChanges_RecordPropertiesNotChanged_ReturnsFalse()
    {
        // Arrange
        var recordService = new RecordService(null); // Mocked IMapper not needed for this test
        var dbRecord = new FirstCodingExam.Models.Record
        {
            Amount = 100,
            LowerBoundInterestRate = 5,
            UpperBoundInterestRate = 10,
            IncrementalRate = 1,
            MaturityYears = 5
        };
        var updatedRecord = new NewRecord
        {
            Amount = 100,
            LowerBoundInterestRate = 5,
            UpperBoundInterestRate = 10,
            IncrementalRate = 1,
            MaturityYears = 5
        };

        // Act
        bool hasChanges = recordService.HasChanges(updatedRecord, dbRecord);

        // Assert
        Assert.False(hasChanges);
    }
}