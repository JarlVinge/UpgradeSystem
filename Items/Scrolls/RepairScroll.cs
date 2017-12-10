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

        public override void AddRecipes() {
            ModRecipe recipe = new ModRecipe(mod);
            recipe.AddIngredient(null, "UpgradeStone", 5);
            recipe.AddIngredient(ItemID.LifeCrystal, 5);
            recipe.AddIngredient(ItemID.HealingPotion, 10);
            recipe.AddIngredient(ItemID.RestorationPotion, 10);
            recipe.SetResult(this);
            recipe.AddRecipe();
        }
    }
}
