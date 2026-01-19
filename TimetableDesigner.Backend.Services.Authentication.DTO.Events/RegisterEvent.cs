namespace TimetableDesigner.Backend.Services.Authentication.DTO.Events;

public record RegisterEvent(
    long Id,
    string Email
);