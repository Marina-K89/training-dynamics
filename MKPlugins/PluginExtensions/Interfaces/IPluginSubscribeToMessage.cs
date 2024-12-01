﻿namespace MKPlugins.PluginExtensions.Interfaces
{
    public interface IPluginSubscribeToMessage
    {
        IPluginSubscribeToEntity ForEntity(string entityLogicalName);

        IPluginSubscribeToEntity ForEntities(params string[] entityLogicalNames);

        IPluginSubscribeToEntity ForAnyEntity();
    }
}
