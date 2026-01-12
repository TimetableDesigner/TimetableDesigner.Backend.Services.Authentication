using TimetableDesigner.Backend.Services.Authentication.Database;

namespace TimetableDesigner.Backend.Services.Authentication.Application.Commands.Register;

public class RegisterHandler
{
    private readonly DatabaseContext _databaseContext;
    
    public RegisterHandler(DatabaseContext databaseContext)
    {
        _databaseContext = databaseContext;
    }

    public async Task<RegisterDto> HandleAsync(RegisterCommand command)
    {
        return null;
    }
}