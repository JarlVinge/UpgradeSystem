using Terraria.ID;
using Terraria.ModLoader;

namespace UpgradeSystem.Items.Placeable {

	public class UpgradeWorkbench : ModItem {

		public override void SetStaticDefaults() {
            DisplayName.SetDefault("Upgrade Workbench");
            Tooltip.SetDefault("Upgrade your weapons with this workbench.");
		}

		public override void SetDefaults() {
			item.width = 28;
			item.height = 14;
			item.maxStack = 99;
			item.useTurn = true;
			item.autoReuse = true;
			item.useAnimation = 15;
			item.useTime = 10;
			item.useStyle = 1;
			item.consumable = true;
			item.value = 150;
			item.createTile = mod.TileType("UpgradeWorkbench");
		}

		public override void AddRecipes() {
			ModRecipe recipe = new ModRecipe(mod);
			//recipe.AddIngredient(ItemID.WorkBench);
			//recipe.AddIngredient(null, "ExampleBlock", 10);
			recipe.SetResult(this);
			recipe.AddRecipe();
		}

	}
}
