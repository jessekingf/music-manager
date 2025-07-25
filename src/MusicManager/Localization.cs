namespace MusicManager;

using System.Globalization;
using System.Resources;

internal static class Localization
{
    private static readonly ResourceManager ResourceManager =
        new("MusicManager.Properties.Resources", typeof(Localization).Assembly);

    public static CultureInfo CurrentCulture
    {
        get
        {
            return CultureInfo.CurrentUICulture;
        }

        set
        {
            CultureInfo.CurrentUICulture = value;
        }
    }

    public static string Get(string name, params string[]? args)
    {
        string? resource = ResourceManager.GetString(name, CurrentCulture);
        if (string.IsNullOrEmpty(resource))
        {
            return name;
        }

        if (args != null && args.Length > 0)
        {
            resource = string.Format(resource, args);
        }

        return resource;
    }
}
