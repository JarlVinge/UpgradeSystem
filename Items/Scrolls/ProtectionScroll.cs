using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace DushyUpgrade.Items
{
    class ProtectionScroll : ModItem
    {
        public override void SetDefaults()
        {
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
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Scroll of Protection");
            Tooltip.SetDefault("Protects your items from being reset or loosing levels.");
        }
        public override bool UseItem(Player player)
        {
            UpgradePlayer thePlayer = player.GetModPlayer<UpgradePlayer>(mod);
            if (thePlayer.protectingStone)
                return false;

            thePlayer.protectingStone = true;
            return true;

        }
    }
}
