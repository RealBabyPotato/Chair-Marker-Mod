using Microsoft.Xna.Framework;
using rail;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;
using static Humanizer.In;

namespace ChairMarker.Content.Projectiles
{
    internal class MaidenMinion : ModProjectile
    {

        private enum ActionState
        {
            Idle, // 0
            Notice, // 1
            Spawn // 2
        }

        public ref float AI_State => ref Projectile.ai[0];
        public ref float timeAlive => ref Projectile.ai[1];
        public ref float moveTime => ref Projectile.ai[2];

        public override void SetStaticDefaults()
        {
            Main.projFrames[Projectile.type] = 9;
            ProjectileID.Sets.MinionTargettingFeature[Projectile.type] = true;

            // Main.projPet[Projectile.type] = true; 

            ProjectileID.Sets.CultistIsResistantTo[Projectile.type] = true; // Make the cultist resistant to this projectile, as it's resistant to all homing projectiles.
        }

        public override void SetDefaults()
        {

            Projectile.friendly = true;
            Projectile.minion = true;
            Projectile.DamageType = DamageClass.Summon;
            Projectile.minionSlots = 0f;
            Projectile.penetrate = -1;
            Projectile.tileCollide = true;

            Projectile.aiStyle = -1;
            Projectile.damage = 12;

            Projectile.width = 26;
            Projectile.height = 32;

            Projectile.scale = 1.5f;
        }

        public override bool OnTileCollide(Vector2 oldVelocity)
        {
            return false;
        }

        public override bool? CanCutTiles()
        {
            return false;
        }

        public override bool MinionContactDamage()
        {
            return true;
        }

        public override void OnSpawn(IEntitySource source)
        {
            Main.NewText($"spawn {(float)ActionState.Spawn} idle {(float)ActionState.Idle} notice {(float)ActionState.Notice}");
            Projectile.frame = 2;
            AI_State = (float)ActionState.Spawn;
        }

        public override void AI()
        {
            timeAlive++;
            Player player = Main.player[Projectile.owner];
            Visuals();
            SearchForTargets(player, out bool foundTarget, out float DistanceFromTarget, out Vector2 targetCenter);

            if(AI_State != (float)ActionState.Spawn && foundTarget)
            {
                AI_State = (float)ActionState.Notice;
            }

            // gravity
            if(AI_State != (float)ActionState.Spawn) Projectile.velocity.Y += 1f;

            if(!foundTarget && AI_State != (float)ActionState.Spawn)
            {
                AI_State = (float)ActionState.Idle;
            }

            Main.NewText(AI_State);

            if (Main.player[Projectile.owner].ownedProjectileCounts[ModContent.ProjectileType<Maiden>()] == 0 && timeAlive > 5 || player.dead || !player.active)
            {
                Projectile.timeLeft = 0;
                Projectile.Kill();
            }

            switch (AI_State)
            {
                case (float)ActionState.Spawn:
                    Spawn();
                    break;

                case (float)ActionState.Idle:
                    Idle();
                    break;

                case (float)ActionState.Notice:
                    Notice(DistanceFromTarget, targetCenter);
                    break;
            }
        }

        public void Spawn()
        {
            // add fx? right now the actionstate is actually changed from Visuals()
        }

        private void SearchForTargets(Player owner, out bool foundTarget, out float distanceFromTarget, out Vector2 targetCenter)
        {
            distanceFromTarget = 700f;
            targetCenter = Projectile.position;
            foundTarget = false;

            if (owner.HasMinionAttackTargetNPC)
            {
                NPC npc = Main.npc[owner.MinionAttackTargetNPC];
                float between = Vector2.Distance(npc.Center, Projectile.Center);

                // Reasonable distance away so it doesn't target across multiple screens
                if (between < 2000f)
                {
                    distanceFromTarget = between;
                    targetCenter = npc.Center;
                    foundTarget = true;
                }
            }

            if (!foundTarget)
            {
                // This code is required either way, used for finding a target
                for (int i = 0; i < Main.maxNPCs; i++)
                {
                    NPC npc = Main.npc[i];

                    if (npc.CanBeChasedBy())
                    {
                        float between = Vector2.Distance(npc.Center, Projectile.Center);
                        bool closest = Vector2.Distance(Projectile.Center, targetCenter) > between;
                        bool inRange = between < distanceFromTarget;
                        bool lineOfSight = Collision.CanHitLine(Projectile.position, Projectile.width, Projectile.height, npc.position, npc.width, npc.height);
                        // Additional check for this specific minion behavior, otherwise it will stop attacking once it dashed through an enemy while flying though tiles afterwards
                        // The number depends on various parameters seen in the movement code below. Test different ones out until it works alright
                        bool closeThroughWall = between < 100f;

                        if (((closest && inRange) || !foundTarget) && (lineOfSight || closeThroughWall))
                        {
                            distanceFromTarget = between;
                            targetCenter = npc.Center;
                            foundTarget = true;
                        }
                    }
                }
            }
            Projectile.friendly = foundTarget;

        }

        public void Idle()
        {
            // add code that tps this to player
        }

        public void Notice(float distanceFromTarget, Vector2 targetCenter)
        {
            float speed = 20f;
            float inertia = 5f;

            Vector2 direction = new Vector2(targetCenter.X, targetCenter.Y) - Projectile.Center;
            direction.Normalize();
            direction *= speed;

            Projectile.velocity = (Projectile.velocity * (inertia - 1) + direction) / inertia;
        }

        public void Visuals()
        {
            int frameSpeedSpawn = 5;
            int frameSpeedIdle = 10;
            Projectile.frameCounter++;

            switch (AI_State)
            {
                case (float)ActionState.Spawn:

                    if (Projectile.frameCounter >= frameSpeedSpawn)
                    {
                        Projectile.frameCounter = 0;
                        Projectile.frame++;

                        if (Projectile.frame == 8)
                        {
                            // switch state when at the end of animation
                            AI_State = (float)ActionState.Idle;
                            Projectile.frameCounter = 0;
                            Projectile.frame = 0;
                        }
                    }

                    break;

                case (float)ActionState.Idle:

                    if (Projectile.frameCounter >= frameSpeedIdle)
                    {
                        Projectile.frameCounter = 0;
                        Projectile.frame++;

                        if (Projectile.frame == 2)
                        {
                            Projectile.frame = 0;
                        }
                    }

                    break;

                case (float)ActionState.Notice:
                    break;
            }
        }
    }
}
