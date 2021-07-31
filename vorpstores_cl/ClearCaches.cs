using System;
using CitizenFX.Core;
using static CitizenFX.Core.Native.API;

namespace vorpstores_cl
{
    internal class ClearCaches : BaseScript
    {
        public ClearCaches()
        {
            EventHandlers["onResourceStop"] += new Action<string>(OnResourceStop);
        }

        private void OnResourceStop(string resourceName)
        {
            if (GetCurrentResourceName() != resourceName)
            {
                return;
            }

            foreach (var blip in vorpstores_init.StoreBlips)
            {
                var _blip = blip;
                RemoveBlip(ref _blip);
            }

            foreach (var npc in vorpstores_init.StorePeds)
            {
                var _ped = npc;
                DeletePed(ref _ped);
            }
        }
    }
}
