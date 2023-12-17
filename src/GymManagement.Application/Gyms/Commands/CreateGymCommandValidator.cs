using System.Data;
using FluentValidation;

namespace GymManagement.Application.Gyms.Commands;

public class CreateGymCommandValidator : AbstractValidator<CreateGymCommand>
{
    public CreateGymCommandValidator()
    {
        RuleFor(x => x.Name)
            .MinimumLength(3)
            .MaximumLength(100);
    }
}