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
    class UIRepairWorkbench {

        private static string[] labels = new string[] { "Item to Repair", "Repair Scroll" };

        public static void DrawSelf(SpriteBatch spriteBatch) {
            float oldScale = Main.inventoryScale;
            Main.inventoryScale = 0.75f;
            Point value = new Point(Main.mouseX, Main.mouseY);

            Rectangle hitbox = new Rectangle(167 - 8, Main.instance.invBottom + 44, 200, 220);
            Main.spriteBatch.Draw(Main.magicPixel, hitbox, Color.Gray);

            Main.spriteBatch.DrawString(Main.fontMouseText,
                "Repair Workbench", new Vector2(167 + 33, Main.instance.invBottom + 54),
                Color.White, 0f, default(Vector2), Main.inventoryScale, SpriteEffects.None, 0f);

            hitbox = new Rectangle(167 + 40, Main.instance.invBottom + 44 + 170, 100, 40);
            Main.spriteBatch.Draw(Main.magicPixel, hitbox, Color.Black);
            Main.spriteBatch.DrawString(Main.fontMouseText,
                "Repair", new Vector2(167 + 66, Main.instance.invBottom + 44 + 180),
                Color.White, 0f, default(Vector2), Main.inventoryScale, SpriteEffects.None, 0f);

            if (hitbox.Contains(value) && !PlayerInput.IgnoreMouseInterface && Main.mouseLeft && Main.mouseLeftRelease) {
                Main.player[Main.myPlayer].mouseInterface = true;
                if (DushyUpgrade.IsInventoryFull(Main.player[Main.myPlayer])) {

                }
                if (DushyUpgrade.repairWorkbenchTE.brokenItem.IsAir) //First case empty
                    Main.NewText("You need an item to repair.");
                else if (DushyUpgrade.repairWorkbenchTE.repairScroll.IsAir) {
                    Main.NewText("You need a repair scroll.");
                }
                else {
                    UpgradeInfo info = DushyUpgrade.repairWorkbenchTE.brokenItem.GetGlobalItem<UpgradeInfo>();
                    info.RepairItem(DushyUpgrade.repairWorkbenchTE.brokenItem, Main.player[Main.myPlayer]);
                    DushyUpgrade.repairWorkbenchTE.brokenItem.stack--;
                }
            }
            for (int i = 0; i < 2; i++) {
                int x = 167;
                int y = 42 * i + Main.instance.invBottom + 84;
                int width = (int)((float)Main.inventoryBackTexture.Width * Main.inventoryScale);
                int height = (int)((float)Main.inventoryBackTexture.Height * Main.inventoryScale);
                Rectangle r = new Rectangle(x, y, width, height);

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

        public static bool CanGoInSlot(Item item, int slotNumber) {
            if (item.IsAir)
                return true;

            if (slotNumber == 0) //First slot
                if (DushyUpgrade.IsItemBroken(item))
                    return true;
                else
                    return false;

            else//Second slot
                return DushyUpgrade.repairScrolls.Contains(item.type);
        }
    }
}
