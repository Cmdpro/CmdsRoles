using BepInEx.IL2CPP;
using CrowdedRoles.Options;
using System;
using System.Collections.Generic;
using System.Text;

namespace CrowdedRoles.Components
{
    public static class RoleActive
    {
        public static CustomToggleOption BlinderActive { get; } = new("Blinder")
        {
            //OnValueChanged = v => RoleApiPlugin.Logger.LogDebug($"new test bool: {v}")
        };
        public static CustomToggleOption CopierActive { get; } = new("Copier")
        {
            //OnValueChanged = v => RoleApiPlugin.Logger.LogDebug($"new test bool: {v}")
        };
        public static CustomToggleOption CurserActive { get; } = new("Curser")
        {
            //OnValueChanged = v => RoleApiPlugin.Logger.LogDebug($"new test bool: {v}")
        };
        public static CustomToggleOption DetectiveActive { get; } = new("Detective")
        {
            //OnValueChanged = v => RoleApiPlugin.Logger.LogDebug($"new test bool: {v}")
        };
        public static CustomToggleOption DissapearerActive { get; } = new("Dissapearer")
        {
            //OnValueChanged = v => RoleApiPlugin.Logger.LogDebug($"new test bool: {v}")
        };
        public static CustomToggleOption ElectricianActive { get; } = new("Electrician")
        {
            //OnValueChanged = v => RoleApiPlugin.Logger.LogDebug($"new test bool: {v}")
        };
        public static CustomToggleOption JesterActive { get; } = new("Jester")
        {
            //OnValueChanged = v => RoleApiPlugin.Logger.LogDebug($"new test bool: {v}")
        };
        public static CustomToggleOption ProtectorActive { get; } = new("Protector")
        {
            //OnValueChanged = v => RoleApiPlugin.Logger.LogDebug($"new test bool: {v}")
        };
        public static CustomToggleOption RevengerActive { get; } = new("Revenger")
        {
            //OnValueChanged = v => RoleApiPlugin.Logger.LogDebug($"new test bool: {v}")
        };
        public static CustomToggleOption TeleporterActive { get; } = new("Teleporter")
        {
            //OnValueChanged = v => RoleApiPlugin.Logger.LogDebug($"new test bool: {v}")
        };
        public static CustomToggleOption TrackerActive { get; } = new("Tracker")
        {
            //OnValueChanged = v => RoleApiPlugin.Logger.LogDebug($"new test bool: {v}")
        };
        public static CustomToggleOption HelperActive { get; } = new("Helper")
        {
            //OnValueChanged = v => RoleApiPlugin.Logger.LogDebug($"new test bool: {v}")
        };
        public static CustomToggleOption EngineerActive { get; } = new("Engineer")
        {
            //OnValueChanged = v => RoleApiPlugin.Logger.LogDebug($"new test bool: {v}")
        };
        public static CustomToggleOption SherriffActive { get; } = new("Sheriff")
        {
            //OnValueChanged = v => RoleApiPlugin.Logger.LogDebug($"new test bool: {v}")
        };
        public static CustomToggleOption DoctorActive { get; } = new("Doctor")
        {
            //OnValueChanged = v => RoleApiPlugin.Logger.LogDebug($"new test bool: {v}")
        };
        public static CustomToggleOption VampireActive { get; } = new("Vampire")
        {
            //OnValueChanged = v => RoleApiPlugin.Logger.LogDebug($"new test bool: {v}")
        };
        public static CustomToggleOption ShrinkerActive { get; } = new("Shrinker")
        {
            //OnValueChanged = v => RoleApiPlugin.Logger.LogDebug($"new test bool: {v}")
        };
        public static CustomToggleOption TrollActive { get; } = new("Troll")
        {
            //OnValueChanged = v => RoleApiPlugin.Logger.LogDebug($"new test bool: {v}")
        };
        public static CustomToggleOption ReflectorActive { get; } = new("Reflector")
        {
            //OnValueChanged = v => RoleApiPlugin.Logger.LogDebug($"new test bool: {v}")
        };
        public static CustomToggleOption HackerActive { get; } = new("Hacker")
        {
            //OnValueChanged = v => RoleApiPlugin.Logger.LogDebug($"new test bool: {v}")
        };
        public static CustomToggleOption ReverseJesterActive { get; } = new("Reverse Jester")
        {
            //OnValueChanged = v => RoleApiPlugin.Logger.LogDebug($"new test bool: {v}")
        };
        public static CustomToggleOption VentSeerActive { get; } = new("Vent Seer")
        {
            //OnValueChanged = v => RoleApiPlugin.Logger.LogDebug($"new test bool: {v}")
        };
        public static CustomToggleOption ShapeshifterActive { get; } = new("Shapeshifter")
        {
            //OnValueChanged = v => RoleApiPlugin.Logger.LogDebug($"new test bool: {v}")
        };
        public static CustomToggleOption NinjaActive { get; } = new("Ninja")
        {
            //OnValueChanged = v => RoleApiPlugin.Logger.LogDebug($"new test bool: {v}")
        };
        public static void RegisterOptions(BasePlugin plugin)
        {
            new OptionPluginWrapper(plugin)
                .AddCustomOption(BlinderActive)
                .AddCustomOption(CopierActive)
                .AddCustomOption(CurserActive)
                .AddCustomOption(DetectiveActive)
                .AddCustomOption(DissapearerActive)
                .AddCustomOption(ElectricianActive)
                .AddCustomOption(JesterActive)
                .AddCustomOption(ProtectorActive)
                .AddCustomOption(RevengerActive)
                .AddCustomOption(TeleporterActive)
                .AddCustomOption(TrackerActive)
                .AddCustomOption(HelperActive)
                .AddCustomOption(EngineerActive)
                .AddCustomOption(SherriffActive)
                .AddCustomOption(DoctorActive)
                .AddCustomOption(VampireActive)
                .AddCustomOption(ShrinkerActive)
                .AddCustomOption(TrollActive)
                .AddCustomOption(ReflectorActive)
                .AddCustomOption(HackerActive)
                .AddCustomOption(ReverseJesterActive)
                .AddCustomOption(VentSeerActive)
                .AddCustomOption(ShapeshifterActive)
                .AddCustomOption(NinjaActive)
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
