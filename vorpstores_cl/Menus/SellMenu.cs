using System.Collections.Generic;
using MenuAPI;

namespace vorpstores_cl.Menus
{
    internal class SellMenu
    {
        private static readonly Menu
                sellMenu = new Menu(GetConfig.Langs["SellButton"], GetConfig.Langs["SellMenuDesc"]);

        private static readonly Menu sellMenuConfirm = new Menu("", GetConfig.Langs["SellMenuConfirmDesc"]);

        private static int indexItem;
        private static int quantityItem;

        private static bool setupDone;

        private static void SetupMenu()
        {
            if (setupDone)
            {
                return;
            }

            setupDone = true;
            MenuController.AddMenu(sellMenu);

            MenuController.EnableMenuToggleKeyOnController = false;
            MenuController.MenuToggleKey = 0;

            MenuController.AddSubmenu(sellMenu, sellMenuConfirm);

            var subMenuConfirmSellBtnYes = new MenuItem("", " ")
            {
                    RightIcon = MenuItem.Icon.TICK
            };
            var subMenuConfirmSellBtnNo = new MenuItem(GetConfig.Langs["SellConfirmButtonNo"], " ")
            {
                    RightIcon = MenuItem.Icon.ARROW_LEFT
            };

            sellMenuConfirm.AddMenuItem(subMenuConfirmSellBtnYes);
            sellMenuConfirm.AddMenuItem(subMenuConfirmSellBtnNo);

            sellMenu.OnListItemSelect += (_menu, _listItem, _listIndex, _itemIndex) =>
            {
                indexItem = _itemIndex;
                quantityItem = _listIndex + 1;
                var totalPrice =
                        double.Parse(GetConfig.Config["Stores"][StoreActions.LaststoreId]["ItemsSell"][_itemIndex]
                                             ["SellPrice"].ToString()) * quantityItem;
                sellMenuConfirm.MenuTitle =
                        GetConfig.ItemsFromDB
                                        [GetConfig.Config["Stores"][StoreActions.LaststoreId]["ItemsSell"][_itemIndex]["Name"].ToString()]
                                ["label"].ToString();
                subMenuConfirmSellBtnYes.Label = string.Format(GetConfig.Langs["SellConfirmButtonYes"],
                                                               (_listIndex + 1).ToString(),
                                                               GetConfig.ItemsFromDB
                                                                               [GetConfig.Config["Stores"][StoreActions.LaststoreId]["ItemsSell"][_itemIndex]["Name"].ToString()]
                                                                       ["label"], totalPrice.ToString());
            };

            sellMenu.OnIndexChange += (_menu, _oldItem, _newItem, _oldIndex, _newIndex) =>
            {
                StoreActions.CreateObjectOnTable(_newIndex, "ItemsSell");
            };

            sellMenu.OnMenuOpen += _menu =>
            {
                sellMenu.ClearMenuItems();

                foreach (var item in GetConfig.Config["Stores"][StoreActions.LaststoreId]["ItemsSell"])
                {
                    var quantityList = new List<string>();

                    for (var i = 1; i < 101; i++)
                    {
                        quantityList.Add($"{GetConfig.Langs["Quantity"]} #{i}");
                    }

                    var _itemToSell =
                            new
                                    MenuListItem(GetConfig.ItemsFromDB[item["Name"].ToString()]["label"] + $" ${item["BuySell"]}",
                                                 quantityList, 0, "");

                    sellMenu.AddMenuItem(_itemToSell);
                    MenuController.BindMenuItem(sellMenu, sellMenuConfirm, _itemToSell);
                }

                StoreActions.CreateObjectOnTable(_menu.CurrentIndex, "ItemsSell");
            };

            sellMenuConfirm.OnItemSelect += (_menu, _item, _index) =>
            {
                if (_index == 0)
                {
                    StoreActions.SellItemStore(indexItem, quantityItem);
                    sellMenu.OpenMenu();
                    sellMenuConfirm.CloseMenu();
                }
                else
                {
                    sellMenu.OpenMenu();
                    sellMenuConfirm.CloseMenu();
                }
            };
        }

        public static Menu GetMenu()
        {
            SetupMenu();
            return sellMenu;
        }
    }
}
