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

    public static bool HandleIsIllegal(string handle)
    {
        //Here would be where you load an external library or list of taboo words
        return handle == "Anon";
    }
}
