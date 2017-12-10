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
    class UIElementWorkbench {

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
            UIElementWorkbench.DrawPanel(spriteBatch, UIElementWorkbench._backgroundTexture, new Color(33, 43, 79) * 0.8f);
            UIElementWorkbench.DrawPanel(spriteBatch, UIElementWorkbench._borderTexture, new Color(89, 116, 213) * 0.7f);

            float oldScale = Main.inventoryScale;
            Main.inventoryScale = 0.75f;
            Point value = new Point(Main.mouseX, Main.mouseY);

            Main.spriteBatch.DrawString(Main.fontMouseText,
                "Upgrade Workbench", new Vector2(UIElementWorkbench.X + 33, UIElementWorkbench.Y + 10),
                Color.White, 0f, default(Vector2), Main.inventoryScale, SpriteEffects.None, 0f);

            Rectangle hitbox = new Rectangle(UIElementWorkbench.X + 66, UIElementWorkbench.Y + 180, 50, 18);
            Main.spriteBatch.DrawString(Main.fontMouseText,
                "Upgrade", new Vector2(UIElementWorkbench.X + 66, UIElementWorkbench.Y + 180),
                Color.White, 0f, default(Vector2), Main.inventoryScale, SpriteEffects.None, 0f);


            if (hitbox.Contains(value) && !PlayerInput.IgnoreMouseInterface && Main.mouseLeft && Main.mouseLeftRelease) {
                Main.player[Main.myPlayer].mouseInterface = true;
                if (DushyUpgrade.elementWorkbenchTE.itemToEnchant.IsAir) //First case empty
                    Main.NewText("You need an item to upgrade");
                else if (DushyUpgrade.elementWorkbenchTE.elementalScroll.IsAir) {
                    Main.NewText("You need an upgrade stone or an upgrade scroll to upgrade");
                }
                else {
                    if (!DushyUpgrade.IsItemBroken(DushyUpgrade.elementWorkbenchTE.itemToEnchant)) {
                        UpgradeInfo info = DushyUpgrade.elementWorkbenchTE.itemToEnchant.GetGlobalItem<UpgradeInfo>();
                        int result;
                        result = info.enchantItem(DushyUpgrade.elementWorkbenchTE.itemToEnchant);

                        DushyUpgrade.elementWorkbenchTE.elementalScroll.stack--;

                        ItemText.NewText(Main.player[Main.myPlayer], "Success !", Color.Green);
                    }
                }
            }
            for (int i = 0; i < 3; i++) {
                int x = UIElementWorkbench.X;
                int y = 42 * i + UIElementWorkbench.Y + 40;
                Rectangle r = new Rectangle(x, y, (int)((float)Main.inventoryBackTexture.Width * Main.inventoryScale), (int)((float)Main.inventoryBackTexture.Height * Main.inventoryScale));
                Item item = i == 0 ? DushyUpgrade.elementWorkbenchTE.itemToEnchant : DushyUpgrade.elementWorkbenchTE.elementalScroll;

                if (r.Contains(value) && !PlayerInput.IgnoreMouseInterface) {
                    Main.player[Main.myPlayer].mouseInterface = true;
                    if (UIElementWorkbench.CanGoInSlot(Main.mouseItem, i))
                        ItemSlot.Handle(ref item, 3);
                }
                ItemSlot.Draw(spriteBatch, ref item, 3, new Vector2(x, y));
                switch (i) {
                    case 0:
                        DushyUpgrade.elementWorkbenchTE.itemToEnchant = item;
                        break;
                    case 1:
                        DushyUpgrade.elementWorkbenchTE.elementalScroll = item;
                        break;
                    default:
                        break;
                }

                Main.spriteBatch.DrawString(Main.fontMouseText,
                UIElementWorkbench.labels[i], new Vector2(x + 48, y + 10),
                Color.White, 0f, default(Vector2), Main.inventoryScale, SpriteEffects.None, 0f);
            }
            Main.inventoryScale = oldScale;
        }

        public static void DrawPanel(SpriteBatch spriteBatch, Texture2D texture, Color color) {
            Point point = new Point(UIElementWorkbench.X - 8, UIElementWorkbench.Y);
            Point point2 = new Point(point.X + UIElementWorkbench.width - UIElementWorkbench.CORNER_SIZE, point.Y + UIElementWorkbench.height);
            int width = point2.X - point.X - UIElementWorkbench.CORNER_SIZE;
            int height = point2.Y - point.Y - UIElementWorkbench.CORNER_SIZE;
            spriteBatch.Draw(texture, new Rectangle(point.X, point.Y, UIElementWorkbench.CORNER_SIZE, UIElementWorkbench.CORNER_SIZE), new Rectangle?(new Rectangle(0, 0, UIElementWorkbench.CORNER_SIZE, UIElementWorkbench.CORNER_SIZE)), color);
            spriteBatch.Draw(texture, new Rectangle(point2.X, point.Y, UIElementWorkbench.CORNER_SIZE, UIElementWorkbench.CORNER_SIZE), new Rectangle?(new Rectangle(UIElementWorkbench.CORNER_SIZE + UIElementWorkbench.BAR_SIZE, 0, UIElementWorkbench.CORNER_SIZE, UIElementWorkbench.CORNER_SIZE)), color);
            spriteBatch.Draw(texture, new Rectangle(point.X, point2.Y, UIElementWorkbench.CORNER_SIZE, UIElementWorkbench.CORNER_SIZE), new Rectangle?(new Rectangle(0, UIElementWorkbench.CORNER_SIZE + UIElementWorkbench.BAR_SIZE, UIElementWorkbench.CORNER_SIZE, UIElementWorkbench.CORNER_SIZE)), color);
            spriteBatch.Draw(texture, new Rectangle(point2.X, point2.Y, UIElementWorkbench.CORNER_SIZE, UIElementWorkbench.CORNER_SIZE), new Rectangle?(new Rectangle(UIElementWorkbench.CORNER_SIZE + UIElementWorkbench.BAR_SIZE, UIElementWorkbench.CORNER_SIZE + UIElementWorkbench.BAR_SIZE, UIElementWorkbench.CORNER_SIZE, UIElementWorkbench.CORNER_SIZE)), color);
            spriteBatch.Draw(texture, new Rectangle(point.X + UIElementWorkbench.CORNER_SIZE, point.Y, width, UIElementWorkbench.CORNER_SIZE), new Rectangle?(new Rectangle(UIElementWorkbench.CORNER_SIZE, 0, UIElementWorkbench.BAR_SIZE, UIElementWorkbench.CORNER_SIZE)), color);
            spriteBatch.Draw(texture, new Rectangle(point.X + UIElementWorkbench.CORNER_SIZE, point2.Y, width, UIElementWorkbench.CORNER_SIZE), new Rectangle?(new Rectangle(UIElementWorkbench.CORNER_SIZE, UIElementWorkbench.CORNER_SIZE + UIElementWorkbench.BAR_SIZE, UIElementWorkbench.BAR_SIZE, UIElementWorkbench.CORNER_SIZE)), color);
            spriteBatch.Draw(texture, new Rectangle(point.X, point.Y + UIElementWorkbench.CORNER_SIZE, UIElementWorkbench.CORNER_SIZE, height), new Rectangle?(new Rectangle(0, UIElementWorkbench.CORNER_SIZE, UIElementWorkbench.CORNER_SIZE, UIElementWorkbench.BAR_SIZE)), color);
            spriteBatch.Draw(texture, new Rectangle(point2.X, point.Y + UIElementWorkbench.CORNER_SIZE, UIElementWorkbench.CORNER_SIZE, height), new Rectangle?(new Rectangle(UIElementWorkbench.CORNER_SIZE + UIElementWorkbench.BAR_SIZE, UIElementWorkbench.CORNER_SIZE, UIElementWorkbench.CORNER_SIZE, UIElementWorkbench.BAR_SIZE)), color);
            spriteBatch.Draw(texture, new Rectangle(point.X + UIElementWorkbench.CORNER_SIZE, point.Y + UIElementWorkbench.CORNER_SIZE, width, height), new Rectangle?(new Rectangle(UIElementWorkbench.CORNER_SIZE, UIElementWorkbench.CORNER_SIZE, UIElementWorkbench.BAR_SIZE, UIElementWorkbench.BAR_SIZE)), color);
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
