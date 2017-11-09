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

namespace UpgradeSystem.UI {
    class UIRepairWorkbench {

        private static string[] labels = new string[] { "Broken item", "Scroll of repair" };
        private static Texture2D _backgroundTexture = TextureManager.Load("Images/UI/PanelBackground");
        private static Texture2D _borderTexture = TextureManager.Load("Images/UI/PanelBorder");
        private static int CORNER_SIZE = 12;
        private static int BAR_SIZE = 4;
        private static int X = 167;
        private static int Y = Main.instance.invBottom + 44;
        private static int width = 200;
        private static int height = 210;

        public static void DrawSelf(SpriteBatch spriteBatch) {
            UIRepairWorkbench.DrawPanel(spriteBatch, UIRepairWorkbench._backgroundTexture, new Color(33, 43, 79) * 0.8f);
            UIRepairWorkbench.DrawPanel(spriteBatch, UIRepairWorkbench._borderTexture, new Color(89, 116, 213) * 0.7f);

            float oldScale = Main.inventoryScale;
            Main.inventoryScale = 0.75f;
            Point value = new Point(Main.mouseX, Main.mouseY);

            Main.spriteBatch.DrawString(Main.fontMouseText,
                "Repair Workbench", new Vector2(UIRepairWorkbench.X + 33, UIRepairWorkbench.Y + 10),
                Color.White, 0f, default(Vector2), Main.inventoryScale, SpriteEffects.None, 0f);

            Rectangle hitbox = new Rectangle(UIRepairWorkbench.X + 66, UIRepairWorkbench.Y + 180, 50, 18);
            Main.spriteBatch.DrawString(Main.fontMouseText,
                "Repair", new Vector2(UIRepairWorkbench.X + 66, UIRepairWorkbench.Y + 180),
                Color.White, 0f, default(Vector2), Main.inventoryScale, SpriteEffects.None, 0f);


            if (hitbox.Contains(value) && !PlayerInput.IgnoreMouseInterface && Main.mouseLeft && Main.mouseLeftRelease) {
                Main.player[Main.myPlayer].mouseInterface = true;

                if (DushyUpgrade.repairWorkbenchTE.brokenItem.IsAir)
                    Main.NewText("You need an item to repair");
                else if (DushyUpgrade.repairWorkbenchTE.repairScroll.IsAir) {
                    Main.NewText("You need a repair scroll to repair your " + DushyUpgrade.repairWorkbenchTE.brokenItem.Name);
                }
                else {
                    UpgradeInfo info = DushyUpgrade.repairWorkbenchTE.brokenItem.GetGlobalItem<UpgradeInfo>();
                    info.RepairItem(DushyUpgrade.repairWorkbenchTE.brokenItem);
                    DushyUpgrade.repairWorkbenchTE.repairScroll.stack--;
                    ItemText.NewText(Main.player[Main.myPlayer], "Restored !", Color.Gold);
                }
            }
            for (int i = 0; i < 2; i++) {
                int x = UIRepairWorkbench.X;
                int y = 42 * i + UIRepairWorkbench.Y + 40;
                Rectangle r = new Rectangle(x, y, (int)((float)Main.inventoryBackTexture.Width * Main.inventoryScale), (int)((float)Main.inventoryBackTexture.Height * Main.inventoryScale));
                Item item = i == 0 ? DushyUpgrade.repairWorkbenchTE.brokenItem : DushyUpgrade.repairWorkbenchTE.repairScroll;

                if (r.Contains(value) && !PlayerInput.IgnoreMouseInterface) {
                    Main.player[Main.myPlayer].mouseInterface = true;
                    if (UIRepairWorkbench.CanGoInSlot(Main.mouseItem, i))
                        ItemSlot.Handle(ref item, 3);
                }
                ItemSlot.Draw(spriteBatch, ref item, 3, new Vector2(x, y));
                switch (i) {
                    case 0:
                        DushyUpgrade.repairWorkbenchTE.brokenItem = item;
                        break;
                    case 1:
                        DushyUpgrade.repairWorkbenchTE.repairScroll = item;
                        break;
                    default:
                        break;
                }

                Main.spriteBatch.DrawString(Main.fontMouseText,
                UIRepairWorkbench.labels[i], new Vector2(x + 48, y + 10),
                Color.White, 0f, default(Vector2), Main.inventoryScale, SpriteEffects.None, 0f);
            }
            Main.inventoryScale = oldScale;
        }

        public static void DrawPanel(SpriteBatch spriteBatch, Texture2D texture, Color color) {
            Point point = new Point(UIRepairWorkbench.X - 8, UIRepairWorkbench.Y);
            Point point2 = new Point(point.X + UIRepairWorkbench.width - UIRepairWorkbench.CORNER_SIZE, point.Y + UIRepairWorkbench.height);
            int width = point2.X - point.X - UIRepairWorkbench.CORNER_SIZE;
            int height = point2.Y - point.Y - UIRepairWorkbench.CORNER_SIZE;
            spriteBatch.Draw(texture, new Rectangle(point.X, point.Y, UIRepairWorkbench.CORNER_SIZE, UIRepairWorkbench.CORNER_SIZE), new Rectangle?(new Rectangle(0, 0, UIRepairWorkbench.CORNER_SIZE, UIRepairWorkbench.CORNER_SIZE)), color);
            spriteBatch.Draw(texture, new Rectangle(point2.X, point.Y, UIRepairWorkbench.CORNER_SIZE, UIRepairWorkbench.CORNER_SIZE), new Rectangle?(new Rectangle(UIRepairWorkbench.CORNER_SIZE + UIRepairWorkbench.BAR_SIZE, 0, UIRepairWorkbench.CORNER_SIZE, UIRepairWorkbench.CORNER_SIZE)), color);
            spriteBatch.Draw(texture, new Rectangle(point.X, point2.Y, UIRepairWorkbench.CORNER_SIZE, UIRepairWorkbench.CORNER_SIZE), new Rectangle?(new Rectangle(0, UIRepairWorkbench.CORNER_SIZE + UIRepairWorkbench.BAR_SIZE, UIRepairWorkbench.CORNER_SIZE, UIRepairWorkbench.CORNER_SIZE)), color);
            spriteBatch.Draw(texture, new Rectangle(point2.X, point2.Y, UIRepairWorkbench.CORNER_SIZE, UIRepairWorkbench.CORNER_SIZE), new Rectangle?(new Rectangle(UIRepairWorkbench.CORNER_SIZE + UIRepairWorkbench.BAR_SIZE, UIRepairWorkbench.CORNER_SIZE + UIRepairWorkbench.BAR_SIZE, UIRepairWorkbench.CORNER_SIZE, UIRepairWorkbench.CORNER_SIZE)), color);
            spriteBatch.Draw(texture, new Rectangle(point.X + UIRepairWorkbench.CORNER_SIZE, point.Y, width, UIRepairWorkbench.CORNER_SIZE), new Rectangle?(new Rectangle(UIRepairWorkbench.CORNER_SIZE, 0, UIRepairWorkbench.BAR_SIZE, UIRepairWorkbench.CORNER_SIZE)), color);
            spriteBatch.Draw(texture, new Rectangle(point.X + UIRepairWorkbench.CORNER_SIZE, point2.Y, width, UIRepairWorkbench.CORNER_SIZE), new Rectangle?(new Rectangle(UIRepairWorkbench.CORNER_SIZE, UIRepairWorkbench.CORNER_SIZE + UIRepairWorkbench.BAR_SIZE, UIRepairWorkbench.BAR_SIZE, UIRepairWorkbench.CORNER_SIZE)), color);
            spriteBatch.Draw(texture, new Rectangle(point.X, point.Y + UIRepairWorkbench.CORNER_SIZE, UIRepairWorkbench.CORNER_SIZE, height), new Rectangle?(new Rectangle(0, UIRepairWorkbench.CORNER_SIZE, UIRepairWorkbench.CORNER_SIZE, UIRepairWorkbench.BAR_SIZE)), color);
            spriteBatch.Draw(texture, new Rectangle(point2.X, point.Y + UIRepairWorkbench.CORNER_SIZE, UIRepairWorkbench.CORNER_SIZE, height), new Rectangle?(new Rectangle(UIRepairWorkbench.CORNER_SIZE + UIRepairWorkbench.BAR_SIZE, UIRepairWorkbench.CORNER_SIZE, UIRepairWorkbench.CORNER_SIZE, UIRepairWorkbench.BAR_SIZE)), color);
            spriteBatch.Draw(texture, new Rectangle(point.X + UIRepairWorkbench.CORNER_SIZE, point.Y + UIRepairWorkbench.CORNER_SIZE, width, height), new Rectangle?(new Rectangle(UIRepairWorkbench.CORNER_SIZE, UIRepairWorkbench.CORNER_SIZE, UIRepairWorkbench.BAR_SIZE, UIRepairWorkbench.BAR_SIZE)), color);
        }

        public static bool CanGoInSlot(Item item, int slotNumber) {
            if (item.IsAir)
                return true;

            if (slotNumber == 0)
                if (DushyUpgrade.IsItemBroken(item))
                    return true;
                else
                    return false;

            else
                return DushyUpgrade.repairScrolls.Contains(item.type);
        }
    }
}
