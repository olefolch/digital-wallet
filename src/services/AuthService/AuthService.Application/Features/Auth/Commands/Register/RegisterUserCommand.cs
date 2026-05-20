using MediatR;

namespace AuthService.Application.Features.Auth.Commands.Register;

public record RegisterUserCommand(string Name, string Email, string Password) : IRequest<Guid>;