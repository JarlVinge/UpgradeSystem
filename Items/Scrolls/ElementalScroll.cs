using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace UpgradeSystem.Items.Scrolls {
    class ElementalScroll : ModItem {
        public override void SetDefaults() {
            item.UseSound = SoundID.Item43;
            item.useStyle = 4;
            item.useTurn = true;
            item.useAnimation = 20;
            item.useTime = 20;
            item.maxStack = 99;
            item.consumable = true;
            item.value = 1000000;
            item.rare = 4;
            return;
        }

        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Scroll of the 11 Elements");
            Tooltip.SetDefault("A special scroll who randomly add element to your item.");
        }
        public override bool UseItem(Player player) {
            Item item = player.inventory[0];
            if (item.type > 0) {
                UpgradeInfo info = item.GetGlobalItem<UpgradeInfo>(mod);
                if (info.elemented && info.level >= 10) {
                    info.ChangeItemElement(item, info);
                    return true;
                }
                return false;
            }
            return false;
        }
    }
}
