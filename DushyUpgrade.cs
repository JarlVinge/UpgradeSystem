using DushyUpgrade.Tiles;
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

        public const int SUCCESS = 1;
        public const int NO_CHANGE = 2;
        public const int FAILURE = 3;
        public const int RESET = 4;
        public const int BREAKING = 5;

        public const int FAILURE_PROTECTED = 10;
        public const int RESET_PROTECTED = 11;
        public const int BREAKING_PROTECTED = 12;

        int[] upgradeStones;
        int[] protectionScrolls;

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
			protectionScrolls = new int[] {
				ItemType<Items.ProtectionScroll>(),
			};

			upgradeStones = new int[] {
				ItemType<Items.UpgradeStone>(),
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
						if (upgradeWorkbenchTE != null)
						{
							DrawSelf(Main.spriteBatch);
						}
						return true;
					},
					InterfaceScaleType.UI)
				);
			}
		}

		string[] labels = new string[] { "Item to Upgrade", "Upgrade Stone", "Scroll of Protection" };
		protected void DrawSelf(SpriteBatch spriteBatch)
		{
			float oldScale = Main.inventoryScale;
			Main.inventoryScale = 0.75f;
            Point value = new Point(Main.mouseX, Main.mouseY);

			Rectangle hitbox = new Rectangle(167 - 8, Main.instance.invBottom + 44, 200, 220);
			Main.spriteBatch.Draw(Main.magicPixel, hitbox, Color.Gray);

			Main.spriteBatch.DrawString(Main.fontMouseText,
				"Upgrade Workbench", new Vector2(167 + 33, Main.instance.invBottom + 54),
				Color.White, 0f, default(Vector2), Main.inventoryScale, SpriteEffects.None, 0f);

			hitbox = new Rectangle(167 + 40, Main.instance.invBottom + 44 + 170, 100, 40);
			Main.spriteBatch.Draw(Main.magicPixel, hitbox, Color.Black);
			Main.spriteBatch.DrawString(Main.fontMouseText,
				"Upgrade", new Vector2(167 + 66, Main.instance.invBottom + 44 + 180),
				Color.White, 0f, default(Vector2), Main.inventoryScale, SpriteEffects.None, 0f);

            if (hitbox.Contains(value) && !PlayerInput.IgnoreMouseInterface && Main.mouseLeft && Main.mouseLeftRelease) {
                Main.player[Main.myPlayer].mouseInterface = true;
                if(IsInventoryFull(Main.player[Main.myPlayer])) {
                    
                }
                if (upgradeWorkbenchTE.upgradeItem.IsAir) //First case empty
                    Main.NewText("You need an item to upgrade");
                else if(upgradeWorkbenchTE.upgradeStone.IsAir) {
                    Main.NewText("You need an upgrade stone to upgrade");
                }
                else {
                    Boolean isProtected = !upgradeWorkbenchTE.protectionScroll.IsAir;
                    if(!IsItemBroken(upgradeWorkbenchTE.upgradeItem)) {
                        UpgradeInfo info = upgradeWorkbenchTE.upgradeItem.GetGlobalItem<UpgradeInfo>(this);
                        int result = info.Upgrade(upgradeWorkbenchTE.upgradeItem, isProtected);

                        upgradeWorkbenchTE.upgradeStone.stack--;
                        if (isProtected)
                            upgradeWorkbenchTE.protectionScroll.stack--;

                        /*if (result == DushyUpgrade.SUCCESS) {
                            ItemText.NewText(Main.player[Main.myPlayer], "Success !", Color.Green);
                            if(IsItemMaxLevel(upgradeWorkbenchTE.upgradeItem)) {
                                ItemText.NewText(Main.player[Main.myPlayer], "Level Max !", Color.LightSkyBlue);
                                Item item = upgradeWorkbenchTE.upgradeItem;
                                Item itemClone = item.Clone();
                                Player player = Main.player[Main.myPlayer];
                                item.position = player.Center;
                                Item item2 = player.GetItem(player.whoAmI, itemClone, false, true);
                                if (item2.stack > 0) {
                                    int num = Item.NewItem((int)player.position.X, (int)player.position.Y, player.width, player.height, item2.type, item2.stack, false, (int)item.prefix, true, false);
                                    Main.item[num].newAndShiny = false;
                                    if (Main.netMode == 1) {
                                        NetMessage.SendData(21, -1, -1, null, num, 1f, 0f, 0f, 0, 0, 0);
                                    }
                                }
                                item = new Item();
                                upgradeWorkbenchTE.upgradeItem.TurnToAir();
                            }
                        }*/
                        if (result == DushyUpgrade.SUCCESS) {
                            ItemText.NewText(Main.player[Main.myPlayer], "Success !", Color.Green);
                            if (IsItemMaxLevel(upgradeWorkbenchTE.upgradeItem)) {
                                ItemText.NewText(Main.player[Main.myPlayer], "Level Max !", Color.LightSkyBlue);
                                Player player = Main.player[Main.myPlayer];
                                Item item = upgradeWorkbenchTE.upgradeItem;
                                item.position = player.Center;
                                Item leftoverItem = player.GetItem(player.whoAmI, upgradeWorkbenchTE.upgradeItem, false, true);
                                if (leftoverItem.stack > 0) {
                                    int num = Item.NewItem((int)player.position.X, (int)player.position.Y, player.width, player.height, leftoverItem.type, leftoverItem.stack, false, (int)leftoverItem.prefix, true, false);
                                    Main.item[num] = leftoverItem.Clone();
                                    Main.item[num].newAndShiny = false;
                                    if (Main.netMode == 1) {
                                        NetMessage.SendData(21, -1, -1, null, num, 1f, 0f, 0f, 0, 0, 0);
                                    }
                                }
                                upgradeWorkbenchTE.upgradeItem.TurnToAir();
                            }
                        }
                        else if (result == DushyUpgrade.NO_CHANGE) {
                            ItemText.NewText(Main.player[Main.myPlayer], "No Change !", Color.White);
                        }
                        else if (result == DushyUpgrade.FAILURE) {
                            ItemText.NewText(Main.player[Main.myPlayer], "Failure !", Color.LightGray);
                        }
                        else if (result == DushyUpgrade.RESET) {
                            ItemText.NewText(Main.player[Main.myPlayer], "Reset !", Color.Gray);
                        }
                        else if (result == DushyUpgrade.BREAKING) {
                            ItemText.NewText(Main.player[Main.myPlayer], "Breaking !", Color.Black);
                        }
                        else {
                            ItemText.NewText(Main.player[Main.myPlayer], "Protected !", Color.DarkBlue);
                        }
                    }
                }
            }
            for (int i = 0; i < 3; i++)
			{
				int x = 167;
				int y = 42 * i + Main.instance.invBottom + 84;
				Rectangle r = new Rectangle(x, y, (int)((float)Main.inventoryBackTexture.Width * Main.inventoryScale), (int)((float)Main.inventoryBackTexture.Height * Main.inventoryScale));
				Item item = i == 0 ? upgradeWorkbenchTE.upgradeItem : (i == 1 ? upgradeWorkbenchTE.upgradeStone : upgradeWorkbenchTE.protectionScroll);

				if (r.Contains(value) && !PlayerInput.IgnoreMouseInterface)
				{
					Main.player[Main.myPlayer].mouseInterface = true;
					if (CanGoInSlot(Main.mouseItem, i))
						ItemSlot.Handle(ref item, 3);
				}
				ItemSlot.Draw(spriteBatch, ref item, 3, new Vector2(x, y));
				switch (i)
				{
					case 0:
						upgradeWorkbenchTE.upgradeItem = item;
						break;
					case 1:
						upgradeWorkbenchTE.upgradeStone = item;
						break;
					case 2:
						upgradeWorkbenchTE.protectionScroll = item;
						break;
					default:
						break;
				}

				Main.spriteBatch.DrawString(Main.fontMouseText,
				labels[i], new Vector2(x + 48, y + 10),
				Color.White, 0f, default(Vector2), Main.inventoryScale, SpriteEffects.None, 0f);
			}
			Main.inventoryScale = oldScale;
		}

		private bool CanGoInSlot(Item item, int slotNumber)
		{
			if (item.IsAir)
                return true;

			if (slotNumber == 0) //First slot
                if (IsUpgradable(item))
                    return true;
                else
                    return false;

			else if (slotNumber == 1) //Second slot
				return upgradeStones.Contains(item.type);

			else //Last slot
				return protectionScrolls.Contains(item.type);
		}

        private Boolean IsUpgradable(Item item) {
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

        public Boolean IsItemBroken(Item item) {
            UpgradeInfo info = item.GetGlobalItem<UpgradeInfo>(this);
            return info.broken;
        }

        public Boolean IsItemMaxLevel(Item item) {
            UpgradeInfo info = item.GetGlobalItem<UpgradeInfo>(this);
            return info.level == 11;
        }

        public Boolean IsInventoryFull(Player player) {
            for(int i = 0; i < 50; i++)
                if(player.inventory[i].IsAir)
                    return true;
            return false;
        }
    }
}
