using MediatR;
using TimetableDesigner.Backend.Services.Authentication.Database;

namespace TimetableDesigner.Backend.Services.Authentication.Application.Commands.Register;

public class RegisterHandler : IRequestHandler<RegisterCommand>
{
    private readonly DatabaseContext _databaseContext;
    
    public RegisterHandler(DatabaseContext databaseContext)
    {
        _databaseContext = databaseContext;
    }

    public async Task Handle(RegisterCommand command, CancellationToken cancellationToken)
    {
        
    }
}