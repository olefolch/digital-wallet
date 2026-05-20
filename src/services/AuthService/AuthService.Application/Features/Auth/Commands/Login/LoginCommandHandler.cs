using AuthService.Application.Common;
using AuthService.Domain.Exceptions;
using AuthService.Domain.Interfaces;
using MediatR;

namespace AuthService.Application.Features.Auth.Commands.Login;

public class LoginCommandHandler(IUserRepository userRepository, IPasswordHasher passwordHasher, IJwtTokenGenerator jwtTokenGenerator) : IRequestHandler<LoginCommand, string>
{
    public async Task<string> Handle(LoginCommand request, CancellationToken cancellationToken)
    {
        var user = await userRepository.GetByEmailAsync(request.Email);

        if (user is null)
            throw new DomainException("Email ou senha inválidos");

        if (!passwordHasher.Verify(request.Password, user.PasswordHash))
            throw new DomainException("Email ou senha inválidos");
        
        return jwtTokenGenerator.GenerateToken(user);
    }   
}