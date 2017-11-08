using DushyUpgrade.Tiles;
using DushyUpgrade.UI;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ReLogic.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using Terraria;
using Terraria.GameInput;
using Terraria.ModLoader;
using Terraria.UI;

namespace DushyUpgrade
{
	class DushyUpgrade : Mod
	{
		internal static UpgradeWorkbenchTE upgradeWorkbenchTE;
		internal static RepairWorkbenchTE repairWorkbenchTE;
		internal static SocketWorkbenchTE socketWorkbenchTE;

        public const int SUCCESS = 1;
        public const int NO_CHANGE = 2;
        public const int FAILURE = 3;
        public const int RESET = 4;
        public const int BREAKING = 5;

        public const int FAILURE_PROTECTED = 10;
        public const int RESET_PROTECTED = 11;
        public const int BREAKING_PROTECTED = 12;

        public static int[] upgradeStones;
        public static int[] protectionScrolls;
        public static int[] upgradeScrolls;
        public static int[] repairScrolls;

        public DushyUpgrade()
		{
			Properties = new ModProperties()
			{
				Autoload = true,
				AutoloadGores = true,
				AutoloadSounds = true
			};
		}

		public override void Load()
		{
            upgradeStones = new int[] {
                ItemType<Items.UpgradeStone>(),
            };

            protectionScrolls = new int[] {
				ItemType<Items.ProtectionScroll>(),
			};

            upgradeScrolls = new int[] {
                ItemType<Items.UpgradeScroll4>(),
                ItemType<Items.UpgradeScroll5>(),
                ItemType<Items.UpgradeScroll6>(),
                ItemType<Items.UpgradeScroll7>(),
                ItemType<Items.UpgradeScroll8>(),
                ItemType<Items.UpgradeScroll9>(),
                ItemType<Items.UpgradeScroll10>(),
                ItemType<Items.UpgradeScroll11>(),
            };

            repairScrolls = new int[] {
                ItemType<Items.RepairScroll>(),
            };
        }

		public static void Log(Array array)
		{
			foreach (var item in array)
			{
				ErrorLogger.Log(item.ToString());
			}
		}

		public override void ModifyInterfaceLayers(List<GameInterfaceLayer> layers)
		{

			int inventoryLayerIndex = layers.FindIndex(layer => layer.Name.Equals("Vanilla: Mouse Text"));
			if (inventoryLayerIndex != -1)
			{
				layers.Insert(inventoryLayerIndex, new LegacyGameInterfaceLayer(
					"DushyUpgrade: Upgrade UI",
					delegate
					{
                        if (upgradeWorkbenchTE != null) {
                            UIUpgradeWorkbench.DrawSelf(Main.spriteBatch);
                        }
                        else if (repairWorkbenchTE != null) {
                            UIRepairWorkbench.DrawSelf(Main.spriteBatch);
                        }
                        else if (socketWorkbenchTE != null) {
                            UISocketWorkbench.DrawSelf(Main.spriteBatch);
                        }
                     return true;
					},
					InterfaceScaleType.UI)
				);
			}
		}

        public static Boolean IsUpgradable(Item item) {
            Boolean isAnArmor = IsAnArmor(item);
            Boolean isAWeapon = IsAWeapon(item);
            Boolean isItemBroken = IsItemBroken(item);
            Boolean isItemMaxLevel = IsItemMaxLevel(item);
            return ((isAnArmor || isAWeapon) && !isItemBroken && !isItemMaxLevel);
        }

        public static Boolean IsAnArmor(Item item) {
            return (item.headSlot > 0 || item.bodySlot > 0 || item.legSlot > 0);
        }

        public static Boolean IsAWeapon(Item item) {
            return (item.melee || item.magic || item.summon || item.ranged);
        }

        public static Boolean IsItemBroken(Item item) {
            UpgradeInfo info = item.GetGlobalItem<UpgradeInfo>();
            return info.broken;
        }

        public static Boolean IsItemMaxLevel(Item item) {
            UpgradeInfo info = item.GetGlobalItem<UpgradeInfo>();
            return info.level == 11;
        }

        public static Boolean IsInventoryFull(Player player) {
            for(int i = 0; i < 50; i++)
                if(player.inventory[i].IsAir)
                    return true;
            return false;
        }
    }
}
