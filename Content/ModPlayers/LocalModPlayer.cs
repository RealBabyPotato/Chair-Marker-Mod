using System;
using Terraria.ModLoader;

namespace ChairMarker.Content.ModPlayers
{
    internal class LocalModPlayer : ModPlayer
    {
        public int streak = 0;
        public int activeAxes = 0;
        public int axeBaseDamage = 20;
        public float axeDamageMultiplier = 1.25f;
        public int axeMaxDamage = 125;
        public int axeDamage;

        public float extraDamage = 0f;
        public float baseLukeDamage = 100f;


        public override void PreUpdate()
        {
            axeDamage = Math.Min((int)(axeBaseDamage + (axeBaseDamage * axeDamageMultiplier * streak)), axeMaxDamage);
        }


    }
}
