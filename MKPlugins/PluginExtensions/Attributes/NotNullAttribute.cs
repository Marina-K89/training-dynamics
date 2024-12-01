﻿using System;

namespace MKPlugins.PluginExtensions.Attributes
{
    [AttributeUsage(AttributeTargets.Method | AttributeTargets.Property | AttributeTargets.Field | AttributeTargets.Event | AttributeTargets.Parameter | AttributeTargets.Delegate)]
    public sealed class NotNullAttribute : Attribute
    {
    }
}
