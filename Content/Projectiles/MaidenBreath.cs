using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace ChairMarker.Content.Projectiles
{
    internal class MaidenBreath : ModProjectile
    {
        public override void SetDefaults()
        {
            Projectile.CloneDefaults(ProjectileID.ChlorophyteBullet);
            Projectile.damage = 1;
            Projectile.aiStyle = 1;
            // AIType = ProjectileID.ChlorophyteBullet;
        }

        public override void AI()
        {
            // Dust.NewDust(Projectile.position, 4, 4, DustID.Pixie, 0, 0, 0, Color.Red, 1f);
            Dust dust;
            dust = Main.dust[Dust.NewDust(Projectile.position, 0, 0, DustID.Dirt, 0, 0, 50, randomColour(), 0.9302325f)];
            dust.noGravity = true;
            dust.fadeIn = 0.20930234f;

        }

        private Color randomColour()
        {
            Random rnd = new Random();
            int roll = rnd.Next(0, 80);

            return new Color(roll, roll / 64, 200);
        }
    }
}
