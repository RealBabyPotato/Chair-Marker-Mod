using ChairMarker.Content.Buffs;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace ChairMarker.Content.Items.Weapons
{
    internal class TurretBlueprint : ModItem
    {
        public override void SetDefaults()
        {
            Item.CloneDefaults(ItemID.StaffoftheFrostHydra);

            Item.UseSound = new Terraria.Audio.SoundStyle($"ChairMarker/Content/Sounds/TurretBlueprint")
            {
                Volume = 0.5f,
            };

            Item.DamageType = DamageClass.Summon;
            Item.buffType = ModContent.BuffType<TurretBuff>();

            Item.damage = 30;
            Item.knockBack = 0f;
            Item.mana = 20;

            Item.useTime = 20;
            Item.useAnimation = 20;

            Item.useStyle = ItemUseStyleID.Thrust;
            Item.rare = ItemRarityID.Yellow;
           
        }
        public override void ModifyShootStats(Player player, ref Vector2 position, ref Vector2 velocity, ref int type, ref int damage, ref float knockback)
        {
            position = Main.MouseWorld;
        }

        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            player.AddBuff(Item.buffType, 2);

            var projectile = Projectile.NewProjectileDirect(source, position, velocity, type, damage, knockback, Main.myPlayer);
            
            projectile.originalDamage = Item.damage;

            return false;
        }
    }
}
