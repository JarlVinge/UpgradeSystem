﻿using Microsoft.Xna.Framework;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.ObjectData;

namespace UpgradeSystem.Tiles {
    public class ElementWorkbench : ModTile {
        public override void SetDefaults() {
            Main.tileSolidTop[Type] = true;
            Main.tileFrameImportant[Type] = true;
            Main.tileNoAttach[Type] = true;
            Main.tileTable[Type] = true;
            Main.tileLavaDeath[Type] = true;
            TileObjectData.newTile.CopyFrom(TileObjectData.Style3x2);
            TileObjectData.newTile.Width = 3;
            TileObjectData.newTile.Height = 2;
            TileObjectData.newTile.CoordinateHeights = new int[] { 16, 16, 18 };
            TileObjectData.newTile.HookPostPlaceMyPlayer = new PlacementHook(mod.GetTileEntity<ElementWorkbenchTE>().Hook_AfterPlacement, -1, 0, true);
            TileObjectData.addTile(Type);
            AddToArray(ref TileID.Sets.RoomNeeds.CountsAsTable);
            ModTranslation name = CreateMapEntryName();
            name.SetDefault("Element Workbench");
            AddMapEntry(new Color(200, 200, 200), name);
            //dustType = mod.DustType("Sparkle");
            disableSmartCursor = true;
            adjTiles = new int[] { TileID.WorkBenches };
        }

        public override void NumDust(int i, int j, bool fail, ref int num) {
            num = fail ? 1 : 3;
        }

        public override void KillMultiTile(int i, int j, int frameX, int frameY) {
            Item.NewItem(i * 16, j * 16, 32, 16, mod.ItemType("ElementWorkbench"));
        }

        public override void RightClick(int i, int j) {
            Player player = Main.LocalPlayer;
            Tile tile = Main.tile[i, j];
            int left = i - (tile.frameX % 54 / 18);
            int top = j - (tile.frameY / 18);
            Main.mouseRightRelease = false;
            if (player.sign >= 0) {
                Main.PlaySound(SoundID.MenuClose);
                player.sign = -1;
                Main.editSign = false;
                Main.npcChatText = "";
            }
            if (Main.editChest) {
                Main.PlaySound(SoundID.MenuTick);
                Main.editChest = false;
                Main.npcChatText = "";
            }
            if (player.editedChestName) {
                NetMessage.SendData(33, -1, -1, Terraria.Localization.NetworkText.FromLiteral(Main.chest[player.chest].name), player.chest, 1f, 0f, 0f, 0, 0, 0);
                player.editedChestName = false;
            }

            int index = mod.GetTileEntity<ElementWorkbenchTE>().Find(left, top);
            if (index == -1) {
                return;
            }
            ElementWorkbenchTE bookcaseTE = (ElementWorkbenchTE)TileEntity.ByID[index];
            DushyUpgrade.elementWorkbenchTE = bookcaseTE;

            Main.playerInventory = true;
            Main.LocalPlayer.chest = -1;
        }
    }

    public class ElementWorkbenchTE : ModTileEntity {
        internal Item itemToEnchant;
        internal Item elementalScroll;

        public ElementWorkbenchTE() {
            itemToEnchant = new Item();
            elementalScroll = new Item();
        }


        public override void Update() {
            if (this == DushyUpgrade.elementWorkbenchTE) {
                if (Main.LocalPlayer.chest != -1 || !Main.playerInventory || Vector2.Distance(Position.ToWorldCoordinates(), Main.LocalPlayer.Center) > 100) {
                    DushyUpgrade.elementWorkbenchTE = null;
                }
            }
            // auto close ui?
        }

        public override bool ValidTile(int i, int j) {
            Tile tile = Main.tile[i, j];
            return tile.active() && tile.type == mod.TileType<ElementWorkbench>() && tile.frameX % 54 == 0 && tile.frameY == 0;
        }

        public override int Hook_AfterPlacement(int i, int j, int type, int style, int direction) {
            //Main.NewText("i " + i + " j " + j + " t " + type + " s " + style + " d " + direction);
            if (Main.netMode == 1) {
                NetMessage.SendTileSquare(Main.myPlayer, i, j, 3);
                NetMessage.SendData(87, -1, -1, null, i, j, Type, 0f, 0, 0, 0);
                return -1;
            }
            int num = Place(i, j);
            // SetSize
            return num;
        }

        public override void OnNetPlace() {
            // SetSize
        }

        public override void OnKill() {
            if (Main.netMode != 1) {
                //for (int i = 0; i < 6; i++)
                //{
                //	Item.NewItem((int)(this.Position.X * 16) + i, (int)(this.Position.Y * 16), 32, 32, items[i].netID, 1, false, (int)items[i].prefix, false, false);
                //}
            }
        }
    }
}