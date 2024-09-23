using WebApi.EndpointHandlers;

namespace WebApi.Extensions
{
    public static class EndpointRouteBuilderExtensions
    {
        public static void RegisterPlatesEndpoints(this IEndpointRouteBuilder endpointRouteBuilder)
        {
            var platesEndpoints = endpointRouteBuilder.MapGroup("/plates")/*.RequireAuthorization()*/;
            var plateWithGuidIdEndpoints = platesEndpoints.MapGroup("/{plateId:guid}");

            platesEndpoints.MapGet("", PlatesHandlers.GetPlatesAsync);

            plateWithGuidIdEndpoints.MapGet("", PlatesHandlers.GetPlateByIdAsync).WithName("GetPlate");

            platesEndpoints.MapGet("/{plateName}", PlatesHandlers.GetPlateByLicenseTextAsync)/*.AllowAnonymous()*/;

            platesEndpoints.MapPost("", PlatesHandlers.CreatePlateAsync);

            //plateWithGuidIdEndpoints.MapPut("", PlatesHandlers.UpdatePlateAsync)/*.RequireAuthorization()*/;

            endpointRouteBuilder.MapPut("/stopWork/{plateId:guid}", PlatesHandlers.UpdatePlateToNotActiveAsync)/*.RequireAuthorization()*/;

            plateWithGuidIdEndpoints.MapDelete("", PlatesHandlers.DeletePlateAsync);

            platesEndpoints.MapGet("/export", PlatesHandlers.GetExportedPlatesFullAsync);
        }
    }
}