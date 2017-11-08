using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace UpgradeSystem.Items.Orbs {
    class OrbLifeSteal : ModItem {
        public override void SetDefaults() {
            item.UseSound = SoundID.Item43;
            item.useStyle = 4;
            item.useTurn = true;
            item.useAnimation = 20;
            item.useTime = 20;
            item.maxStack = 99;
            item.consumable = true;
            item.value = 500000;
            item.rare = 9;
            return;
        }
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Life Steal Pearl");
            Tooltip.SetDefault("Hitting an enemy has a chance to steal life.");
        }
        public override bool UseItem(Player player) {
            Item item = player.inventory[0];
            if (item.type > 0 && (item.magic || item.melee || item.summon || item.ranged)) {
                UpgradeInfo info = item.GetGlobalItem<UpgradeInfo>(mod);
                int socketNumber = info.getNumberOfSocket();
                if (socketNumber > 0) {
                    if (!info.socketAvailable())
                        return false;
                    info.AddIntoSocket("+5% Life Steal", new Color(255, 0, 0), info);
                    return true;
                }
                return false;
            }
            return false;
        }
    }
}
