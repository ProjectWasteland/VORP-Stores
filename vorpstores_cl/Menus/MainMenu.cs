using MenuAPI;

namespace vorpstores_cl.Menus
{
    internal class MainMenu
    {
        private static readonly Menu mainMenu = new Menu("", GetConfig.Langs["DescMainMenu"]);
        private static bool setupDone;

        private static void SetupMenu()
        {
            if (setupDone)
            {
                return;
            }

            setupDone = true;
            MenuController.AddMenu(mainMenu);

            MenuController.EnableMenuToggleKeyOnController = false;
            MenuController.MenuToggleKey = 0;

            //Buy Menu
            MenuController.AddSubmenu(mainMenu, BuyMenu.GetMenu());

            var subMenuBuyBtn = new MenuItem(GetConfig.Langs["BuyButton"], " ")
            {
                    RightIcon = MenuItem.Icon.ARROW_RIGHT
            };

            mainMenu.AddMenuItem(subMenuBuyBtn);
            MenuController.BindMenuItem(mainMenu, BuyMenu.GetMenu(), subMenuBuyBtn);

            //Sell Menu
            MenuController.AddSubmenu(mainMenu, SellMenu.GetMenu());

            var subMenuSellBtn = new MenuItem(GetConfig.Langs["SellButton"], " ")
            {
                    RightIcon = MenuItem.Icon.ARROW_RIGHT
            };

            mainMenu.AddMenuItem(subMenuSellBtn);
            MenuController.BindMenuItem(mainMenu, SellMenu.GetMenu(), subMenuSellBtn);

            mainMenu.OnMenuClose += _menu => { StoreActions.ExitBuyStore(); };
        }

        public static Menu GetMenu()
        {
            SetupMenu();
            return mainMenu;
        }
    }
}
