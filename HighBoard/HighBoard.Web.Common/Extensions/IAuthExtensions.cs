namespace HighBoard.Web.Common.Extensions;

public interface IAuthExtensions
{
    string GenerateJwt(User user);
}