using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace DushyUpgrade.Items
{
	public class UpgradeStone : ModItem
	{
		public override void SetDefaults()
		{
            item.maxStack = 999;
            item.value = 3500;
            item.rare = 4;
            return;
        }
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Blessed Enhancement Stone");
            Tooltip.SetDefault("Used to upgrade weapons and armor.");
        }
    }
}
