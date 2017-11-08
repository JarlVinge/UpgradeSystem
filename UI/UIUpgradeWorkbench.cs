using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.GameInput;
using Terraria.UI;

namespace DushyUpgrade.UI {
    class UIUpgradeWorkbench {

        private static string[] labels = new string[] { "Item to Upgrade", "Upgrade Stone", "Scroll of Protection" };

        public static void DrawSelf(SpriteBatch spriteBatch) {
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
                if (DushyUpgrade.upgradeWorkbenchTE.upgradeItem.IsAir) //First case empty
                    Main.NewText("You need an item to upgrade");
                else if (DushyUpgrade.upgradeWorkbenchTE.upgradeStone.IsAir) {
                    Main.NewText("You need an upgrade stone to upgrade");
                }
                else {
                    Boolean isProtected = !DushyUpgrade.upgradeWorkbenchTE.protectionScroll.IsAir;
                    if (!DushyUpgrade.IsItemBroken(DushyUpgrade.upgradeWorkbenchTE.upgradeItem)) {
                        UpgradeInfo info = DushyUpgrade.upgradeWorkbenchTE.upgradeItem.GetGlobalItem<UpgradeInfo>();
                        int result = info.Upgrade(DushyUpgrade.upgradeWorkbenchTE.upgradeItem, isProtected);

                        DushyUpgrade.upgradeWorkbenchTE.upgradeStone.stack--;
                        if (isProtected)
                            DushyUpgrade.upgradeWorkbenchTE.protectionScroll.stack--;

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
                            if (DushyUpgrade.IsItemMaxLevel(DushyUpgrade.upgradeWorkbenchTE.upgradeItem)) {
                                ItemText.NewText(Main.player[Main.myPlayer], "Level Max !", Color.LightSkyBlue);
                                Player player = Main.player[Main.myPlayer];
                                Item item = DushyUpgrade.upgradeWorkbenchTE.upgradeItem;
                                item.position = player.Center;
                                Item leftoverItem = player.GetItem(player.whoAmI, DushyUpgrade.upgradeWorkbenchTE.upgradeItem, false, true);
                                if (leftoverItem.stack > 0) {
                                    int num = Item.NewItem((int)player.position.X, (int)player.position.Y, player.width, player.height, leftoverItem.type, leftoverItem.stack, false, (int)leftoverItem.prefix, true, false);
                                    Main.item[num] = leftoverItem.Clone();
                                    Main.item[num].newAndShiny = false;
                                    if (Main.netMode == 1) {
                                        NetMessage.SendData(21, -1, -1, null, num, 1f, 0f, 0f, 0, 0, 0);
                                    }
                                }
                                DushyUpgrade.upgradeWorkbenchTE.upgradeItem.TurnToAir();
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
            for (int i = 0; i < 3; i++) {
                int x = 167;
                int y = 42 * i + Main.instance.invBottom + 84;
                Rectangle r = new Rectangle(x, y, (int)((float)Main.inventoryBackTexture.Width * Main.inventoryScale), (int)((float)Main.inventoryBackTexture.Height * Main.inventoryScale));
                Item item = i == 0 ? DushyUpgrade.upgradeWorkbenchTE.upgradeItem : (i == 1 ? DushyUpgrade.upgradeWorkbenchTE.upgradeStone : DushyUpgrade.upgradeWorkbenchTE.protectionScroll);

                if (r.Contains(value) && !PlayerInput.IgnoreMouseInterface) {
                    Main.player[Main.myPlayer].mouseInterface = true;
                    if (UIUpgradeWorkbench.CanGoInSlot(Main.mouseItem, i))
                        ItemSlot.Handle(ref item, 3);
                }
                ItemSlot.Draw(spriteBatch, ref item, 3, new Vector2(x, y));
                switch (i) {
                    case 0:
                        DushyUpgrade.upgradeWorkbenchTE.upgradeItem = item;
                        break;
                    case 1:
                        DushyUpgrade.upgradeWorkbenchTE.upgradeStone = item;
                        break;
                    case 2:
                        DushyUpgrade.upgradeWorkbenchTE.protectionScroll = item;
                        break;
                    default:
                        break;
                }

                Main.spriteBatch.DrawString(Main.fontMouseText,
                UIUpgradeWorkbench.labels[i], new Vector2(x + 48, y + 10),
                Color.White, 0f, default(Vector2), Main.inventoryScale, SpriteEffects.None, 0f);
            }
            Main.inventoryScale = oldScale;
        }

        public static bool CanGoInSlot(Item item, int slotNumber) {
            if (item.IsAir)
                return true;

            if (slotNumber == 0) //First slot
                if (DushyUpgrade.IsUpgradable(item))
                    return true;
                else
                    return false;

            else if (slotNumber == 1) //Second slot
                return DushyUpgrade.upgradeStones.Contains(item.type);

            else //Last slot
                return DushyUpgrade.protectionScrolls.Contains(item.type);
        }
    }
}
