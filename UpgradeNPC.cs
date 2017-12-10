using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace UpgradeSystem {
    class UpgradeNPC : GlobalNPC {
        public override void NPCLoot(NPC npc) {
            if (Main.rand.Next(10000) < 250) { //2.5% UpgradeStone
                Item.NewItem((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height, mod.ItemType("UpgradeStone"));
            }
        }
    }
}
