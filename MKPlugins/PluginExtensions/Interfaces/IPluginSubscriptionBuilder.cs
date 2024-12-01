namespace MKPlugins.PluginExtensions.Interfaces
{
    public interface IPluginSubscriptionBuilder
    {
        IPluginSubscribeToMessage ToMessage(string message);
    }
}
