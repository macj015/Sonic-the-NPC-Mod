using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;
using Terraria;
using Terraria.GameContent;
using Terraria.GameContent.Bestiary;
using Terraria.GameContent.Personalities;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;
using Terraria.Utilities;

namespace SonicNPC.Content.NPCs.TownNPCs
{
    [AutoloadHead]
    public class Hedgehog : ModNPC
    {
        public override LocalizedText DeathMessage => this.GetLocalization("DeathMessage");
        private static int ShimmerHeadIndex;
    private static Profiles.StackedNPCProfile NPCProfile;
        public override void Load() {
		    ShimmerHeadIndex = Mod.AddNPCHeadTexture(Type, Texture + "_Shimmer_Head");
        }
        public override void SetStaticDefaults()
        {
            Main.npcFrameCount[Type] = 26;

            NPCID.Sets.ExtraFramesCount[Type] = 10;
            NPCID.Sets.AttackFrameCount[Type] = 5;
            NPCID.Sets.DangerDetectRange[Type] = 1000;
            NPCID.Sets.AttackType[Type] = 2;
            NPCID.Sets.AttackTime[Type] = 40;
            NPCID.Sets.AttackAverageChance[Type] = 10;
			NPCID.Sets.ShimmerTownTransform[Type] = true;
            NPCID.Sets.MagicAuraColor[Type] = Color.Blue;
			NPCID.Sets.NPCBestiaryDrawModifiers drawModifiers = new NPCID.Sets.NPCBestiaryDrawModifiers()
            {
                Velocity = 1f,
                Direction = 1
            };
            NPCID.Sets.NPCBestiaryDrawOffset.Add(Type, drawModifiers);
            NPC.Happiness
				.SetBiomeAffection<ForestBiome>(AffectionLevel.Like)
				.SetBiomeAffection<DesertBiome>(AffectionLevel.Dislike)
                .SetBiomeAffection<JungleBiome>(AffectionLevel.Hate)
				.SetNPCAffection(NPCID.BestiaryGirl, AffectionLevel.Love)
				.SetNPCAffection(NPCID.Dryad, AffectionLevel.Like)
				.SetNPCAffection(NPCID.Merchant, AffectionLevel.Dislike)
				.SetNPCAffection(NPCID.Demolitionist, AffectionLevel.Hate)
			;
            NPCProfile = new Profiles.StackedNPCProfile(
				new Profiles.DefaultNPCProfile(Texture, NPCHeadLoader.GetHeadSlot(HeadTexture), Texture + "_Party"),
				new Profiles.DefaultNPCProfile(Texture + "_Shimmer", ShimmerHeadIndex, Texture + "_Shimmer_Party")
			);
        }

        public override void SetDefaults()
        {
            NPC.townNPC = true;
            NPC.friendly = true;
            NPC.width = 18;
            NPC.height = 54;
            NPC.aiStyle = NPCAIStyleID.Passive;
            NPC.defense = 990;
            NPC.lifeMax = 1000;
            NPC.HitSound = SoundID.NPCHit1;
            NPC.DeathSound = SoundID.NPCDeath1;
            NPC.knockBackResist = 0.2f;

            AnimationType = NPCID.Guide;
        }
    		public override void SetBestiary(BestiaryDatabase database, BestiaryEntry bestiaryEntry) {
			bestiaryEntry.Info.AddRange([

				BestiaryDatabaseNPCsPopulator.CommonTags.SpawnConditions.Biomes.Surface,

				new FlavorTextBestiaryInfoElement("Mods.SonicNPC.Bestiary.Hedgehog_1"),

				new FlavorTextBestiaryInfoElement("Mods.SonicNPC.Bestiary.Hedgehog_2")
			]);
		}

        public override bool CanTownNPCSpawn(int numTownNPCs)
        {
            if (NPC.downedBoss1)
            {
                return true;
            }
                return false;
        }

        public override List<string> SetNPCNameList()
        {
            return new List<string>()
            {
                "Sonic"
            };
        }
		public override ITownNPCProfile TownNPCProfile() {
		    return NPCProfile;
	    }

        public override bool UsesPartyHat() {
	        return false;
        }

        public override string GetChat() {
            if (NPC.homeless)
            {
                return Language.GetTextValue("Mods.SonicNPC.NPCS.Hedgehog.TownMood.NoHome");
            }
			WeightedRandom<string> chat = new WeightedRandom<string>();
            if (Main.LocalPlayer.ZoneNormalUnderground)
            {
                chat.Add(Language.GetTextValue("Mods.SonicNPC.Dialogue.Hedgehog.UndergroundDialogue"));
            }
            if(Main.LocalPlayer.name == "Sonic" || Main.LocalPlayer.name == "Sonic the Hedgehog" || Main.LocalPlayer.name == "Sonic the hedgehog" || Main.LocalPlayer.name == "sonic")
            {
                chat.Add(Language.GetTextValue("Mods.SonicNPC.Dialogue.Hedgehog.FakerDialogue"));
            }
            if (NPC.downedMoonlord)
            {
                chat.Add(Language.GetTextValue("Mods.SonicNPC.Dialogue.Hedgehog.DownedMoonLord"));
            }

            if (NPC.IsShimmerVariant)
            {
                chat.Add(Language.GetTextValue("Mods.SonicNPC.Dialogue.Hedgehog.ShimmerDialogue1"));
                chat.Add(Language.GetTextValue("Mods.SonicNPC.Dialogue.Hedgehog.ShimmerDialogue2"));
                chat.Add(Language.GetTextValue("Mods.SonicNPC.Dialogue.Hedgehog.ShimmerDialogue3"));
            }
            else {
                chat.Add(Language.GetTextValue("Mods.SonicNPC.Dialogue.Hedgehog.StandardDialogue1"));
		        chat.Add(Language.GetTextValue("Mods.SonicNPC.Dialogue.Hedgehog.StandardDialogue2"));
		        chat.Add(Language.GetTextValue("Mods.SonicNPC.Dialogue.Hedgehog.StandardDialogue3"));
		        chat.Add(Language.GetTextValue("Mods.SonicNPC.Dialogue.Hedgehog.StandardDialogue4"));
            }
            string chosenChat = chat;

            return chosenChat;
        }

        public override bool CanGoToStatue(bool toKingStatue) {
	        return toKingStatue;
        }

        public override void TownNPCAttackProj(ref int projType, ref int attackDelay) {

	        projType = ProjectileID.MagicMissile;
	        attackDelay = 1;
        }

        public override void TownNPCAttackProjSpeed(ref float multiplier, ref float gravityCorrection, ref float randomOffset) {
	        multiplier = 16f;
            randomOffset = 5f;
        }
    }
}