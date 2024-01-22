<img src="https://github.com/alexeipancratov/gym-clean-architecture/assets/3188163/6dc0c717-1b7e-46f8-a34f-12f3d41c30aa" width=350 />

# Gym Clean Architecture

Gym Clean Architecture is a sample project that demonstrates how to implement the Clean Architecture in a .NET application.

## Table of Contents

- [Tech stack](#tech-stack)
- [Installation](#installation)
- [Usage](#usage)
- [License](#license)

## Tech stack

- .NET 8
- EF Core 8

## Installation

To install the project, follow these steps:
```
git clone git@github.com:alexeipancratov/gym-clean-architecture.git
cd gym-clean-architecture
dotnet restore
```

## Usage

In order to run the project, follow these steps:
```
dotnet run --project src/GymManagement.Api
```

## Project structure

The project is structured according to the Clean Architecture principles as follows:

- `src/GymManagement.Api` - presentation layer, which contains the API controllers and is responsible for converting the requests to the application layer and the responses to the client
- `src/GymManagement.Application` - application layer, which contains the application logic and is responsible for coordinating the application, defining the interfaces of the application services, and orchestrating the domain layer using the MediatR library
- `src/GymManagement.Domain` - domain layer, which contains the domain logic and is responsible for defining the domain entities and the core business rules
- `src/GymManagement.Infrastructure` - infrastructure layer, which contains the infrastructure logic and is responsible for implementing the interfaces defined in the application layer, such as the repositories and the unit of work, using the Entity Framework Core library

## Validation

Validation comes in two flavors: model validation and domain validation. Generally, model validation is implemented in the presentation layer or right at the top of the application layer, while domain validation is implemented strictly in the domain layer.

Model validation is responsible for validating the input data, e.g., if username is between 1 and 50 characters long (as required by the database). Domain validation is responsible for validating the business rules, e.g., if the username is unique. Thefefore, acting as an additional layer of validation.

In this project model validation is implemented right at the top of the application layer using the FluentValidation library as a cross-cutting concern using the MediatR's pipeline behaviors (see `src/GymManagement.Application/Behaviors/ValidationBehavior.cs`). Validators are implemented at the command request level.

## Useful CLI commands

Execute the following commands from the root directory of the project:

`dotnet ef migrations add GymsAndSubscriptionGyms -p src/GymManagement.Infrastructure -s src/GymManagement.Api` -
creates a new migration

`dotnet ef database update -p src/GymManagement.Infrastructure -s src/GymManagement.Api` - updates the database

## Eventual consistency using domain events

This project relies on eventual consistency when handling complicated flows which involve updating data in multiple
places, e.g, `DeleteSubscriptionCommandHandler`. In this case, Admin's subscription gets assigned to null, and then
a domain event is being raised - `SubscriptionDeletedEvent`. Then corresponding event handlers handle deletion of
actual and related data.

In order to optimize the duration of request execution event handlers will be executed after the response is sent.
This is handled in the `EventualConsistencyMiddleware`. This middleware executes its logic inside a DB transaction
so that either all event handlers succeed or none of them do.

This approach is the opposite to "transactional consistency" where an "orchestrator" handles all the logic surrounding a business use-case,
thus making request processing faster. There're several benefits to this approach:
- high performance
- flexible error handling (side effects can be retried multiple times in the background without user knowing about it)
- scalability

## Authentication and authorization

For authentication we're using Authorized attribute with JWT bearer authentication scheme.
For authorization we're using MediatR behaviors, specifically `AuthorizationBehavior` which checks if the user is authorized to execute the command.

Currently it's being discussed whether it's a good idea to use MediatR for authorization, or if this should be handled in the Presentation layer.
MediatR is a good candidate for this, because authorization is business rule. So it makes sense to handle only authentication in the Presentation layer,

## License

None
