using CitizenFX.Core;
using MenuAPI;
using System.Collections.Generic;

namespace vorpstores_cl.Menus
{
    class MainMenu
    {
        private static Menu mainMenu = new Menu("", GetConfig.Langs["DescMainMenu"]);
        private static bool setupDone = false;

        private static void SetupMenu()
        {
            if (setupDone) return;
            setupDone = true;
            MenuController.AddMenu(mainMenu);

            MenuController.EnableMenuToggleKeyOnController = false;
            MenuController.MenuToggleKey = (Control)0;

            //Buy Menu
            MenuController.AddSubmenu(mainMenu, BuyMenu.GetMenu());

            MenuItem subMenuBuyBtn = new MenuItem(GetConfig.Langs["BuyButton"], " ")
            {
                RightIcon = MenuItem.Icon.ARROW_RIGHT
            };

            mainMenu.AddMenuItem(subMenuBuyBtn);
            MenuController.BindMenuItem(mainMenu, BuyMenu.GetMenu(), subMenuBuyBtn);

            //Sell Menu
            MenuController.AddSubmenu(mainMenu, SellMenu.GetMenu());

            MenuItem subMenuSellBtn = new MenuItem(GetConfig.Langs["SellButton"], " ")
            {
                RightIcon = MenuItem.Icon.ARROW_RIGHT
            };

            mainMenu.AddMenuItem(subMenuSellBtn);
            MenuController.BindMenuItem(mainMenu, SellMenu.GetMenu(), subMenuSellBtn);

            mainMenu.OnMenuClose += (_menu) =>
            {
                StoreActions.ExitBuyStore();
            };


        }
        public static Menu GetMenu()
        {
            SetupMenu();
            return mainMenu;
        }

    }
}
