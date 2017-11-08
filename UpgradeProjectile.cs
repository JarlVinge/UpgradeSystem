using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace UpgradeSystem {
    class UpgradeProjectile : GlobalProjectile {

        public override bool PreAI(Projectile projectile) {
            try {
                UpgradeProjectileInfo info = projectile.GetGlobalProjectile<UpgradeProjectileInfo>(mod);
                if (!projectile.hostile && projectile.friendly) {
                    Player player = Main.player[projectile.owner];
                    Item item = player.inventory[player.selectedItem];
                    if(!item.IsAir) {
                        UpgradeInfo itemInfo = item.GetGlobalItem<UpgradeInfo>(mod);

                        if (itemInfo.elemented) {
                            info.elemented = true;
                            info.elementalProjectile = projectile;
                            info.elementType = itemInfo.elementType;
                            info.elementDamage = itemInfo.elementDamage;
                            info.sourceItem = item;
                            info.broken = itemInfo.broken;
                        }
                    }
                }
            }
            catch (Exception e) {
                ErrorLogger.Log(e.ToString());
            }
            return base.PreAI(projectile);
        }
        
        public override void OnHitNPC(Projectile projectile, NPC target, int damage, float knockback, bool crit) {
            try {
                UpgradeProjectileInfo info = projectile.GetGlobalProjectile<UpgradeProjectileInfo>(mod);
                if (info.elemented) {
                    ApplyElementBonusNPC(projectile, target, info.elementType, damage);
                }
                if (info.broken)
                    damage = 0;
            }
            catch (Exception e) {
                ErrorLogger.Log(e.ToString());
            }
            base.OnHitNPC(projectile, target, damage, knockback, crit);
        }

        public override void OnHitPvp(Projectile projectile, Player target, int damage, bool crit) {
            try {
                UpgradeProjectileInfo info = projectile.GetGlobalProjectile<UpgradeProjectileInfo>(mod);
                if (info.elemented) {
                    ApplyElementBonusPVP(projectile, target, info.elementType, damage);
                }
                if (info.broken)
                    damage = 0;
            }
            catch (Exception e) {
                ErrorLogger.Log(e.ToString());
            }
            base.OnHitPvp(projectile, target, damage, crit);
        }

        public override void ModifyHitNPC(Projectile projectile, NPC target, ref int damage, ref float knockback, ref bool crit, ref int hitDirection) {
            int damages = damage;
            try {
                UpgradeProjectileInfo info = projectile.GetGlobalProjectile<UpgradeProjectileInfo>(mod);
                if (info.elemented) {
                    damages += info.elementDamage;
                    if(info.elementType == "Air") {
                        knockback = (int) (knockback * 2);
                    }
                }
                if (info.broken)
                    damages = 0;
            }
            catch (Exception e) {
                ErrorLogger.Log(e.ToString());
            }
            base.ModifyHitNPC(projectile, target, ref damages, ref knockback, ref crit, ref hitDirection);
        }

        public override void ModifyHitPvp(Projectile projectile, Player target, ref int damage, ref bool crit) {
            int damages = damage;
            try {
                UpgradeProjectileInfo info = projectile.GetGlobalProjectile<UpgradeProjectileInfo>(mod);
                if (info.elemented) {
                    damages += info.elementDamage;
                }
                if (info.broken)
                    damages = 0;
            }
            catch (Exception e) {
                ErrorLogger.Log(e.ToString());
            }
            base.ModifyHitPvp(projectile, target, ref damages, ref crit);
        }
        
        private void ApplyElementBonusNPC(Projectile projectile, NPC target, String elementType, int damage) { //PVE
            if(!projectile.npcProj) {
                Player player = Main.player[Main.myPlayer];
                if (elementType == "Void") {
                    player.statLife += (int)Math.Round((Convert.ToDouble(damage * 0.10)));
                }
                if (elementType == "Fire") {
                    target.AddBuff(BuffID.OnFire, 180); //3 Seconds
                }
                if (elementType == "Ice") {
                    target.AddBuff(BuffID.Frozen, 180); //3 Seconds, Don't work now
                }
                if (elementType == "Water") {
                    target.AddBuff(BuffID.Slow, 180); //3 Seconds, Don't work now
                }
                if (elementType == "Earth") {
                    target.AddBuff(BuffID.Ichor, 180); //3 Seconds
                }
                if (elementType == "Air") {
                    //Increase knockback
                }
                if (elementType == "Thunder") {
                    target.AddBuff(BuffID.Confused, 180); //3 Seconds
                    target.AddBuff(BuffID.Midas, 1800); //30 Secondes
                }
                if (elementType == "Shadow") {
                    target.AddBuff(BuffID.ShadowFlame, 180); //3 Seconds
                }
                if (elementType == "Toxic") {
                    target.AddBuff(BuffID.Poisoned, 180); //3 Seconds
                }
                if (elementType == "Aether") {
                    player.statMana += (int)Math.Round((Convert.ToDouble(damage * 0.10)));
                }
                if (elementType == "Spirit") {
                    player.statLife += (int)Math.Round((Convert.ToDouble(damage * 0.05)));
                    player.statMana += (int)Math.Round((Convert.ToDouble(damage * 0.05)));
                }
            }
        }
        
        private void ApplyElementBonusPVP(Projectile projectile, Player target, String elementType, int damage) { //PVP
            if (!projectile.npcProj) {
                Player player = Main.player[Main.myPlayer];
                if (elementType == "Void") {
                    player.statLife += (int)Math.Round((Convert.ToDouble(damage * 0.10)));
                }
                if (elementType == "Fire") {
                    target.AddBuff(BuffID.OnFire, 180); //3 Seconds
                }
                if (elementType == "Ice") {
                    target.AddBuff(BuffID.Frozen, 180); //3 Seconds
                }
                if (elementType == "Water") {
                    target.AddBuff(BuffID.Slow, 180); //3 Seconds
                }
                if (elementType == "Earth") {
                    target.AddBuff(BuffID.Ichor, 180); //3 Seconds
                }
                if (elementType == "Air") {
                    //Increase knockback
                }
                if (elementType == "Thunder") {
                    target.AddBuff(BuffID.Confused, 180); //3 Seconds
                }
                if (elementType == "Shadow") {
                    target.AddBuff(BuffID.ShadowFlame, 180); //3 Seconds
                }
                if (elementType == "Toxic") {
                    target.AddBuff(BuffID.Poisoned, 180); //3 Seconds
                }
                if (elementType == "Aether") {
                    player.statMana += (int)Math.Round((Convert.ToDouble(damage * 0.10)));
                }
                if (elementType == "Spirit") {
                    player.statLife += (int)Math.Round((Convert.ToDouble(damage * 0.05)));
                    player.statMana += (int)Math.Round((Convert.ToDouble(damage * 0.05)));
                }
            }
        }

    }
}
