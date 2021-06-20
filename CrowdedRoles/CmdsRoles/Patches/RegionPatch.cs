using Array = System.Array;

namespace CmdsRoles.Patches
{
    class RegionPatch
    {
        private static IRegionInfo[] regions = new IRegionInfo[]
        {
            // 64.227.31.186:22023
            new DnsRegionInfo("67.248.215.131", "CmdPro's Server", StringNames.NoTranslation, "67.248.215.131", 22023).Duplicate(),
            new DnsRegionInfo("localhost", "LocalHost", StringNames.NoTranslation, "127.0.0.1", 22023).Duplicate()
        };

        public static void Patch()
        {
            IRegionInfo[] patchedRegions = new IRegionInfo[ServerManager.DefaultRegions.Length + regions.Length];
            Array.Copy(ServerManager.DefaultRegions, patchedRegions, ServerManager.DefaultRegions.Length);
            Array.Copy(regions, 0, patchedRegions, ServerManager.DefaultRegions.Length, regions.Length);

            ServerManager.DefaultRegions = patchedRegions;
            ServerManager.Instance.AvailableRegions = patchedRegions;
            ServerManager.Instance.SaveServers();
        }
    }
    
}