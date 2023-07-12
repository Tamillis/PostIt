using Microsoft.AspNetCore.Cryptography.KeyDerivation;
using System.Security.Claims;

namespace PostItDemo;

public static class Utils
{
    public static bool UserHasHandle(ClaimsPrincipal? user)
    {
        return user != null && user.Claims.Where(c => c.Type == "Handle").FirstOrDefault() is not null;
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

    public static string HashPasswd(string passwd)
    {
        //bit of a hack but hey
        byte[] salt = { 1, 2, 3, 4, 5, 6, 7, 8, 9, 0, 1, 2, 3, 4, 5, 6 };

        return Convert.ToBase64String(KeyDerivation.Pbkdf2(
            password: passwd,
            salt: salt,
            prf: KeyDerivationPrf.HMACSHA256,
            iterationCount: 100_000,
            numBytesRequested: 256 / 8
        ));
    }
}
