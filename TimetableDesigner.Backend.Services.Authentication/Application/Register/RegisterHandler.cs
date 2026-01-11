using TimetableDesigner.Backend.Services.Authentication.Database;
using TimetableDesigner.Backend.Services.Authentication.DTO;

namespace TimetableDesigner.Backend.Services.Authentication.Application.Register;

public class RegisterHandler
{
    private readonly DatabaseContext _databaseContext;
    
    public RegisterHandler(DatabaseContext databaseContext)
    {
        _databaseContext = databaseContext;
    }

    public async Task<RegisterResponse> HandleAsync(RegisterCommand command)
    {
        return null;
    }
}