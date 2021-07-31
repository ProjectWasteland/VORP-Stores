using System.Threading.Tasks;
using CitizenFX.Core;
using MenuAPI;
using static CitizenFX.Core.Native.API;

namespace vorpstores_cl
{
    public class StoreActions : BaseScript
    {
        private static int ObjectStore;
        private static int CamStore;
        public static int LaststoreId;

        public static async Task EnterBuyStore(int storeId)
        {
            LaststoreId = storeId;
            var Camerax = float.Parse(GetConfig.Config["Stores"][storeId]["CameraMain"][0].ToString());
            var Cameray = float.Parse(GetConfig.Config["Stores"][storeId]["CameraMain"][1].ToString());
            var Cameraz = float.Parse(GetConfig.Config["Stores"][storeId]["CameraMain"][2].ToString());
            var CameraRotx = float.Parse(GetConfig.Config["Stores"][storeId]["CameraMain"][3].ToString());
            var CameraRoty = float.Parse(GetConfig.Config["Stores"][storeId]["CameraMain"][4].ToString());
            var CameraRotz = float.Parse(GetConfig.Config["Stores"][storeId]["CameraMain"][5].ToString());

            TriggerEvent("vorp:setInstancePlayer", true);
            NetworkSetInSpectatorMode(true, PlayerPedId());
            FreezeEntityPosition(PlayerPedId(), true);
            SetEntityVisible(PlayerPedId(), false);

            CamStore = CreateCamWithParams("DEFAULT_SCRIPTED_CAMERA", Camerax, Cameray, Cameraz, CameraRotx, CameraRoty,
                                           CameraRotz, 50.00f, false, 0);
            SetCamActive(CamStore, true);
            RenderScriptCams(true, true, 500, true, true, 0);

            MenuController.MainMenu.MenuTitle = GetConfig.Config["Stores"][storeId]["name"].ToString();

            MenuController.MainMenu.OpenMenu();
        }

        public static async Task CreateObjectOnTable(int index, string list)
        {
            DeleteObject(ref ObjectStore);
            var objectX = float.Parse(GetConfig.Config["Stores"][LaststoreId]["SpawnObjectStore"][0].ToString());
            var objectY = float.Parse(GetConfig.Config["Stores"][LaststoreId]["SpawnObjectStore"][1].ToString());
            var objectZ = float.Parse(GetConfig.Config["Stores"][LaststoreId]["SpawnObjectStore"][2].ToString());
            var objectH = float.Parse(GetConfig.Config["Stores"][LaststoreId]["SpawnObjectStore"][3].ToString());
            var idObject =
                    (uint)GetHashKey(GetConfig.Config["Stores"][LaststoreId][list][index]["ObjectModel"].ToString());
            await vorpstores_init.LoadModel(idObject);
            ObjectStore = CreateObject(idObject, objectX, objectY, objectZ, false, true, true, true, true);
        }

        public static async Task ExitBuyStore()
        {
            await Delay(100);
            if (!MenuController.IsAnyMenuOpen())
            {
                TriggerEvent("vorp:setInstancePlayer", false);
                NetworkSetInSpectatorMode(false, PlayerPedId());
                FreezeEntityPosition(PlayerPedId(), false);
                SetEntityVisible(PlayerPedId(), true);
                SetCamActive(CamStore, false);
                RenderScriptCams(false, true, 1000, true, true, 0);
                DestroyCam(CamStore, true);

                DeleteObject(ref ObjectStore);
            }
        }

        public static async Task BuyItemStore(int indexItem, int quantityItem)
        {
            TriggerServerEvent("vorpstores:buyItems",
                               GetConfig.Config["Stores"][LaststoreId]["ItemsBuy"][indexItem]["Name"].ToString(),
                               quantityItem,
                               GetConfig.Config["Stores"][LaststoreId]["ItemsBuy"][indexItem]["BuyPrice"]
                                        .ToObject<double>());
        }

        public static async Task SellItemStore(int indexItem, int quantityItem)
        {
            TriggerServerEvent("vorpstores:sellItems",
                               GetConfig.Config["Stores"][LaststoreId]["ItemsSell"][indexItem]["Name"].ToString(),
                               quantityItem,
                               GetConfig.Config["Stores"][LaststoreId]["ItemsSell"][indexItem]["SellPrice"]
                                        .ToObject<double>());
        }
    }
}
