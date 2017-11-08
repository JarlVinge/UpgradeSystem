using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ModLoader;
using Terraria.ModLoader.IO;

namespace UpgradeSystem {
    class UpgradeItem : GlobalItem {

        public override bool NeedsSaving(Item item) {
            UpgradeInfo info = item.GetGlobalItem<UpgradeInfo>(mod);
            if (info.upgraded)
                return true;
            else
                return false;
        }
        public override TagCompound Save(Item item) {
            UpgradeInfo info = item.GetGlobalItem<UpgradeInfo>(mod);
            try {
                return new TagCompound {
                    {"baseName", info.baseName},
                    {"baseArmor", info.baseArmor},
                    {"baseDamage", info.baseDamage},
                    {"baseCrit", info.baseCrit},
                    {"upgraded", info.upgraded},
                    {"elemented", info.elemented},
                    {"broken", info.broken},
                    {"elementType", info.elementType},
                    {"elementDamage", info.elementDamage},
                    {"level", info.level},
                    {"modifier", info.modifier},
                    {"socket1", info.socket1},
                    {"socket2", info.socket2},
                    {"socket3", info.socket3},
                    {"modName", "UpgradeSystem"}
                };
            } catch (Exception e) {
                ErrorLogger.Log(e.ToString());
            }
            return new TagCompound {
                {"modName", "UpgradeSystem"}
            };
        }
        public override void Load(Item item, TagCompound tag) {
            try {
                UpgradeInfo info = item.GetGlobalItem<UpgradeInfo>(mod);
                String baseName = tag.GetString("baseName");
                int baseArmor = tag.GetInt("baseArmor");
                int baseDamage = tag.GetInt("baseDamage");
                int baseCrit = tag.GetInt("baseCrit");
                bool upgraded = tag.GetBool("upgraded");
                bool elemented = tag.GetBool("elemented");
                bool broken = tag.GetBool("broken");
                String elementType = tag.GetString("elementType");
                int elementDamage = tag.GetInt("elementDamage");
                int level = tag.GetInt("level");
                int modifier = tag.GetInt("modifier");
                String socket1 = tag.GetString("socket1");
                String socket2 = tag.GetString("socket2");
                String socket3 = tag.GetString("socket3");
                String modName = tag.GetString("modName");

                if (ModLoader.GetMod(modName) != null) {
                    info.SetProperties(baseName, baseArmor, baseDamage, baseCrit, upgraded, elemented, broken, elementType, elementDamage, level, modifier, socket1, socket2, socket3, item);
                }
                else {
                    info.ResetProperties();
                }
            }
            catch (Exception e) {
                ErrorLogger.Log(e.ToString());
            }

        }
        public override void LoadLegacy(Item item, BinaryReader reader) {
            try {
                UpgradeInfo info = item.GetGlobalItem<UpgradeInfo>(mod);
                String baseName = reader.ReadString();
                int baseArmor = reader.ReadInt32();
                int baseDamage = reader.ReadInt32();
                int baseCrit = reader.ReadInt32();
                bool upgraded = reader.ReadBoolean();
                bool elemented = reader.ReadBoolean();
                bool broken = reader.ReadBoolean();
                String elementType = reader.ReadString();
                int elementDamage = reader.ReadInt32();
                int level = reader.ReadInt32();
                int modifier = reader.ReadInt32();
                String socket1 = reader.ReadString();
                String socket2 = reader.ReadString();
                String socket3 = reader.ReadString();
                String modName = reader.ReadString();

                if (ModLoader.GetMod(modName) != null) {
                    info.SetProperties(baseName, baseArmor, baseDamage, baseCrit, upgraded, elemented, broken, elementType, elementDamage, level, modifier, socket1, socket2, socket3, item);
                }
                else {
                    info.ResetProperties();
                }
            }
            catch (Exception e) {
                ErrorLogger.Log(e.ToString());
            }
        }
        public override void UpdateEquip(Item item, Player player) {
            
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
                /*if (head.level >= 8 && body.level >= 8 && leg.level >= 8) {
                    CheckForUpgradeBonus(player);
                }*/
            }
        }
       /* private void CheckForUpgradeBonus(Player player) {

            if (!player.armor[0].toolTip2.Contains("Upgrade bonus")) {
                AddTooltip2(player.armor[0], "Upgrade bonus: 30% increased health");
            }
            if (!player.armor[1].toolTip2.Contains("Upgrade bonus")) {
                AddTooltip2(player.armor[1], "Upgrade bonus: 30% increased health");
            }
            if (!player.armor[2].toolTip2.Contains("Upgrade bonus")) {
                AddTooltip2(player.armor[2], "Upgrade bonus: 30% increased health");
            }
        }*/
        public override void ModifyTooltips(Item item, List<TooltipLine> tooltips) {
            UpgradeInfo info = item.GetGlobalItem<UpgradeInfo>(mod);
            if(!info.broken) {
                Player player = Main.player[Main.myPlayer];
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
                    if (!(head.level >= 8 && body.level >= 8 && leg.level >= 8)) {
                        for (int i = 0; i < tooltips.Count; i++) {
                            if (tooltips[i].text.Contains("Upgrade bonus")) {
                                tooltips.Remove(tooltips[i]);
                            }
                        }
                    }
                }
                else {
                    for (int i = 0; i < tooltips.Count; i++) {
                        if (tooltips[i].text.Contains("Upgrade bonus")) {
                            tooltips.Remove(tooltips[i]);
                        }
                    }
                }
                if (info.elemented) {
                    for (int i = 0; i < tooltips.Count; i++) {
                        for (int j = 0; j < UpgradeInfo.ElementsType.Count; j++) {
                            if (tooltips[i].text.Contains(UpgradeInfo.ElementsType[j] + " damage")) {
                                tooltips.Remove(tooltips[i]);
                            }
                        }
                    }
                }
                if (info.elemented && (item.magic || item.melee || item.ranged || item.summon)) {
                    TooltipLine tip = new TooltipLine(mod, "Elemental", info.elementDamage + " " + info.elementType + " damage");
                    tip.overrideColor = getColor(info.elementType);
                    if (item.favorited)
                        tooltips.Insert(4, tip);
                    else
                        tooltips.Insert(2, tip);
                }

                for (int i = 0; i < tooltips.Count; i++) {
                    if (tooltips[i].Name.Contains("Socket")) {
                        tooltips.Remove(tooltips[i]);
                    }
                }

                int bonus = 0;
                if (info.elemented)
                    bonus += 1;
                if (item.favorited)
                    bonus += 2;


                int numberOfSocket = info.getNumberOfSocket();


                if (info.socket1 == "")
                    info.socket1 = "NoSocket";
                if (info.socket2 == "")
                    info.socket2 = "NoSocket";
                if (info.socket3 == "")
                    info.socket3 = "NoSocket";

                String[] sockets = { info.socket1, info.socket2, info.socket3 };

                for (int i = 0; i < tooltips.Count; i++) {
                    if (tooltips[i].Name.Contains("Socket"))
                        tooltips.Remove(tooltips[i]);
                }
                for (int i = 1; i <= numberOfSocket; i++) {
                    if (sockets[i - 1] == "NoSocket" || sockets[i - 1] == null || sockets[i - 1] == "") { //S'il le socket est null (Aucune perle)
                        TooltipLine tip = new TooltipLine(mod, "Socket" + i, "Empty Socket");
                        tip.overrideColor = new Color(255, 255, 255);

                        tooltips.Insert(1 + i + bonus, tip);
                    }
                    else {
                        TooltipLine tip = new TooltipLine(mod, sockets[i - 1], info.getSocketContent(i));
                        if (sockets[i - 1] == "+5% Life Steal")
                            tip.overrideColor = new Color(255, 0, 0);
                        if (sockets[i - 1] == "+5% Movement Speed")
                            tip.overrideColor = new Color(0, 255, 0);

                        tooltips.Insert(1 + i + bonus, tip);
                    }
                }
            }
            else {
                int bonus = 0;
                if (item.favorited)
                    bonus += 2;
                for (int i = 1 + bonus; i < tooltips.Count(); i++)
                    tooltips.RemoveAt(i);
                TooltipLine tip = new TooltipLine(mod, "broken", "Broken Item");
                tooltips.Insert(1 + bonus, tip);
            }
            
        }
        private Color? getColor(String elementType) {
            switch (elementType) {
                case "Void":
                    return new Color(255, 0, 255);
                case "Fire":
                    return new Color(255, 0, 0);
                case "Ice":
                    return new Color(102, 255, 255);
                case "Water":
                    return new Color(0, 0, 255);
                case "Earth":
                    return new Color(51, 25, 0);
                case "Air":
                    return new Color(204, 255, 204);
                case "Thunder":
                    return new Color(255, 255, 0);
                case "Shadow":
                    return new Color(64, 64, 64);
                case "Toxic":
                    return new Color(0, 51, 0);
                case "Aether":
                    return new Color(51, 0, 51);
                case "Spirit":
                    return new Color(255, 204, 204);
                default:
                    return new Color(255, 255, 255);
            }
        }
        public override void ModifyHitNPC(Item item, Player player, NPC target, ref int damage, ref float knockBack, ref bool crit) {
            try {
                UpgradeInfo info = item.GetGlobalItem<UpgradeInfo>(mod);
                if (info.elemented)
                    damage += info.elementDamage;
                if (info.broken)
                    damage = 0;
            }
            catch (Exception e) {
                ErrorLogger.Log(e.ToString());
            }
            base.ModifyHitNPC(item, player, target, ref damage, ref knockBack, ref crit);
        }
        public override bool CanUseItem(Item item, Player player) {
            UpgradeInfo info = item.GetGlobalItem<UpgradeInfo>(mod);
            return !info.broken;
        }
    }
}
