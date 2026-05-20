using AuthService.Application.Common;
using AuthService.Domain.Entities;
using AuthService.Domain.Exceptions;
using AuthService.Domain.Interfaces;
using MediatR;

namespace AuthService.Application.Features.Auth.Commands.Register;

public class RegisterUserCommandHandler(IUserRepository userRepository, IPasswordHasher passwordHasher) : IRequestHandler<RegisterUserCommand, Guid>
{
    public async Task<Guid> Handle(RegisterUserCommand request, CancellationToken cancellationToken)
    {
        var user = await userRepository.GetByEmailAsync(request.Email);

        if (user is not null)
            throw new DomainException("Já existe um usuário com este email cadastrado");

        string passwordHash = passwordHasher.Hash(request.Password);

        var createdUser = User.Create(request.Name, request.Email, passwordHash);
        await userRepository.AddAsync(createdUser);

        return createdUser.Id;
    }
}