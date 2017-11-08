using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace UpgradeSystem.Items.Scrolls {
    class RepairScroll : ModItem {
        public override void SetDefaults() {
            item.UseSound = SoundID.Item43;
            item.useStyle = 4;
            item.useTurn = true;
            item.useAnimation = 20;
            item.useTime = 20;
            item.maxStack = 999;
            item.consumable = true;
            item.value = 200000;
            item.rare = 6;
            return;
        }
        public override void SetStaticDefaults() {
            DisplayName.SetDefault("Scroll of Repair");
            Tooltip.SetDefault("Repairs your broken items and restores previous enhancements.");
        }
        public override bool UseItem(Player player) {
            Item item = player.inventory[0];
            if (item.type > 0) {
                UpgradeInfo info = item.GetGlobalItem<UpgradeInfo>(mod);
                if (info.broken) {
                    info.RepairItem(item, player);
                    return true;
                }
                return false;
            }
            return false;
        }
    }
}
