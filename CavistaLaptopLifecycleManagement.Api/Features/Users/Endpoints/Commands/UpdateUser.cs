using CavistaLaptopLifecycleManagement.Api.Database;
using CavistaLaptopLifecycleManagement.Api.Features.Shared.Services;
using CavistaLaptopLifecycleManagement.Api.Features.Users.Models;
using CavistaLaptopLifecycleManagement.Api.Features.Users.Services;
using Immediate.Apis.Shared;
using Immediate.Handlers.Shared;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Serilog;

namespace CavistaLaptopLifecycleManagement.Api.Features.Users.Endpoints.Commands
{
    [Handler]
    [MapPut("{userId}")]
    [MapGroup<UserMapGroup>]
    [Authorize(Policy = Policies.ITRolePolicy)]
    public static partial class UpdateUser
    {
        public sealed record Body
        {
            public required Role Role { get; set; }
        }

        public sealed partial record UpdateUserBody
        {
            [FromRoute]
            public required Guid userId { get; set; }

            [FromBody]
            public required Body Body { get; set; }
        }

        public sealed record UpdateUserResponse
        {
            public Guid? userId { get; set; }

            public string Message { get; set; }

            public UpdateUserResponse(string message)
            {
                Message = message;
                userId = null;
            }

            public UpdateUserResponse(Guid id)
            {
                Message = "successful";
                userId = id;
            }
        }

        private async static ValueTask<Results<Ok<UpdateUserResponse>, BadRequest<UpdateUserResponse>, UnauthorizedHttpResult>> HandleAsync(
            UpdateUserBody command,
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

            var existingUser = await userService.GetUserAsync(command.userId, context);

            if (existingUser == null)
            {
                return TypedResults.BadRequest(new UpdateUserResponse("No user found"));
            }

            if (existingUser.Role == command.Body.Role) return TypedResults.BadRequest(new UpdateUserResponse("User already has this role"));

            var requestBody = command.Body;

            existingUser.Role = requestBody.Role;
            existingUser.Modified = DateTime.UtcNow.ToUniversalTime();

            await auditTrailService.AddAuditTrailAsync(context, user.Id, AuditTrailService.AuditAction.Update, AuditTrailService.AuditOn.User, existingUser.Id);

            try
            {
                if (await context.SaveChangesAsync() > 0)
                {
                    return TypedResults.Ok(new UpdateUserResponse(existingUser.Id));
                }
            }
            catch (Exception ex)
            {
                Log.Error($"An error occurred => {ex.Message}");
            }

            return TypedResults.BadRequest(new UpdateUserResponse("An error occurred"));
        }
    }
}
