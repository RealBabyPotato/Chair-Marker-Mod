using ChairMarker.Content.ModPlayers;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace ChairMarker.Content.Projectiles
{
    internal class DeathBeam : ModProjectile
    {
        bool flag = false;
        public override void SetStaticDefaults()
        {
            // Main.projFrames[Projectile.type] = 2;
        }
        public override void SetDefaults()
        {
            Projectile.damage = 100;
            Projectile.penetrate = -1;
            Projectile.tileCollide = false;
            Projectile.DamageType = DamageClass.Magic;
            Projectile.alpha = 255;
        }

        public override void AI()
        {
            
            if (Projectile.ai[0] < 5 * 60)
            {
                Projectile.ai[0]++;
                foreach(NPC npc in Main.npc)
                {
                    if (npc.active && !npc.friendly && !npc.dontTakeDamage) //&& npc.localAI[1] != 1)
                    {
                        //Projectile.NewProjectile(Projectile.GetSource_FromThis(), npc.Center, new Vector2(0, 0), ModContent.ProjectileType<FriendlyDeathBeam>(), 0, 0);
                        Dust.NewDust(npc.position, npc.width, npc.height, DustID.FrostHydra, 0.01f * Projectile.ai[0], 0.01f * Projectile.ai[0], (int)(300 / 200* Projectile.ai[0]), Color.Turquoise, 0.005f * Projectile.ai[0]);
                        // npc.localAI[1] = 1;
                    }
                }
                flag = true;
                Dust.NewDust(Main.player[Projectile.owner].position, Main.player[Projectile.owner].width, Main.player[Projectile.owner].height, DustID.FrostHydra, 0.01f * Projectile.ai[0], 0.01f * Projectile.ai[0], (int)(300 / 255* Projectile.ai[0]), Color.PaleVioletRed, 0.01f * Projectile.ai[0]);
                Main.player[Projectile.owner].frozen = true;
                return;
            }
            var localModPlayer = Main.player[Projectile.owner].GetModPlayer<LocalModPlayer>();

            foreach (NPC npc in Main.npc)
            {
                if (npc.active && !npc.friendly && !npc.dontTakeDamage)
                {
                    NPC.HitInfo hitInfo = new NPC.HitInfo();
                    hitInfo.Damage = (int)Math.Floor(localModPlayer.baseLukeDamage) + (int)Math.Floor(localModPlayer.extraDamage);
                    Main.player[Projectile.owner].frozen = false;

                    npc.StrikeNPC(hitInfo, true, false);
                }
            }
            Projectile.Kill();
        }

        public override void OnHitNPC(NPC target, NPC.HitInfo hit, int damageDone)
        {
            Dust.NewDust(Projectile.position, Projectile.width, Projectile.height, DustID.BubbleBurst_Purple, 0.25f, 0.25f, 150, Color.Purple, 1.5f);
        }

    }
}
