using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DushyUpgrade.Items;
using Terraria;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;

namespace DushyUpgrade {
    class UpgradeProjectileInfo : GlobalProjectile {
        public bool elemented { get; set; }
        public bool broken { get; set; }
        public int elementDamage { get; set; }
        public String elementType { get; set; }
        public Projectile elementalProjectile { get; set; }
        public Item sourceItem { get; set; }
        public override bool InstancePerEntity
        {
            get
            {
                return true;
            }
        }
        public override bool CloneNewInstances
        {
            get
            {
                return true;
            }
        }
    }
    class UpgradeInfo : GlobalItem {

        public String baseName { get; set; }
        public int baseArmor { get; set; }
        public int baseDamage { get; set; }
        public int baseCrit { get; set; }
        public bool upgraded { get; set; }
        public bool elemented { get; set; }
        public bool broken { get; set; }
        public String elementType { get; set; }
        public int elementDamage { get; set; }
        public int level { get; set; }
        public int modifier { get; set; }
        public Item item { get; set; }
        public String socket1 { get; set; }
        public String socket2 { get; set; }
        public String socket3 { get; set; }
        public override bool InstancePerEntity { get { return true; } }
        public override bool CloneNewInstances
        {
            get
            {
                return true;
            }
        }

        public static List<String> ElementsType = new List<String>() {
            "Void", //Lifesteal
            "Fire", //Fire debuff
            "Ice", //Froze at a % chance (NEED TO CREATE THAT FCKING BUFF)
            "Water", //Slow
            "Earth", //Ichor (reduc def)
            "Air", //Increase knockback
            "Thunder", //Confuse at a %chance
            "Shadow", //Shadowflame debuff
            "Toxic", //Toxic debuff
            "Aether", //Manasteal
            "Spirit" //Mana and lifesteal
        };

        public int Upgrade(Item itemToUpgrade, Boolean isProtected) {
            UpgradeInfo info = itemToUpgrade.GetGlobalItem<UpgradeInfo>(mod);

            if (DushyUpgrade.IsAWeapon(itemToUpgrade)) { //Si c'est une arme
                int upgradeResult = UpgradeWeapon(itemToUpgrade, isProtected);
                return upgradeResult;
            } else if (DushyUpgrade.IsAnArmor(itemToUpgrade)) { //Si c'est une armure
                int upgradeResult = UpgradeArmor(itemToUpgrade, isProtected);
                return upgradeResult;
            } else {
                return DushyUpgrade.NO_CHANGE;
            }
        }
        public int UpgradeWeapon(Item itemToUpgrade, Boolean isProtected) {
            UpgradeInfo info = itemToUpgrade.GetGlobalItem<UpgradeInfo>(mod);

            if (!info.upgraded) {
                info.level = 1;
                info.upgraded = true;
                info.baseCrit = itemToUpgrade.crit;
                info.baseDamage = itemToUpgrade.damage;
                info.baseName = itemToUpgrade.Name;

                info.modifier = getDamageModifier(info.level);
                itemToUpgrade.damage = (int)Math.Round((Convert.ToDouble(info.baseDamage * info.modifier) / 100));
                itemToUpgrade.crit = (int)Math.Round((Convert.ToDouble(info.baseCrit * info.modifier) / 100));
                itemToUpgrade.SetNameOverride(info.baseName + " +" + info.level);
                return DushyUpgrade.SUCCESS;
            }
            else {
                int roll = RollUpgrade(info.level);
                if (roll == DushyUpgrade.SUCCESS) {
                    info.level++;
                    info.modifier = getDamageModifier(info.level);
                    itemToUpgrade.damage = (int)Math.Round((Convert.ToDouble(info.baseDamage * info.modifier) / 100));
                    itemToUpgrade.crit = (int)Math.Round((Convert.ToDouble(info.baseCrit * info.modifier) / 100));
                    itemToUpgrade.SetNameOverride(info.baseName + " +" + info.level);
                    if (info.level >= 10 && !info.elemented)
                        EnchantItemElement(itemToUpgrade, info);
                    if (info.elemented)
                        info.elementDamage = (int)Math.Round(Convert.ToDouble(itemToUpgrade.damage * 0.10));
                    return DushyUpgrade.SUCCESS;
                }
                else if (roll == DushyUpgrade.NO_CHANGE)
                    return DushyUpgrade.NO_CHANGE;

                else if (roll == DushyUpgrade.FAILURE) {
                    if (isProtected)
                        return DushyUpgrade.FAILURE_PROTECTED;

                    info.level--;
                    info.modifier = getDamageModifier(info.level);
                    itemToUpgrade.damage = (int)Math.Round((Convert.ToDouble(info.baseDamage * info.modifier) / 100));
                    itemToUpgrade.crit = (int)Math.Round((Convert.ToDouble(info.baseCrit * info.modifier) / 100));
                    itemToUpgrade.SetNameOverride(info.baseName + " +" + info.level);
                    if (info.elemented)
                        info.elementDamage = (int)Math.Round(Convert.ToDouble(itemToUpgrade.damage * 0.10));
                    return DushyUpgrade.FAILURE;
                }
                else if (roll == DushyUpgrade.RESET) {
                    if (isProtected)
                        return DushyUpgrade.RESET_PROTECTED;

                    info.level = 0;
                    itemToUpgrade.SetNameOverride(info.baseName);
                    info.modifier = getDamageModifier(info.level);
                    itemToUpgrade.damage = (int)Math.Round((Convert.ToDouble(info.baseDamage * info.modifier) / 100));
                    itemToUpgrade.crit = (int)Math.Round((Convert.ToDouble(info.baseCrit * info.modifier) / 100));
                    if (info.elemented)
                        info.elementDamage = (int)Math.Round(Convert.ToDouble(itemToUpgrade.damage * 0.10));
                    return DushyUpgrade.RESET;
                }
                else {
                    if (isProtected)
                        return DushyUpgrade.BREAKING_PROTECTED;

                    info.level = 0;
                    itemToUpgrade.SetNameOverride("Broken " + info.baseName);
                    info.broken = true;
                    itemToUpgrade.damage = 0;
                    return DushyUpgrade.BREAKING;
                }
            }
        }

        public int UpgradeArmor(Item itemToUpgrade, Boolean isProtected) {
            UpgradeInfo info = itemToUpgrade.GetGlobalItem<UpgradeInfo>(mod);

            if (!info.upgraded) {
                info.level = 1;
                info.upgraded = true;
                info.baseArmor = itemToUpgrade.defense;
                info.baseName = itemToUpgrade.Name;

                info.modifier = getDamageModifier(info.level);
                itemToUpgrade.SetNameOverride(info.baseName + " +" + info.level);
                itemToUpgrade.defense = (int)Math.Round((Convert.ToDouble(info.baseArmor * info.modifier) / 100));
                return DushyUpgrade.SUCCESS;
            }
            else {
                int roll = RollUpgrade(info.level);
                if (roll == DushyUpgrade.SUCCESS) {
                    info.level++;
                    info.modifier = getDamageModifier(info.level);
                    itemToUpgrade.SetNameOverride(info.baseName + " +" + info.level);
                    itemToUpgrade.defense = (int)Math.Round((Convert.ToDouble(info.baseArmor * info.modifier) / 100));

                    return DushyUpgrade.SUCCESS;
                }
                else if (roll == DushyUpgrade.NO_CHANGE)
                    return DushyUpgrade.NO_CHANGE;

                else if (roll == DushyUpgrade.FAILURE) {
                    if (isProtected)
                        return DushyUpgrade.FAILURE_PROTECTED;

                    info.level--;
                    info.modifier = getDamageModifier(info.level);
                    itemToUpgrade.defense = (int)Math.Round((Convert.ToDouble(info.baseArmor * info.modifier) / 100));
                    itemToUpgrade.SetNameOverride(info.baseName + " +" + info.level);
                    return DushyUpgrade.FAILURE;
                }
                else if (roll == DushyUpgrade.RESET) {
                    if (isProtected)
                        return DushyUpgrade.RESET_PROTECTED;

                    info.level = 0;
                    itemToUpgrade.SetNameOverride(info.baseName);
                    info.modifier = getDamageModifier(info.level);
                    itemToUpgrade.defense = (int)Math.Round((Convert.ToDouble(info.baseArmor * info.modifier) / 100));
                    return DushyUpgrade.RESET;
                }
                else {
                    if (isProtected)
                        return DushyUpgrade.BREAKING_PROTECTED;

                    info.level = 0;
                    itemToUpgrade.SetNameOverride("Broken " + info.baseName);
                    info.broken = true;
                    itemToUpgrade.defense = 0;
                    return DushyUpgrade.BREAKING;
                }
            }
        }

        public String getSocketContent(int i)
        {
            if (i == 1)
                return socket1;
            if (i == 2)
                return socket2;
            if (i == 3)
                return socket3;
            return "";
        }

        public bool socketAvailable()
        {
            bool isAvailable = false;
            if ((socket1 == null || socket1 == "" || socket1 == "NoSocket") && this.getNumberOfSocket() >= 1)
                isAvailable = true;
            if ((socket2 == null || socket2 == "" || socket2 == "NoSocket") && this.getNumberOfSocket() >= 2)
                isAvailable = true;
            if ((socket3 == null || socket3 == "" || socket3 == "NoSocket") && this.getNumberOfSocket() >= 3)
                isAvailable = true;

            return isAvailable;
        }

        public void EnchantItemElement(Item item, UpgradeInfo info)
        {
            info.elemented = true;
            info.elementDamage = (int)Math.Round(Convert.ToDouble(item.damage * 0.10));
            Random random = new Random();
            int index = random.Next(ElementsType.Count);
            info.elementType = ElementsType[index];
        }

        public void ChangeItemElement(Item item, UpgradeInfo info)
        {
            Random random = new Random();
            int index = random.Next(ElementsType.Count);
            info.elementType = ElementsType[index];
        }

        public int RollUpgrade(int level)
        {
            Random random = new Random();
            int leRandom = random.Next(0, 9999);
            switch (level)
            {
                case 0: //Level 0 à 1
                    return DushyUpgrade.SUCCESS;
                case 1: //Level 1 à 2
                    return DushyUpgrade.SUCCESS;
                case 2: //Level 2 à 3
                    if (leRandom < 9500)
                        return DushyUpgrade.SUCCESS;
                    return DushyUpgrade.NO_CHANGE;
                case 3: //Level 3 à 4
                    if (leRandom < 5700)
                        return DushyUpgrade.SUCCESS;
                    if (leRandom < 5700 + 4050)
                        return DushyUpgrade.NO_CHANGE;
                    return DushyUpgrade.FAILURE;
                case 4: //Level 4 à 5
                    if (leRandom < 3000)
                        return DushyUpgrade.SUCCESS;
                    if (leRandom < 3000 + 4400)
                        return DushyUpgrade.NO_CHANGE;
                    return DushyUpgrade.FAILURE;
                case 5: //Level 5 à 6
                    if (leRandom < 1650)
                        return DushyUpgrade.SUCCESS;
                    if (leRandom < 1650 + 3250)
                        return DushyUpgrade.NO_CHANGE;
                    return DushyUpgrade.FAILURE;
                case 6: //Level 6 à 7
                    if (leRandom < 1218)
                        return DushyUpgrade.SUCCESS;
                    if (leRandom < 1218 + 1973)
                        return DushyUpgrade.NO_CHANGE;
                    if (leRandom < 1218 + 1973 + 3654)
                        return DushyUpgrade.FAILURE;
                    if (leRandom < 1218 + 1973 + 3654 + 1328)
                        return DushyUpgrade.RESET;
                    return DushyUpgrade.BREAKING;
                case 7: //Level 7 à 8
                    if (leRandom < 750)
                        return DushyUpgrade.SUCCESS;
                    if (leRandom < 750 + 942)
                        return DushyUpgrade.NO_CHANGE;
                    if (leRandom < 750 + 942 + 3865)
                        return DushyUpgrade.FAILURE;
                    if (leRandom < 750 + 942 + 3865 + 2596)
                        return DushyUpgrade.RESET;
                    return DushyUpgrade.BREAKING;
                case 8: //Level 8 à 9
                    if (leRandom < 346)
                        return DushyUpgrade.SUCCESS;
                    if (leRandom < 346 + 1635)
                        return DushyUpgrade.NO_CHANGE;
                    if (leRandom < 346 + 1635 + 3115)
                        return DushyUpgrade.FAILURE;
                    if (leRandom < 346 + 1635 + 3115 + 2923)
                        return DushyUpgrade.RESET;
                    return DushyUpgrade.BREAKING;
                case 9: //Level 9 à 10
                    if (leRandom < 133)
                        return DushyUpgrade.SUCCESS;
                    if (leRandom < 133 + 728)
                        return DushyUpgrade.NO_CHANGE;
                    if (leRandom < 133 + 728 + 2252)
                        return DushyUpgrade.FAILURE;
                    if (leRandom < 133 + 728 + 2252 + 3907)
                        return DushyUpgrade.RESET;
                    return DushyUpgrade.BREAKING;
                case 10: //Level 10 à 11
                    if (leRandom < 99)
                        return DushyUpgrade.SUCCESS;
                    if (leRandom < 99 + 1026)
                        return DushyUpgrade.NO_CHANGE;
                    if (leRandom < 99 + 1026 + 1390)
                        return DushyUpgrade.FAILURE;
                    if (leRandom < 99 + 1026 + 1390 + 4735)
                        return DushyUpgrade.RESET;
                    return DushyUpgrade.BREAKING;
                default:
                    return DushyUpgrade.NO_CHANGE;
            }
        }

        private int getDamageModifier(int level)
        {
            switch (level)
            {
                case 0:
                    return 100;
                case 1:
                    return 103;
                case 2:
                    return 106;
                case 3:
                    return 109;
                case 4:
                    return 116;
                case 5:
                    return 123;
                case 6:
                    return 130;
                case 7:
                    return 145;
                case 8:
                    return 160;
                case 9:
                    return 175;
                case 10:
                    return 215;
                case 11:
                    return 255;
                default:
                    return 100;
            }

        }

        public int getNumberOfSocket()
        {
            int numberOfSocket = 0;

            if (this.level >= 5)
                numberOfSocket++;
            if (this.level >= 8)
                numberOfSocket++;
            if (this.level == 11)
                numberOfSocket++;

            return numberOfSocket;
        }

        public void UpgradeItemScroll(Item item, Player player, int scrollLevel)
        {
            UpgradeInfo info = item.GetGlobalItem<UpgradeInfo>(mod);
            UpgradePlayer thePlayer = player.GetModPlayer<UpgradePlayer>(mod);
            if (item.magic || item.melee || item.summon || item.ranged)
            {
                if (!info.upgraded)
                {
                    info.upgraded = true;
                    info.baseCrit = item.crit;
                    info.baseDamage = item.damage;
                    info.baseName = item.Name;
                }

                info.level = scrollLevel;
                info.modifier = getDamageModifier(info.level);
                item.damage = (int)Math.Round((Convert.ToDouble(info.baseDamage * info.modifier) / 100));
                item.crit = (int)Math.Round((Convert.ToDouble(info.baseCrit * info.modifier) / 100));
                item.SetNameOverride(info.baseName + " +" + info.level);
                if (info.elemented)
                    info.elementDamage = (int)Math.Round(Convert.ToDouble(item.damage * 0.10));
            }
            else
            {
                if (!info.upgraded)
                {
                    info.upgraded = true;
                    info.baseArmor = item.defense;
                    info.baseName = item.Name;
                }

                info.level = scrollLevel;
                info.modifier = getDamageModifier(info.level);
                item.SetNameOverride(info.baseName + " +" + info.level);
                item.defense = (int)Math.Round((Convert.ToDouble(info.baseArmor * info.modifier) / 100));

            }
            if (info.level >= 10 && info.elemented == false)
                EnchantItemElement(item, info);
        }

        public void AddIntoSocket(String pearlName, Color color, UpgradeInfo info)
        {

            if (info.socket1 == null || info.socket1 == "NoSocket" || info.socket1 == "")
            {
                info.socket1 = pearlName;
                return;
            }
            if (info.socket2 == null || info.socket2 == "NoSocket" || info.socket2 == "")
            {
                info.socket2 = pearlName;
                return;
            }
            if (info.socket3 == null || info.socket3 == "NoSocket" || info.socket3 == "")
            {
                info.socket3 = pearlName;
                return;
            }
        }

        public void RepairItem(Item item, Player player)
        {
            UpgradeInfo info = item.GetGlobalItem<UpgradeInfo>(mod);
            if (info.broken)
            {
                broken = false;
                info.modifier = getDamageModifier(info.level);
                item.SetNameOverride(this.baseName);
                if (item.melee || item.summon || item.magic || item.ranged)
                {
                    item.damage = (int)Math.Round((Convert.ToDouble(info.baseDamage * info.modifier) / 100));
                    item.crit = (int)Math.Round((Convert.ToDouble(info.baseCrit * info.modifier) / 100));
                    info.elementDamage = (int)Math.Round(Convert.ToDouble(item.damage * 0.10));
                }
                else
                    item.defense = (int)Math.Round((Convert.ToDouble(info.baseArmor * info.modifier) / 100));
            }
        }

        internal void SetProperties(String baseName, int baseArmor, int baseDamage, int baseCrit, bool upgraded, bool elemented, bool broken, String elementType, int elementDamage, int level, int modifier, String socket1, String socket2, String socket3, Item item)
        {
            this.baseName = baseName;
            this.baseArmor = baseArmor;
            this.baseDamage = baseDamage;
            this.baseCrit = baseCrit;
            this.upgraded = upgraded;
            this.elemented = elemented;
            this.broken = broken;
            this.elementType = elementType;
            this.elementDamage = elementDamage;
            this.level = level;
            this.modifier = modifier;
            this.socket1 = socket1;
            this.socket2 = socket2;
            this.socket3 = socket3;
            this.item = item;
            if (this.broken)
                item.SetNameOverride("Broken " + this.baseName + " +" + this.level);
            else
                item.SetNameOverride(this.baseName + " +" + this.level);
            if (!this.broken)
            {
                if (item.magic || item.melee || item.summon || item.ranged)
                {
                    item.damage = (int)Math.Round((Convert.ToDouble(this.baseDamage * this.modifier) / 100));
                    item.crit = (int)Math.Round((Convert.ToDouble(this.baseCrit * this.modifier) / 100));
                }
                if (item.headSlot > 0 || item.bodySlot > 0 || item.legSlot > 0)
                    item.defense = (int)Math.Round((Convert.ToDouble(this.baseArmor * this.modifier) / 100));
            }
            else
                item.damage = 0;
        }
        internal void ResetProperties()
        {
            this.upgraded = false;
            this.elemented = false;
            this.broken = false;
            this.elementType = "";
            this.elementDamage = 0;
            this.level = 0;
            this.modifier = 100;
            this.socket1 = "NoSocket";
            this.socket2 = "NoSocket";
            this.socket3 = "NoSocket";
        }
    }
}
