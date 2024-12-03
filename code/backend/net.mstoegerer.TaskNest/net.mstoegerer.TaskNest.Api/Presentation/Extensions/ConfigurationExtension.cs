namespace net.mstoegerer.TaskNest.Api.Presentation.Extensions;

public static class ConfigurationExtension
{
    public static T GetConfig<T>(this IConfiguration configuration, string sectionName) where T : class, new()
    {
        var config = new T();
        configuration.GetSection(sectionName).Bind(config);
        return config;
    }
}