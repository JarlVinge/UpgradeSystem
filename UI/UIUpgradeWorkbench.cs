using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using ReLogic.Graphics;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.GameInput;
using Terraria.UI;
using Terraria.GameContent.UI.Elements;
using Terraria.Graphics;
using System.Diagnostics;

namespace UpgradeSystem.UI {
    class UIUpgradeWorkbench {

        private static string[] labels = new string[] { "Item", "Upgrade Stone/Scroll", "Scroll of Protection" };
        private static Texture2D _backgroundTexture = TextureManager.Load("Images/UI/PanelBackground");
        private static Texture2D _borderTexture = TextureManager.Load("Images/UI/PanelBorder");
        private static int CORNER_SIZE = 12;
        private static int BAR_SIZE = 4;
        private static int X = 167;
        private static int Y = Main.instance.invBottom + 44;
        private static int width = 200;
        private static int height = 210;

        public static void DrawSelf(SpriteBatch spriteBatch) {
            UIUpgradeWorkbench.DrawPanel(spriteBatch, UIUpgradeWorkbench._backgroundTexture, new Color(33, 43, 79) * 0.8f);
            UIUpgradeWorkbench.DrawPanel(spriteBatch, UIUpgradeWorkbench._borderTexture, new Color(89, 116, 213) * 0.7f);

            float oldScale = Main.inventoryScale;
            Main.inventoryScale = 0.75f;
            Point value = new Point(Main.mouseX, Main.mouseY);

            Main.spriteBatch.DrawString(Main.fontMouseText,
                "Upgrade Workbench", new Vector2(UIUpgradeWorkbench.X + 33, UIUpgradeWorkbench.Y + 10),
                Color.White, 0f, default(Vector2), Main.inventoryScale, SpriteEffects.None, 0f);

            Rectangle hitbox = new Rectangle(UIUpgradeWorkbench.X + 66, UIUpgradeWorkbench.Y + 180, 50, 18);
            Main.spriteBatch.DrawString(Main.fontMouseText,
                "Upgrade", new Vector2(UIUpgradeWorkbench.X + 66, UIUpgradeWorkbench.Y + 180),
                Color.White, 0f, default(Vector2), Main.inventoryScale, SpriteEffects.None, 0f);
            

            if (hitbox.Contains(value) && !PlayerInput.IgnoreMouseInterface && Main.mouseLeft && Main.mouseLeftRelease) {
                Main.player[Main.myPlayer].mouseInterface = true;
                if (DushyUpgrade.upgradeWorkbenchTE.itemToUpgrade.IsAir) //First case empty
                    Main.NewText("You need an item to upgrade");
                else if (DushyUpgrade.upgradeWorkbenchTE.upgradeMaterial.IsAir) {
                    Main.NewText("You need an upgrade stone or an upgrade scroll to upgrade");
                }
                else {
                    Boolean isProtected = !DushyUpgrade.upgradeWorkbenchTE.protectionScroll.IsAir;
                    if (!DushyUpgrade.IsItemBroken(DushyUpgrade.upgradeWorkbenchTE.itemToUpgrade)) {
                        UpgradeInfo info = DushyUpgrade.upgradeWorkbenchTE.itemToUpgrade.GetGlobalItem<UpgradeInfo>();
                        int result;
                        if (DushyUpgrade.upgradeScrolls.Contains(DushyUpgrade.upgradeWorkbenchTE.upgradeMaterial.type))
                            result = info.upgradeWithScroll(DushyUpgrade.upgradeWorkbenchTE.itemToUpgrade, DushyUpgrade.upgradeWorkbenchTE.upgradeMaterial);
                        else
                            result = info.upgrade(DushyUpgrade.upgradeWorkbenchTE.itemToUpgrade, isProtected);

                        DushyUpgrade.upgradeWorkbenchTE.upgradeMaterial.stack--;
                        if (isProtected)
                            DushyUpgrade.upgradeWorkbenchTE.protectionScroll.stack--;

                        if (result == DushyUpgrade.SUCCESS) {
                            ItemText.NewText(Main.player[Main.myPlayer], "Success !", Color.Green);
                            if (DushyUpgrade.IsItemMaxLevel(DushyUpgrade.upgradeWorkbenchTE.itemToUpgrade)) {
                                ItemText.NewText(Main.player[Main.myPlayer], "Level Max !", Color.LightSkyBlue);
                                Player player = Main.player[Main.myPlayer];
                                Item item = DushyUpgrade.upgradeWorkbenchTE.itemToUpgrade;
                                item.position = player.Center;
                                Item leftoverItem = player.GetItem(player.whoAmI, DushyUpgrade.upgradeWorkbenchTE.itemToUpgrade, false, true);
                                if (leftoverItem.stack > 0) {
                                    int num = Item.NewItem((int)player.position.X, (int)player.position.Y, player.width, player.height, leftoverItem.type, leftoverItem.stack, false, (int)leftoverItem.prefix, true, false);
                                    Main.item[num] = leftoverItem.Clone();
                                    Main.item[num].newAndShiny = false;
                                    if (Main.netMode == 1) {
                                        NetMessage.SendData(21, -1, -1, null, num, 1f, 0f, 0f, 0, 0, 0);
                                    }
                                }
                                DushyUpgrade.upgradeWorkbenchTE.itemToUpgrade.TurnToAir();
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
                int x = UIUpgradeWorkbench.X;
                int y = 42 * i + UIUpgradeWorkbench.Y + 40;
                Rectangle r = new Rectangle(x, y, (int)((float)Main.inventoryBackTexture.Width * Main.inventoryScale), (int)((float)Main.inventoryBackTexture.Height * Main.inventoryScale));
                Item item = i == 0 ? DushyUpgrade.upgradeWorkbenchTE.itemToUpgrade : (i == 1 ? DushyUpgrade.upgradeWorkbenchTE.upgradeMaterial : DushyUpgrade.upgradeWorkbenchTE.protectionScroll);

                if (r.Contains(value) && !PlayerInput.IgnoreMouseInterface) {
                    Main.player[Main.myPlayer].mouseInterface = true;
                    if (UIUpgradeWorkbench.CanGoInSlot(Main.mouseItem, i))
                        ItemSlot.Handle(ref item, 3);
                }
                ItemSlot.Draw(spriteBatch, ref item, 3, new Vector2(x, y));
                switch (i) {
                    case 0:
                        DushyUpgrade.upgradeWorkbenchTE.itemToUpgrade = item;
                        break;
                    case 1:
                        DushyUpgrade.upgradeWorkbenchTE.upgradeMaterial = item;
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

        public static void DrawPanel(SpriteBatch spriteBatch, Texture2D texture, Color color) {
            Point point = new Point(UIUpgradeWorkbench.X - 8, UIUpgradeWorkbench.Y);
            Point point2 = new Point(point.X + UIUpgradeWorkbench.width - UIUpgradeWorkbench.CORNER_SIZE, point.Y + UIUpgradeWorkbench.height);
            int width = point2.X - point.X - UIUpgradeWorkbench.CORNER_SIZE;
            int height = point2.Y - point.Y - UIUpgradeWorkbench.CORNER_SIZE;
            spriteBatch.Draw(texture, new Rectangle(point.X, point.Y, UIUpgradeWorkbench.CORNER_SIZE, UIUpgradeWorkbench.CORNER_SIZE), new Rectangle?(new Rectangle(0, 0, UIUpgradeWorkbench.CORNER_SIZE, UIUpgradeWorkbench.CORNER_SIZE)), color);
            spriteBatch.Draw(texture, new Rectangle(point2.X, point.Y, UIUpgradeWorkbench.CORNER_SIZE, UIUpgradeWorkbench.CORNER_SIZE), new Rectangle?(new Rectangle(UIUpgradeWorkbench.CORNER_SIZE + UIUpgradeWorkbench.BAR_SIZE, 0, UIUpgradeWorkbench.CORNER_SIZE, UIUpgradeWorkbench.CORNER_SIZE)), color);
            spriteBatch.Draw(texture, new Rectangle(point.X, point2.Y, UIUpgradeWorkbench.CORNER_SIZE, UIUpgradeWorkbench.CORNER_SIZE), new Rectangle?(new Rectangle(0, UIUpgradeWorkbench.CORNER_SIZE + UIUpgradeWorkbench.BAR_SIZE, UIUpgradeWorkbench.CORNER_SIZE, UIUpgradeWorkbench.CORNER_SIZE)), color);
            spriteBatch.Draw(texture, new Rectangle(point2.X, point2.Y, UIUpgradeWorkbench.CORNER_SIZE, UIUpgradeWorkbench.CORNER_SIZE), new Rectangle?(new Rectangle(UIUpgradeWorkbench.CORNER_SIZE + UIUpgradeWorkbench.BAR_SIZE, UIUpgradeWorkbench.CORNER_SIZE + UIUpgradeWorkbench.BAR_SIZE, UIUpgradeWorkbench.CORNER_SIZE, UIUpgradeWorkbench.CORNER_SIZE)), color);
            spriteBatch.Draw(texture, new Rectangle(point.X + UIUpgradeWorkbench.CORNER_SIZE, point.Y, width, UIUpgradeWorkbench.CORNER_SIZE), new Rectangle?(new Rectangle(UIUpgradeWorkbench.CORNER_SIZE, 0, UIUpgradeWorkbench.BAR_SIZE, UIUpgradeWorkbench.CORNER_SIZE)), color);
            spriteBatch.Draw(texture, new Rectangle(point.X + UIUpgradeWorkbench.CORNER_SIZE, point2.Y, width, UIUpgradeWorkbench.CORNER_SIZE), new Rectangle?(new Rectangle(UIUpgradeWorkbench.CORNER_SIZE, UIUpgradeWorkbench.CORNER_SIZE + UIUpgradeWorkbench.BAR_SIZE, UIUpgradeWorkbench.BAR_SIZE, UIUpgradeWorkbench.CORNER_SIZE)), color);
            spriteBatch.Draw(texture, new Rectangle(point.X, point.Y + UIUpgradeWorkbench.CORNER_SIZE, UIUpgradeWorkbench.CORNER_SIZE, height), new Rectangle?(new Rectangle(0, UIUpgradeWorkbench.CORNER_SIZE, UIUpgradeWorkbench.CORNER_SIZE, UIUpgradeWorkbench.BAR_SIZE)), color);
            spriteBatch.Draw(texture, new Rectangle(point2.X, point.Y + UIUpgradeWorkbench.CORNER_SIZE, UIUpgradeWorkbench.CORNER_SIZE, height), new Rectangle?(new Rectangle(UIUpgradeWorkbench.CORNER_SIZE + UIUpgradeWorkbench.BAR_SIZE, UIUpgradeWorkbench.CORNER_SIZE, UIUpgradeWorkbench.CORNER_SIZE, UIUpgradeWorkbench.BAR_SIZE)), color);
            spriteBatch.Draw(texture, new Rectangle(point.X + UIUpgradeWorkbench.CORNER_SIZE, point.Y + UIUpgradeWorkbench.CORNER_SIZE, width, height), new Rectangle?(new Rectangle(UIUpgradeWorkbench.CORNER_SIZE, UIUpgradeWorkbench.CORNER_SIZE, UIUpgradeWorkbench.BAR_SIZE, UIUpgradeWorkbench.BAR_SIZE)), color);
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
                return DushyUpgrade.upgradeStones.Contains(item.type) || DushyUpgrade.upgradeScrolls.Contains(item.type);

            else //Last slot
                return DushyUpgrade.protectionScrolls.Contains(item.type);
        }
    }
}
