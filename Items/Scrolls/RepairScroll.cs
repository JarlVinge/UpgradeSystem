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
            item.maxStack = 999;
            item.value = 200000;
            item.rare = 6;
            return;
        }
        public override void SetStaticDefaults() {
            DisplayName.SetDefault("Scroll of Repair");
            Tooltip.SetDefault("Repairs your broken items.");
        }
    }
}
