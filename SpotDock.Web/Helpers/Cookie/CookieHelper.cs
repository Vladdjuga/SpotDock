using MassTransit;

namespace SpotDock.Web.Helpers.Cookie;

public static class CookieHelper
{
    public static void SetAuth(this IResponseCookies cookies,string token)
    {
        var cookieOptions = new CookieOptions
        {
            HttpOnly = true,
            Secure = true,
            SameSite = SameSiteMode.Strict,
            Expires = DateTimeOffset.UtcNow.AddHours(1)
        };
        cookies.Append("access_token", token, cookieOptions);
    }
}