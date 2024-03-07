namespace SudiBlog.API.Services;
public class SlugService ( ApplicationDbContext context) : ISlugService
{
    private readonly ApplicationDbContext _context = context;
    public bool IsUnique(string slug)
    {
        return !_context.Posts.Any(p => p.Slug == slug);
    }

    public string UrlFriendly(string title)
    {
        if (title == null) return "";

        const int maxlen = 80;
        var len = title.Length;
        var prevdash = false;

        var stringBuilder = new StringBuilder(len);

        char c;
        for (var i = 0; i < len; i++)
        {
            c = title[i];
            switch (c)
            {
                case >= 'a' and <= 'z':
                case >= '0' and <= '9':
                    stringBuilder.Append(c);
                    prevdash = false;
                    break;
                case >= 'A' and <= 'Z':
                    // tricky way to convert to lowercase
                    stringBuilder.Append((char)(c | 32));
                    prevdash = false;
                    break;
                case ' ' or ',' or '.' or '/' or '\\' or '-' or '_' or '=':
                {
                    if (!prevdash && stringBuilder.Length > 0)
                    {
                        stringBuilder.Append('-');
                        prevdash = true;
                    }

                    break;
                }
                case '#':
                {
                    if (i > 0)
                        if (title[i - 1] == 'C' || title[i - 1] == 'F')
                            stringBuilder.Append("-sharp");
                    break;
                }
                case '+':
                    stringBuilder.Append("-plus");
                    break;
                default:
                {
                    if (c >= 128)
                    {
                        var prevlen = stringBuilder.Length;
                        stringBuilder.Append(RemapInternationalCharToAscii(c));
                        if (prevlen != stringBuilder.Length) prevdash = false;
                    }

                    break;
                }
            }
            if (stringBuilder.Length == maxlen) break;
        }
        return prevdash ? stringBuilder.ToString().Substring(0, stringBuilder.Length - 1) : stringBuilder.ToString();
    }

    private string RemapInternationalCharToAscii(char c)
    {
        var s = c.ToString().ToLowerInvariant();
        if ("àåáâäãåą".Contains(s))
        {
            return "a";
        }

        if ("èéêëę".Contains(s))
        {
            return "e";
        }
        if ("ìíîïı".Contains(s))
        {
            return "i";
        }
        if ("òóôõöøőð".Contains(s))
        {
            return "o";
        }

        if ("ùúûüŭů".Contains(s))
        {
            return "u";
        }
        if ("çćčĉ".Contains(s))
        {
            return "c";
        }
        if ("żźž".Contains(s))
        {
            return "z";
        }
        if ("śşšŝ".Contains(s))
        {
            return "s";
        }
        if ("ñń".Contains(s))
        {
            return "n";
        }
        if ("ýÿ".Contains(s))
        {
            return "y";
        }
        if ("ğĝ".Contains(s))
        {
            return "g";
        }

        return c switch
        {
            'ř' => "r",
            'ł' => "l",
            'đ' => "d",
            'ß' => "ss",
            'Þ' => "th",
            'ĥ' => "h",
            _ => c == 'ĵ' ? "j" : ""
        };
    }
}