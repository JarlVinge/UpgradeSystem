using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace UpgradeSystem.Items.Scrolls.UpgradeScrolls {
    class UpgradeScroll6 : ModItem {
        public int scrollLevel = 6;

        public override void SetDefaults() {
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

        public override void SetStaticDefaults() {
            DisplayName.SetDefault("Upgrade Scroll +" + this.scrollLevel);
            Tooltip.SetDefault("Upgrades items to " + this.scrollLevel + ", Item level must be below scroll level.");
        }

        public override void AddRecipes() {
            ModRecipe recipe = new ModRecipe(mod);
            recipe.AddIngredient(null, "UpgradeScroll5", 1);
            recipe.AddIngredient(null, "UpgradeStone", 4);
            recipe.AddIngredient(ItemID.MeteoriteBar, 10);
            recipe.AddIngredient(ItemID.Bone, 40);
            recipe.SetResult(this);
            recipe.AddRecipe();
        }
    }
}
