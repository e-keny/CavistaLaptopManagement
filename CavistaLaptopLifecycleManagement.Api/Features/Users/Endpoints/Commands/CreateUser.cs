using CavistaLaptopLifecycleManagement.Api.Database;
using CavistaLaptopLifecycleManagement.Api.Features.Laptop.Services;
using CavistaLaptopLifecycleManagement.Api.Features.Shared.Services;
using CavistaLaptopLifecycleManagement.Api.Features.Users.Models;
using CavistaLaptopLifecycleManagement.Api.Features.Users.Services;
using Immediate.Apis.Shared;
using Immediate.Handlers.Shared;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Serilog;
using System.Text.Json;

namespace CavistaLaptopLifecycleManagement.Api.Features.Users.Endpoints.Commands
{
    [Handler]
    [MapPost("")]
    [MapGroup<UserMapGroup>]
    public static partial class CreateUser
    {
        public sealed record CreateUserBody
        {
            public required string Email { get; init; }

            public required string FirstName { get; init; }

            public required string LastName { get; init; }

            public string? MiddleName { get; init; }

            public  Role Role { get; init; }
        }

        public sealed record Command
        {
            [FromBody]
            public required CreateUserBody Body { get; init; }
        }

        public sealed record CreateUserResponse
        {
            public Guid? userId { get; init; }

            public string Message { get; init; }

            public CreateUserResponse(string message)
            {
                Message = message;
                userId = null;
            }

            public CreateUserResponse(Guid id)
            {
                Message = "successful";
                userId = id;
            }
        }

        private async static ValueTask<Results<Ok<CreateUserResponse>, BadRequest<CreateUserResponse>, UnauthorizedHttpResult>> HandleAsync(
            Command command,
            AuditTrailService auditTrailService,
            CLMDbContext context,
            UserService userService,
            CancellationToken token)
        {
            var user = await userService.GetCurrentUserAsync();

            if (user == null)
            {
                return TypedResults.Unauthorized(); ;
            }

            var requestBody = command.Body;

            var userToAdd = new Database.Entities.User
            {
                EmailAddress = requestBody.Email,
                FirstName = requestBody.FirstName,
                LastName = requestBody.LastName,
                MiddleName = requestBody.MiddleName,
                Role = requestBody.Role,                
                Created_At = DateTime.UtcNow.ToUniversalTime(),
                Modified = DateTime.UtcNow.ToUniversalTime()
            };

            context.Users.Add(userToAdd);

            try
            {
                if (await context.SaveChangesAsync() > 0)
                {
                    await auditTrailService.AddAuditTrailAsync(user.Id, AuditTrailService.AuditAction.Create, AuditTrailService.AuditOn.User, userToAdd.Id);

                    return TypedResults.Ok(new CreateUserResponse(userToAdd.Id));
                }
            }
            catch (Exception ex)
            {
                Log.Error($"An error occurred => {ex.Message}");
            }

            return TypedResults.BadRequest(new CreateUserResponse("An error occurred"));
        }
    }
}
