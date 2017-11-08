using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.ModLoader;
using Terraria.ModLoader.IO;

namespace UpgradeSystem {
    class UpgradePlayer : ModPlayer {
        public bool protectingStone { get; set; }
        internal void SetProperties(bool protectingStone) {
            this.protectingStone = protectingStone;
        }
        public override void Load(TagCompound tag) {
            protectingStone = tag.GetBool("protectingStone");
        }
        public override void LoadLegacy(BinaryReader reader) {
            try {
                UpgradePlayer thePlayer = player.GetModPlayer<UpgradePlayer>(mod);
                bool protectingStone = reader.ReadBoolean();
                String modName = reader.ReadString();

                if (ModLoader.GetMod(modName) != null) {
                    thePlayer.SetProperties(protectingStone);
                }
                else {
                    thePlayer.ResetProperties();
                }
            }
            catch (Exception e) {
                ErrorLogger.Log(e.ToString());
            }
        }
        public override TagCompound Save() {
            return new TagCompound {
                {"protectingStone", protectingStone}
            };
        }
        private void ResetProperties() {
            this.protectingStone = false;
        }
        public override void PreUpdate() {
            if (player.statLife > player.statLifeMax2) {
                player.statLife = player.statLifeMax2;
            }
        }
        public override void PostUpdateEquips() {
            bool isFullArmroed = true;
            if (player.armor[0].type <= 0 || player.armor[0].stack <= 0)
                isFullArmroed = false;
            if (player.armor[1].type <= 0 || player.armor[1].stack <= 0)
                isFullArmroed = false;
            if (player.armor[2].type <= 0 || player.armor[2].stack <= 0)
                isFullArmroed = false;
            if (isFullArmroed) {
                UpgradeInfo head = player.armor[0].GetGlobalItem<UpgradeInfo>(mod);
                UpgradeInfo body = player.armor[1].GetGlobalItem<UpgradeInfo>(mod);
                UpgradeInfo leg = player.armor[2].GetGlobalItem<UpgradeInfo>(mod);
                if (head.level >= 8 && body.level >= 8 && leg.level >= 8) {
                    player.statLifeMax2 += (int)Math.Round(Convert.ToDouble(0.30 * player.statLifeMax));
                    /*TooltipLine line = new TooltipLine(mod, "UpgradeBonus", "Upgrade bonus: 30% increased health");
                    line.overrideColor = new Color(100, 100, 255);
                    tooltips.Add(line);
                    foreach (TooltipLine line2 in tooltips) {
                    }*/
                }
            }
        }
    }
}
