using System.Security.Claims;

namespace PostItDemo;

public static class Utils
{
    public static bool UserHasHandle(ClaimsPrincipal user)
    {
        return user.Claims.Where(c => c.Type == "Handle").FirstOrDefault() is not null;
    }
    public static string GetUserHandle(ClaimsPrincipal user)
    {
        return user.Claims.Where(c => c.Type == "Handle").FirstOrDefault().Value;
    }
}
