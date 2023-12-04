using ChairMarker.Content.ModPlayers;
using ChairMarker.Content.Projectiles;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace ChairMarker.Content.Items.Weapons
{
    internal class LukesStaff : ModItem
    {
        public override void SetStaticDefaults()
        {
            Item.staff[Type] = true;
        }
        public override void SetDefaults()
        {
            Item.damage = 100;

            Item.noMelee = true;
            Item.DamageType = DamageClass.Magic;
            Item.width = 20;
            Item.height = 40;

            Item.useTime = 300;
            Item.useAnimation = 300;
            Item.rare = ItemRarityID.Yellow;

            Item.useStyle = ItemUseStyleID.HoldUp;
            Item.knockBack = 1f;

            Item.autoReuse = false;

            Item.shoot = ModContent.ProjectileType<DeathBeam>(); ;
            Item.mana = 200;
            Item.UseSound = SoundID.Item43;
        }

        public override bool? UseItem(Player player)
        {
            var localModPlayer = player.GetModPlayer<LocalModPlayer>();
            Item.damage = (int)Math.Floor(localModPlayer.baseLukeDamage) + (int)Math.Floor(localModPlayer.extraDamage);
            return true;
        }

    }
}
