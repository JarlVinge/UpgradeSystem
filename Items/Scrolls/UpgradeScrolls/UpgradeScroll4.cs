using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace UpgradeSystem.Items.Scrolls.UpgradeScrolls {
    class UpgradeScroll4 : ModItem {
        public int scrollLevel = 4;
        public override void SetDefaults() {
            item.UseSound = SoundID.Item43;
            item.useStyle = 4;
            item.useTurn = true;
            item.useAnimation = 20;
            item.useTime = 20;
            item.maxStack = 99;
            item.consumable = true;
            item.value = 50000;
            item.rare = 4;
            return;
        }
        public override void SetStaticDefaults() {
            DisplayName.SetDefault("Upgrade Scroll +" + this.scrollLevel);
            Tooltip.SetDefault("Upgrades items to " + this.scrollLevel + ", Item level must be below scroll level.");
        }
        public override bool UseItem(Player player) {
            Item item = player.inventory[0];
            if (item.type > 0) {
                UpgradeInfo info = item.GetGlobalItem<UpgradeInfo>(mod);
                if ((!info.upgraded || info.level < this.scrollLevel) && !info.broken) {
                    info.UpgradeItemScroll(item, player, this.scrollLevel);
                    return true;
                }
                return false;
            }
            return false;
        }
    }
}
