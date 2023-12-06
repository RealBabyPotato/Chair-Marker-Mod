using Terraria;
using Terraria.ModLoader;
using Terraria.ID;
using Terraria.GameContent.Creative;
using ChairMarker.Content.ModPlayers;
using ChairMarker.Content.Projectiles;

namespace ChairMarker.Content.Items.Weapons
{
    internal class Whirling_Death : ModItem
    {
        public override void SetDefaults()
        {
            Item.width = 20;
            Item.height = 40;

            Item.useStyle = ItemUseStyleID.Swing;
            Item.useTime = 20;
            Item.useAnimation = 20;
            Item.autoReuse = true;

            Item.noMelee = true;
            Item.scale = 1.2f;

            Item.DamageType = DamageClass.Melee;
            Item.damage = 1; 
            Item.knockBack = 0f;

            Item.value = Item.sellPrice(copper: 1);
            Item.rare = ItemRarityID.Blue;

            Item.UseSound = SoundID.Item1;

            Item.shoot = ModContent.ProjectileType<DravenAxe>(); ;
            Item.shootSpeed = 10f;
        }

        public override bool CanUseItem(Player player)
        {
            var localModPlayer = player.GetModPlayer<LocalModPlayer>();
            return localModPlayer.activeAxes < 3;
        }

        public override void AddRecipes()
        {
            CreateRecipe()
                .AddIngredient(ItemID.DirtBlock, 1)
                .AddTile(TileID.Anvils)
                .Register();
        }

        public override bool? UseItem(Player player)
        {
            var localModPlayer = player.GetModPlayer<LocalModPlayer>();
            localModPlayer.activeAxes++;
            Item.damage = localModPlayer.axeDamage;
            return true;
        }

    }
}
