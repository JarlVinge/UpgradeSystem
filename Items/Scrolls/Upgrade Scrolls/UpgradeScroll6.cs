﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace DushyUpgrade.Items
{
    class UpgradeScroll6 : ModItem
    {
        public int scrollLevel = 6;

        public override void SetDefaults()
        {
            item.UseSound = SoundID.Item43;
            item.useStyle = 4;
            item.useTurn = true;
            item.useAnimation = 20;
            item.useTime = 20;
            item.maxStack = 99;
            item.consumable = true;
            item.value = 500000;
            item.rare = 6;
            return;
        }

        public override void SetStaticDefaults()
        {
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

        public override void AddRecipes() {
            ModRecipe recipe = new ModRecipe(mod);
            recipe.AddIngredient(null, "UpgradeScroll5", 5);
            recipe.SetResult(this);
            recipe.AddRecipe();
        }
    }
}
