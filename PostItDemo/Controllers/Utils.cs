using Microsoft.AspNetCore.Cryptography.KeyDerivation;
using Microsoft.EntityFrameworkCore;
using PostItDemo.Models;
using System.Security.Claims;

namespace PostItDemo.Controllers;

public static class Utils
{
    public static bool UserHasHandle(ClaimsPrincipal? user)
    {
        return user != null &&
            user.Claims is not null &&
            user.Claims.Where(c => c.Type == "Handle").FirstOrDefault() is not null;
    }
    public static string GetUserHandle(ClaimsPrincipal user)
    {
        if (user.Claims is null) throw new Exception("No claims found on user");

        var handleClaim = user.Claims.Where(c => c.Type == "Handle").FirstOrDefault();

        if (handleClaim is null) throw new Exception("No Handle claim found on user");

        return handleClaim.Value;
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

    public static double GetPostValue(PostIt post)
    {
        //A very basic algorithm for now, as its mostly for demo purposes
        var sinceToday = DateTime.Now - post.Uploaded;
        return (1 + post.Likes) / (1 + sinceToday.TotalDays);
    }


}
