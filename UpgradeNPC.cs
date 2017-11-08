using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace DushyUpgrade {
    class UpgradeNPC : GlobalNPC {
        public override void NPCLoot(NPC npc) {
            if (Main.rand.Next(10000) < 250) { //2.5% UpgradeStone
                Item.NewItem((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height, mod.ItemType("UpgradeStone"));
            }
            if (Main.rand.Next(10000) < 100) { //1% UpgradeScroll4
                Item.NewItem((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height, mod.ItemType("UpgradeScroll4"));
            }
            if (Main.rand.Next(10000) < 20) { //0.2% UpgradeScroll5
                Item.NewItem((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height, mod.ItemType("UpgradeScroll5"));
            }
            if (Main.rand.Next(10000) < 10) { //0.1% UpgradeScroll6
                Item.NewItem((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height, mod.ItemType("UpgradeScroll6"));
            }
        }
    }
}
