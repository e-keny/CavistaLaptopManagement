using CavistaLaptopLifecycleManagement.Api.Database;
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
    [MapPut("/{userID}")]
    [MapGroup<UserMapGroup>]
    public static partial class UpdateUser
    {
        public sealed record UpdateUserBody
        {
            public required string Email { get; init; }

            public required string FirstName { get; init; }

            public required string LastName { get; init; }

            public string? MiddleName { get; init; }

            public List<Role> Role { get; init; }
        }

        public sealed record Command
        {
            [FromRoute]
            public required Guid UserID { get; init; }

            [FromBody]
            public required UpdateUserBody Body { get; init; }
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
            var user = await userService.GetCurrentUser();

            if (user == null)
            {
                return TypedResults.Unauthorized(); ;
            }

            var existingUser = await userService.GetUser(command.UserID, context);

            if (existingUser == null)
            {
                return TypedResults.BadRequest(new CreateUserResponse("No user found"));
            }

            var requestBody = command.Body;

            existingUser.EmailAddress = requestBody.Email;
            existingUser.FirstName = requestBody.FirstName;
            existingUser.LastName = requestBody.LastName;
            existingUser.MiddleName = requestBody.MiddleName;
            existingUser.Roles = JsonSerializer.Serialize(requestBody.Role);
            existingUser.Modified = DateTime.UtcNow.ToUniversalTime();

            try
            {
                if (await context.SaveChangesAsync() > 0)
                {
                    await auditTrailService.AddAuditTrail(user.Id, AuditTrailService.AuditAction.Update, AuditTrailService.AuditOn.User, existingUser.Id);

                    return TypedResults.Ok(new CreateUserResponse(existingUser.Id));
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
