using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace UpgradeSystem.Items.Scrolls.UpgradeScrolls {
    class UpgradeScroll9 : ModItem {
        public int scrollLevel = 9;

        public override void SetDefaults() {
            item.UseSound = SoundID.Item43;
            item.useStyle = 4;
            item.useTurn = true;
            item.useAnimation = 20;
            item.useTime = 20;
            item.maxStack = 99;
            item.consumable = true;
            item.value = 10000000;
            item.rare = 9;
            return;
        }
        public override void SetStaticDefaults() {
            DisplayName.SetDefault("Upgrade Scroll +" + this.scrollLevel);
            Tooltip.SetDefault("Upgrades items to " + this.scrollLevel + ", Item level must be below scroll level.");
        }

        public override void AddRecipes() {
            ModRecipe recipe = new ModRecipe(mod);
            recipe.AddIngredient(null, "UpgradeScroll8", 1);
            recipe.AddIngredient(null, "UpgradeStone", 15);
            recipe.AddIngredient(ItemID.ChlorophyteBar, 30);
            recipe.AddIngredient(ItemID.ShroomiteBar, 30);
            recipe.AddIngredient(ItemID.SpectreBar, 30);
            recipe.AddIngredient(ItemID.BeetleHusk, 25);
            recipe.SetResult(this);
            recipe.AddRecipe();
        }
    }
}
