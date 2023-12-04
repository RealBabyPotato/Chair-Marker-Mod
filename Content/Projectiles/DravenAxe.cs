using ChairMarker.Content.ModPlayers;
using Microsoft.Xna.Framework;
using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace ChairMarker.Content.Projectiles
{
    internal class DravenAxe : ModProjectile
    {
        bool hasHit = false;
        bool playerHasCaught = false;
        public override void SetDefaults()
        {
            Projectile.DamageType = DamageClass.Ranged;
            Projectile.width = 20;
            Projectile.height = 40;
            Projectile.scale = 1.2f;

            Projectile.friendly = true;
            Projectile.hostile = false;

            Projectile.penetrate = 2;
            Projectile.ignoreWater = true;

            Projectile.damage = 20;

            Projectile.tileCollide = true;
        }

        public override void AI()
        {
            Projectile.rotation += MathHelper.ToDegrees(10);
            if (!hasHit) { Projectile.velocity.Y += 0.1f; }
            else
            {
                Player player = Main.player[Projectile.owner];
                Projectile.velocity.Y += player.moveSpeed * 0.2f;
            }

            if (hasHit)
            {
                Dust.NewDust(Projectile.position, Projectile.width, Projectile.height, DustID.GoldCoin, Projectile.velocity.X * 0.25f, Projectile.velocity.Y * 0.25f, 150, Color.Gold, 2f);
            } else if (Main.player[Projectile.owner].GetModPlayer<LocalModPlayer>().streak > 0)
            {
                Dust.NewDust(Projectile.position, Projectile.width, Projectile.height, DustID.Lava, Projectile.velocity.X * 0.25f, Projectile.velocity.Y * 0.25f, 150, Color.Gold, 2f);
            }

        }

        public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
        {
            hasHit = true;
            Projectile.damage = Main.player[Projectile.owner].GetModPlayer<LocalModPlayer>().axeDamage;

            Vector2 normal = Vector2.Normalize(target.Center - Projectile.Center);
            Projectile.velocity = Vector2.Reflect(new Vector2(Projectile.velocity.X *= 1.2f, Projectile.velocity.Y *= 0.8f), normal);

        }

        public override bool? Colliding(Rectangle projHitbox, Rectangle targetHitbox)
        {
            var localModPlayer = Main.player[Projectile.owner].GetModPlayer<LocalModPlayer>();
            // on collide with caster
            if (Projectile.owner == Main.myPlayer && projHitbox.Intersects(Main.player[Projectile.owner].getRect()) && hasHit && !playerHasCaught)
            {
                playerHasCaught = true;
                localModPlayer.streak++;
                Projectile.Kill();
                Main.NewText($"Streak: {localModPlayer.streak}");
            }

            return base.Colliding(projHitbox, targetHitbox);
        }

        public override void OnKill(int timeLeft)
        {
            var localModPlayer = Main.player[Projectile.owner].GetModPlayer<LocalModPlayer>();
            if(!playerHasCaught) { localModPlayer.streak = 0; }
            localModPlayer.activeAxes--;
        }

    }
}
