using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using Microsoft.Xna.Framework;

namespace ChairMarker.Content.Projectiles
{
    internal class FriendlyDeathBeam : ModProjectile
    {
        public override void SetDefaults()
        {
            Projectile.CloneDefaults(ProjectileID.SandnadoHostileMark);
            Projectile.damage = 0;
            Projectile.knockBack = 0;
            Projectile.friendly = true;
            Projectile.timeLeft = 400;
        }

        public override void AI()
        {
            NPC nearest = FindNearestNPC();
            if (nearest != null)
            {
                // Calculate the direction vector towards the nearest NPC
                Vector2 direction = nearest.Center - Projectile.Center;
                direction.Normalize();

                // Set the velocity to move the projectile towards the nearest NPC
                Projectile.velocity = direction * 1000; // Replace someSpeed with the desired speed

                // Optional: Face the projectile towards the direction it is moving
                Projectile.rotation = Projectile.velocity.ToRotation();

                // Additional logic if needed...
            }
        }

        private NPC FindNearestNPC()
        {
            NPC nearestNPC = null;
            float nearestDistanceSquared = float.MaxValue;

            foreach (NPC npc in Main.npc)
            {
                // Check if the NPC is active, not friendly, and not immune to damage
                if (npc.active && !npc.friendly && !npc.dontTakeDamage)
                {
                    // Calculate the squared distance to the NPC from the projectile
                    float distanceSquared = Vector2.DistanceSquared(Projectile.Center, npc.Center);

                    // Update nearest NPC if the current NPC is closer
                    if (distanceSquared < nearestDistanceSquared)
                    {
                        nearestDistanceSquared = distanceSquared;
                        nearestNPC = npc;
                    }
                }
            }

            return nearestNPC;
        }
    }
}
