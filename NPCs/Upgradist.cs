using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace DushyUpgrade.NPCs
{
    class Dushy : ModNPC
    {
        public override bool Autoload(ref string name)
        {
            name = "Upgradist";
            return mod.Properties.Autoload;
        }
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Upgradist");
        }
        public override void SetDefaults()
        {
            npc.townNPC = true;
            npc.friendly = true;
            npc.width = 18;
            npc.height = 40;
            npc.aiStyle = 7;
            npc.damage = 10;
            npc.defense = 15;
            npc.lifeMax = 250;
            npc.HitSound = SoundID.NPCHit1;
            npc.DeathSound = SoundID.NPCDeath1;
            npc.knockBackResist = 0.5f;
            Main.npcFrameCount[npc.type] = 25;
            NPCID.Sets.ExtraFramesCount[npc.type] = 9;
            NPCID.Sets.AttackFrameCount[npc.type] = 4;
            NPCID.Sets.DangerDetectRange[npc.type] = 700;
            NPCID.Sets.AttackType[npc.type] = 0;
            NPCID.Sets.AttackTime[npc.type] = 90;
            NPCID.Sets.AttackAverageChance[npc.type] = 30;
            NPCID.Sets.HatOffsetY[npc.type] = 4;
            NPCID.Sets.ExtraTextureCount[npc.type] = 1;
            animationType = NPCID.Guide;
        }
        
        public override string TownNPCName()
        {
            return "Dushy";
        }

        public override bool CanTownNPCSpawn(int numTownNPCs, int money) {
            for (int k = 0; k < 255; k++) {
                Player player = Main.player[k];
                if (player.active)
                    return true;
            }
            return false;
        }

        public override void SetupShop(Chest shop, ref int nextSlot) {
            if (NPC.downedBoss1) { //Eye
                shop.item[nextSlot].SetDefaults(mod.ItemType("UpgradeScroll4"));
                nextSlot++;
            }
            if (NPC.downedBoss2) { //EoW BoF
                shop.item[nextSlot].SetDefaults(mod.ItemType("UpgradeScroll5"));
                nextSlot++;
            }
            if (NPC.downedBoss3) { //Skeletron
                shop.item[nextSlot].SetDefaults(mod.ItemType("UpgradeScroll6"));
                nextSlot++;
            }
            if (Main.hardMode) { //Hardmode WoF
                shop.item[nextSlot].SetDefaults(mod.ItemType("UpgradeScroll7"));
                nextSlot++;
            }
            if (NPC.downedMechBossAny) { //Any mecha
                shop.item[nextSlot].SetDefaults(mod.ItemType("UpgradeScroll8"));
                nextSlot++;
            }
            if (NPC.downedPlantBoss) { //Plantera
                shop.item[nextSlot].SetDefaults(mod.ItemType("UpgradeScroll9"));
                nextSlot++;
            }
            if (NPC.downedGolemBoss) { //Golem
                shop.item[nextSlot].SetDefaults(mod.ItemType("UpgradeScroll10"));
                nextSlot++;
                shop.item[nextSlot].SetDefaults(mod.ItemType("ElementalScroll"));
                nextSlot++;
            }
            if (NPC.downedFishron) { //Fishron
                shop.item[nextSlot].SetDefaults(mod.ItemType("UpgradeScroll11"));
                nextSlot++;
            }
            if (NPC.downedBoss2) { //EoW BoF
                shop.item[nextSlot].SetDefaults(mod.ItemType("OrbMoveSpeed"));
                nextSlot++;
                shop.item[nextSlot].SetDefaults(mod.ItemType("OrbLifeSteal"));
                nextSlot++;
            }
            shop.item[nextSlot].SetDefaults(mod.ItemType("ProtectionScroll"));
            nextSlot++;
            shop.item[nextSlot].SetDefaults(mod.ItemType("RepairScroll"));
            nextSlot++;

        }

        public override void SetChatButtons(ref string button, ref string button2) {
            button = Lang.inter[28].Value;
        }

        public override void OnChatButtonClicked(bool firstButton, ref bool shop) {
            if (firstButton)
                shop = true;
        }

        public override void TownNPCAttackStrength(ref int damage, ref float knockback)
        {
            damage = 20;
            knockback = 4f;
        }

        public override void TownNPCAttackCooldown(ref int cooldown, ref int randExtraCooldown)
        {
            cooldown = 30;
            randExtraCooldown = 30;
        }

        public override void TownNPCAttackProj(ref int projType, ref int attackDelay)
        {
            projType = mod.ProjectileType("SparklingBall");
            attackDelay = 1;
        }

        public override void TownNPCAttackProjSpeed(ref float multiplier, ref float gravityCorrection, ref float randomOffset)
        {
            multiplier = 12f;
            randomOffset = 2f;
        }

        public override string GetChat()
        {
            switch (Main.rand.Next(3))
            {
                case 0:
                    return "Sometimes I feel like I'm different from everyone else here.";
                case 1:
                    return "What's your favorite color? My favorite colors are white and black.";
                default:
                    return "What? I don't have any arms or legs? Oh, don't be ridiculous!";
            }
        }
    }
}
