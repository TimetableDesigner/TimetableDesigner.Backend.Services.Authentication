using TimetableDesigner.Backend.Events;
using TimetableDesigner.Backend.Events.Extensions.AspNetCore.OpenApi;
using TimetableDesigner.Backend.Services.Authentication.DTO.WebAPI;

namespace TimetableDesigner.Backend.Services.Authentication.Events;

public static class Handlers
{
    public static void MapEventHandlers(this WebApplication app)
    {
        app.AddEventHandler<RegisterRequest>(Test);
    }

    public static async Task Test(RegisterRequest registerRequest)
    {
        Console.WriteLine(registerRequest.Email);
    }
}