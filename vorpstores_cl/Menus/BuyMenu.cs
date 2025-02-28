﻿using System.Collections.Generic;
using MenuAPI;

namespace vorpstores_cl.Menus
{
    public class BuyMenu
    {
        private static readonly Menu buyMenu = new Menu(GetConfig.Langs["BuyButton"], GetConfig.Langs["BuyMenuDesc"]);
        private static readonly Menu buyMenuConfirm = new Menu("", GetConfig.Langs["BuyMenuConfirmDesc"]);

        private static int indexItem;
        private static int quantityItem;

        private static bool setupDone;

        public static List<string> quantityList = new List<string>();

        private static void SetupMenu()
        {
            if (setupDone)
            {
                return;
            }

            setupDone = true;
            MenuController.AddMenu(buyMenu);

            MenuController.EnableMenuToggleKeyOnController = false;
            MenuController.MenuToggleKey = 0;

            MenuController.AddSubmenu(buyMenu, buyMenuConfirm);

            for (var i = 1; i < 101; i++)
            {
                quantityList.Add($"{GetConfig.Langs["Quantity"]} #{i}");
            }

            var subMenuConfirmBuyBtnYes = new MenuItem("", " ")
            {
                    RightIcon = MenuItem.Icon.TICK
            };
            var subMenuConfirmBuyBtnNo = new MenuItem(GetConfig.Langs["BuyConfirmButtonNo"], " ")
            {
                    RightIcon = MenuItem.Icon.ARROW_LEFT
            };

            buyMenuConfirm.AddMenuItem(subMenuConfirmBuyBtnYes);
            buyMenuConfirm.AddMenuItem(subMenuConfirmBuyBtnNo);

            buyMenu.OnListItemSelect += (_menu, _listItem, _listIndex, _itemIndex) =>
            {
                indexItem = _itemIndex;
                quantityItem = _listIndex + 1;
                var totalPrice =
                        double.Parse(GetConfig.Config["Stores"][StoreActions.LaststoreId]["ItemsBuy"][_itemIndex]
                                             ["BuyPrice"].ToString()) * quantityItem;
                buyMenuConfirm.MenuTitle =
                        GetConfig.ItemsFromDB
                                        [GetConfig.Config["Stores"][StoreActions.LaststoreId]["ItemsBuy"][_itemIndex]["Name"].ToString()]
                                ["label"].ToString();
                subMenuConfirmBuyBtnYes.Label = string.Format(GetConfig.Langs["BuyConfirmButtonYes"],
                                                              (_listIndex + 1).ToString(),
                                                              GetConfig.ItemsFromDB
                                                                              [GetConfig.Config["Stores"][StoreActions.LaststoreId]["ItemsBuy"][_itemIndex]["Name"].ToString()]
                                                                      ["label"], totalPrice.ToString());
            };

            buyMenu.OnIndexChange += (_menu, _oldItem, _newItem, _oldIndex, _newIndex) =>
            {
                StoreActions.CreateObjectOnTable(_newIndex, "ItemsBuy");
            };

            buyMenu.OnMenuOpen += _menu =>
            {
                buyMenu.ClearMenuItems();

                foreach (var item in GetConfig.Config["Stores"][StoreActions.LaststoreId]["ItemsBuy"])
                {
                    var _itemToBuy =
                            new
                                    MenuListItem(GetConfig.ItemsFromDB[item["Name"].ToString()]["label"] + $" ${item["BuyPrice"]}",
                                                 quantityList, 0, "");

                    buyMenu.AddMenuItem(_itemToBuy);
                    MenuController.BindMenuItem(buyMenu, buyMenuConfirm, _itemToBuy);
                }

                StoreActions.CreateObjectOnTable(_menu.CurrentIndex, "ItemsBuy");
            };

            buyMenuConfirm.OnItemSelect += (_menu, _item, _index) =>
            {
                if (_index == 0)
                {
                    StoreActions.BuyItemStore(indexItem, quantityItem);
                    buyMenu.OpenMenu();
                    buyMenuConfirm.CloseMenu();
                }
                else
                {
                    buyMenu.OpenMenu();
                    buyMenuConfirm.CloseMenu();
                }
            };
        }

        public static Menu GetMenu()
        {
            SetupMenu();
            return buyMenu;
        }
    }
}
