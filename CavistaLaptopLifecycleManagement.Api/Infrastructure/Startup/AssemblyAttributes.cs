
using CavistaLaptopLifecycleManagement.Api.Infrastructure.Logging;
using Immediate.Handlers.Shared;
using Immediate.Validations.Shared;

[assembly: Behaviors(
        typeof(LoggingBehavior<,>),
        typeof(ValidationBehavior<,>)
    )]