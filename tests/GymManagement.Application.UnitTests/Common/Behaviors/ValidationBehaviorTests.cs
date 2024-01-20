using CSharpFunctionalExtensions;
using FluentAssertions;
using FluentValidation;
using FluentValidation.Results;
using GymManagement.Application.Common.Behaviors;
using GymManagement.Application.Gyms.Commands.CreateGym;
using GymManagement.Core.ErrorHandling;
using GymManagement.Domain.Gyms;
using MediatR;
using NSubstitute;
using TestCommon.Gyms;

namespace GymManagement.Application.UnitTests.Common.Behaviors;

public class ValidationBehaviorTests
{
    private readonly RequestHandlerDelegate<Result<Gym, OperationError>> _mockNextBehavior;
    private readonly IValidator<CreateGymCommand> _mockValidator;
    private readonly ValidationBehavior<CreateGymCommand,Result<Gym, OperationError>> _sut;

    public ValidationBehaviorTests()
    {
        _mockNextBehavior = Substitute.For<RequestHandlerDelegate<Result<Gym, OperationError>>>();
        _mockValidator = Substitute.For<IValidator<CreateGymCommand>>();
        
        _sut = new ValidationBehavior<CreateGymCommand, Result<Gym, OperationError>>(_mockValidator);
    }
    
    [Fact]
    public async Task InvokeBehavior_WhenValidatorIsValid_ShouldInvokeNextBehavior()
    {
        // Arrange
        var gym = GymFactory.CreateGym();
        _mockNextBehavior.Invoke().Returns(Result.Success<Gym, OperationError>(gym));
        
        var createGymCommand = GymCommandFactory.CreateCreateGymCommand();
        _mockValidator
            .ValidateAsync(createGymCommand, Arg.Any<CancellationToken>())
            .Returns(new FluentValidation.Results.ValidationResult());
        
        // Act
        var result = await _sut.Handle(createGymCommand, _mockNextBehavior, CancellationToken.None);

        // Assert
        result.IsSuccess.Should().BeTrue();
        result.Value.Should().BeEquivalentTo(gym);
    }
    
    [Fact]
    public async Task InvokeBehavior_WhenValidatorIsInvalid_ShouldReturnInvalidResult()
    {
        // Arrange
        var createGymCommand = GymCommandFactory.CreateCreateGymCommand();
        _mockValidator
            .ValidateAsync(createGymCommand, Arg.Any<CancellationToken>())
            .Returns(new ValidationResult([new ValidationFailure("Name", "Name is required.")]));
        
        // Act
        var result = await _sut.Handle(createGymCommand, _mockNextBehavior, CancellationToken.None);

        // Assert
        result.IsSuccess.Should().BeFalse();
        result.Error.ValidationErrors.Should().HaveCount(1);
        result.Error.ValidationErrors.First().Identifier.Should().Be("Name");
        result.Error.ValidationErrors.First().Message.Should().Be("Name is required.");
    }
}