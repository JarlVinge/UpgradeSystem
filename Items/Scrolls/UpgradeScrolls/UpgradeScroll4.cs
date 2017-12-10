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

        public override void AddRecipes() {
            ModRecipe recipe = new ModRecipe(mod);
            recipe.AddIngredient(null, "UpgradeStone", 1);
            recipe.AddIngredient(ItemID.FallenStar, 2);
            recipe.AddIngredient(ItemID.Silk, 5);
            recipe.SetResult(this);
            recipe.AddRecipe();
        }
    }
}
