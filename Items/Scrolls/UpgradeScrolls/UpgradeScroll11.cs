using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace UpgradeSystem.Items.Scrolls.UpgradeScrolls {
    class UpgradeScroll11 : ModItem {

        public int scrollLevel = 11;

        public override void SetDefaults() {
            item.UseSound = SoundID.Item43;
            item.useStyle = 4;
            item.useTurn = true;
            item.useAnimation = 20;
            item.useTime = 20;
            item.maxStack = 99;
            item.consumable = true;
            item.value = 100000000;
            item.rare = 11;
            return;
        }
        public override void SetStaticDefaults() {
            DisplayName.SetDefault("Upgrade Scroll +" + this.scrollLevel);
            Tooltip.SetDefault("Upgrades items to " + this.scrollLevel + ", Item level must be below scroll level.");
        }

        public override void AddRecipes() {
            ModRecipe recipe = new ModRecipe(mod);
            recipe.AddIngredient(null, "UpgradeScroll10", 2);
            recipe.AddIngredient(null, "UpgradeStone", 40);
            recipe.AddIngredient(ItemID.FragmentSolar, 40);
            recipe.AddIngredient(ItemID.FragmentNebula, 40);
            recipe.AddIngredient(ItemID.FragmentVortex, 40);
            recipe.AddIngredient(ItemID.FragmentStardust, 40);
            recipe.AddIngredient(ItemID.LunarBar, 50);
            recipe.SetResult(this);
            recipe.AddRecipe();
        }
    }
}
