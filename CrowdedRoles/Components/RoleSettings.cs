using BepInEx.IL2CPP;
using CrowdedRoles.Options;
using System;
using System.Collections.Generic;
using System.Text;

namespace CrowdedRoles.Components
{
    public static class RoleSettings
    {
        public static CustomToggleOption DetHelpKnow { get; } = new("Detective and Helper know each other")
        {
            //OnValueChanged = v => RoleApiPlugin.Logger.LogDebug($"new test bool: {v}")
        };
        public static CustomToggleOption DetHelpShare { get; } = new("Detective and Helper share their findings")
        {
            //OnValueChanged = v => RoleApiPlugin.Logger.LogDebug($"new test bool: {v}")
        };
        public static CustomNumberOption VampireMin { get; } = new("Vampire Minimum Cooldown", new FloatRange(3, 8))
        {
            //OnValueChanged = v => RoleApiPlugin.Logger.LogDebug($"new test bool: {v}")
        };
        public static void RegisterOptions(BasePlugin plugin)
        {

            new OptionPluginWrapper(plugin)
                .AddCustomOption(DetHelpKnow)
                .AddCustomOption(DetHelpShare)
                .AddCustomOption(VampireMin)
                //.AddCustomOption(ExitMeetingVote)
                ;
            // OR
            // new OptionPluginWrapper(plugin)
            //     .AddCustomOptions(new CustomOption[]
            //     {
            //         IncrementMe,
            //         ToggleMe,
            //         FixMe
            //     });
        }
    }
}
