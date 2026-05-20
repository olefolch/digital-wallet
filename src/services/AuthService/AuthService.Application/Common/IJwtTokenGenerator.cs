using AuthService.Domain.Entities;

namespace AuthService.Application.Common;

public interface IJwtTokenGenerator
{
    string GenerateToken(User user);
}