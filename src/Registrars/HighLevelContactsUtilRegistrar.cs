using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Soenneker.HighLevel.ClientUtil.Registrars;
using Soenneker.HighLevel.Contacts.Abstract;

namespace Soenneker.HighLevel.Contacts.Registrars;

/// <summary>
/// A .NET typesafe implementation of High Level's contact API
/// </summary>
public static class HighLevelContactsUtilRegistrar
{
    /// <summary>
    /// Adds <see cref="IHighLevelContactsUtil"/> as a singleton service. <para/>
    /// </summary>
    public static IServiceCollection AddHighLevelContactsUtilAsSingleton(this IServiceCollection services)
    {
        services.AddHighLevelClientUtilAsSingleton().TryAddSingleton<IHighLevelContactsUtil, HighLevelContactsUtil>();

        return services;
    }

    /// <summary>
    /// Adds <see cref="IHighLevelContactsUtil"/> as a scoped service. <para/>
    /// </summary>
    public static IServiceCollection AddHighLevelContactsUtilAsScoped(this IServiceCollection services)
    {
        services.AddHighLevelClientUtilAsSingleton().TryAddScoped<IHighLevelContactsUtil, HighLevelContactsUtil>();

        return services;
    }
}
