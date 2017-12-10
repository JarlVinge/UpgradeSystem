using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace UpgradeSystem.Items.Scrolls.UpgradeScrolls {
    class UpgradeScroll5 : ModItem {
        public int scrollLevel = 5;

        public override void SetDefaults() {
            item.UseSound = SoundID.Item43;
            item.useStyle = 4;
            item.useTurn = true;
            item.useAnimation = 20;
            item.useTime = 20;
            item.maxStack = 99;
            item.consumable = true;
            item.value = 100000;
            item.rare = 5;
            return;
        }

        public override void SetStaticDefaults() {
            DisplayName.SetDefault("Upgrade Scroll +" + this.scrollLevel);
            Tooltip.SetDefault("Upgrades items to " + this.scrollLevel + ", Item level must be below scroll level.");
        }

        public override void AddRecipes() {
            ModRecipe recipe = new ModRecipe(mod);
            recipe.AddIngredient(null, "UpgradeScroll4", 1);
            recipe.AddIngredient(null, "UpgradeStone", 2);
            recipe.AddIngredient(ItemID.FallenStar, 4);
            recipe.AddIngredient(ItemID.ShadowScale, 10);
            recipe.SetResult(this);
            recipe.AddRecipe();

            recipe = new ModRecipe(mod);
            recipe.AddIngredient(null, "UpgradeScroll4", 1);
            recipe.AddIngredient(null, "UpgradeStone", 2);
            recipe.AddIngredient(ItemID.FallenStar, 4);
            recipe.AddIngredient(ItemID.TissueSample, 10);
            recipe.SetResult(this);
            recipe.AddRecipe();
        }
    }
}
