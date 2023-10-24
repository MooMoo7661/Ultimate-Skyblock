using Terraria.ID;
using Terraria.ModLoader;
using Terraria;
using Microsoft.Xna.Framework;
using Terraria.DataStructures;
using Terraria.ObjectData;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System;
using OneBlock.Items.Placeable;

namespace OneBlock.Tiles.Blocks
{
    public class CursedEarth_Item : ModItem {

        public override void SetStaticDefaults()
        {
            Item.ResearchUnlockCount = 30;
        }
        public override void SetDefaults() {
            Item.width = 16;
            Item.height = 16;
            Item.placeStyle = 0;
            Item.consumable = true;
            Item.autoReuse = true;
            Item.useStyle = ItemUseStyleID.Swing;
            Item.maxStack = Item.CommonMaxStack;
            Item.createTile = ModContent.TileType<CursedEarth_Tile>();
            Item.rare = ItemRarityID.LightRed;
            Item.useTime = 10;
            Item.useTurn = true;
            Item.useAnimation = 15;
            
            ItemID.Sets.ShimmerTransformToItem[Type] = ItemID.DirtBlock;
        }

        public override void AddRecipes()
        {
            CreateRecipe(2)
                .AddIngredient(ModContent.ItemType<VolcanicStone>())
                .AddIngredient(ItemID.Hellstone)
                .AddTile(TileID.Hellforge)
                .Register();
        }
    }

    public class CursedEarth_Tile : ModTile {
        public override void SetStaticDefaults() {
            Main.tileMerge[Type][Type] = true;
            Main.tileMerge[Type][TileID.Ash] = true;
            Main.tileMerge[TileID.Ash][Type] = true;
            Main.tileMerge[ModContent.TileType<VolcanicStoneTile>()][Type] = true;
            Main.tileMerge[Type][ModContent.TileType<VolcanicStoneTile>()] = true;
            Main.tileSolid[Type] = true;
            Main.tileMergeDirt[Type] = true;
            Main.tileMerge[Type][TileID.Stone] = true;
            Main.tileLighted[Type] = false;
            Main.tileMerge[Type][TileID.LavaMoss] = true;
            Main.tileNoSunLight[Type] = false;
            Main.tileBlockLight[Type] = true;
            Main.tileMerge[Type][TileID.LavaMoss] = true;
            Main.tileMerge[Type][TileID.AshGrass] = true;
            Main.tileMerge[Type][TileID.HellstoneBrick] = true;
            Main.tileMerge[Type][TileID.Dirt] = true;
            Main.tileMerge[TileID.Dirt][Type] = true;
            Main.tileMerge[TileID.HellstoneBrick][Type] = true;

            TileID.Sets.Stone[Type] = true;

            DustType = DustID.Torch;
            HitSound = SoundID.Tink;
            MinPick = 0;
            MineResist = 2f;

            RegisterItemDrop(ModContent.ItemType<CursedEarth_Item>());

            AddMapEntry(Color.Black);

            TileObjectData.newTile.CopyFrom(TileObjectData.Style1x1);
            TileObjectData.newTile.HookPostPlaceMyPlayer = new PlacementHook(ModContent.GetInstance<CursedEarth_TE>().Hook_AfterPlacement, -1, 0, false);
            TileObjectData.newTile.UsesCustomCanPlace = true;
            TileObjectData.addTile(Type);
        }

        public override void KillTile(int i, int j, ref bool fail, ref bool effectOnly, ref bool noItem) {
            ModContent.GetInstance<CursedEarth_TE>().Kill(i, j);
            base.KillTile(i, j, ref fail, ref effectOnly, ref noItem);
        }

        public override void AnimateIndividualTile(int type, int i, int j, ref int frameXOffset, ref int frameYOffset) {
            if (type == Type) {
                int x = Dust.NewDust(new Vector2(i, j), 8, 8, DustID.Smoke, 0f, -1f, 0, Color.White, 1f);
                Main.dust[x].active = true;
            }
        }

        public override bool CanPlace(int i, int j)
        {
            return true;
        }
    }
    public static class Groups {
        public const int Slime = 0;
        public const int Zombie = 1;
        public const int DemonEye = 2;

        public static class Zombies {
            public const int Zombie = 0;
            public const int Bald = 1;
            public const int Pincushion = 2;
            public const int Slimed = 3;
            public const int Swamp = 4;
            public const int Twiggy = 5;
            public const int Female = 6;
            public const int Torch = 7;
        }

        public static class DemonEyes {
            public const int Demon = 0;
            public const int Cataract = 1;
            public const int Sleepy = 2;
            public const int Dilated = 3;
            public const int Green = 4;
            public const int Purple = 5;
        }
    }

    public class Node {
        public List<Node> children { get; set; }
        public List<float> weights { get; set; }
        public List<float> weightsPrefixSum { get; set; }

        public int value { get; set; }

        public Node(int value) {
            this.value = value;
            children = new();
            weights = new();
            weightsPrefixSum = new();
        }

        public void Add(Node child, float weight) {
            children.Add(child);
            weights.Add(weight);
            weightsPrefixSum.Add(weight + weightsPrefixSum.Sum());
        }

        public void UpdateWeightsPrefixSum() {
            weightsPrefixSum.Clear();
            for (int i = 0; i < weights.Count; i++) {
                weightsPrefixSum.Add(weights[i]);
                for (int j = 0; j < i; j++) {
                    if (i == j) continue;
                    weightsPrefixSum[i] += weights[j];
                }
            }
        }
    };

    public class Tree {
        public Node root { get; set; }

        public Tree() { root = new(NPCID.None); }

        public int SelectNPC(Node? node = null) => Select(node).value;

        public void Build() {
            root.Add(new Node(NPCID.GreenSlime), 0.25f);
            root.Add(new Node(NPCID.Zombie),     0.25f);
            root.Add(new Node(NPCID.DemonEye),   0.25f);
            root.Add(new Node(NPCID.Raven),      0.25f);

            BuildSlimeVariants();
            BuildZombieVariants();
            BuildDemonEyeVariants();
        }

        public void Update() {
            bool night = !Main.dayTime;
            float groups = 1 / (1 + (night ? 2 : 0) + (Main.halloween ? 1 : 0));
            float zombieVariantChance = 1 / (3 + (Main.expertMode ? 1 : 0));

            Console.WriteLine(!Main.dayTime ? groups : 0f);
            Console.WriteLine(groups);

            root.weights[Groups.Slime] =    groups;
            root.weights[Groups.Zombie] =   night ? groups : 0f;
            root.weights[Groups.DemonEye] = night ? groups : 0f;
            root.weights[3] =               Main.halloween ? groups : 0f; // ravens
            root.UpdateWeightsPrefixSum();


            root.children[Groups.Zombie].children[Groups.Zombies.Zombie].weights[3] = Main.expertMode ? zombieVariantChance : 0f;
            root.children[Groups.Zombie].children[Groups.Zombies.Zombie].UpdateWeightsPrefixSum();
            root.children[Groups.Zombie].children[Groups.Zombies.Pincushion].weights[3] = Main.expertMode ? zombieVariantChance : 0f;
            root.children[Groups.Zombie].children[Groups.Zombies.Pincushion].UpdateWeightsPrefixSum();
            root.children[Groups.Zombie].children[Groups.Zombies.Slimed].weights[3] = Main.expertMode ? zombieVariantChance : 0f;
            root.children[Groups.Zombie].children[Groups.Zombies.Slimed].UpdateWeightsPrefixSum();
            root.children[Groups.Zombie].children[Groups.Zombies.Swamp].weights[3] = Main.expertMode ? zombieVariantChance : 0f;
            root.children[Groups.Zombie].children[Groups.Zombies.Swamp].UpdateWeightsPrefixSum();
            root.children[Groups.Zombie].children[Groups.Zombies.Twiggy].weights[3] = Main.expertMode ? zombieVariantChance : 0f;
            root.children[Groups.Zombie].children[Groups.Zombies.Twiggy].UpdateWeightsPrefixSum();
            root.children[Groups.Zombie].children[Groups.Zombies.Female].weights[3] = Main.expertMode ? zombieVariantChance : 0f;
            root.children[Groups.Zombie].children[Groups.Zombies.Female].UpdateWeightsPrefixSum();
            root.children[Groups.Zombie].children[Groups.Zombies.Torch].weights[0] = Main.expertMode ? 0.5f : 1f;
            root.children[Groups.Zombie].children[Groups.Zombies.Torch].weights[1] = Main.expertMode ? 0.5f : 0f;
            root.children[Groups.Zombie].children[Groups.Zombies.Torch].UpdateWeightsPrefixSum();
        }

        private Node Select(Node? node = null) {

            
            if (node is null) node = root;

            if (node.children.Count == 0) return node;

            float num = Main.rand.NextFloat(5.3f);

            if (num > 1f) { return new Node(NPCID.None); }

                for (int i = 0; i < node.children.Count; i++)
                if (num < node.weightsPrefixSum[i]) return Select(node.children[i]);

            return node; // never gets triggered but stupid C# error CS0161 makes me need it here
        }

        private void BuildSlimeVariants() {
            Node slimeVariants = root.children[Groups.Slime];
            slimeVariants.Add(new Node(NPCID.GreenSlime),  0.50f);
            slimeVariants.Add(new Node(NPCID.BlueSlime),   0.30f);
            slimeVariants.Add(new Node(NPCID.PurpleSlime), 0.18f);
            slimeVariants.Add(new Node(NPCID.Pinky),       0.02f);
        }

        private void BuildZombieVariants() {
            float zombieVariantChance = 1 / (3 + (Main.expertMode ? 1 : 0));
            float torchZombieChance = 1 / (1 + (Main.expertMode ? 1 : 0));

            Node zombieGroupVariants = root.children[Groups.Zombie];
            zombieGroupVariants.Add(new Node(NPCID.Zombie), 0.125f);
            zombieGroupVariants.Add(new Node(NPCID.BaldZombie), 0.125f);
            zombieGroupVariants.Add(new Node(NPCID.PincushionZombie), 0.125f);
            zombieGroupVariants.Add(new Node(NPCID.SlimedZombie), 0.125f);
            zombieGroupVariants.Add(new Node(NPCID.SwampZombie), 0.125f);
            zombieGroupVariants.Add(new Node(NPCID.TwiggyZombie), 0.125f);
            zombieGroupVariants.Add(new Node(NPCID.FemaleZombie), 0.125f);
            zombieGroupVariants.Add(new Node(NPCID.TorchZombie), 0.125f);

            Node zombieVariants = zombieGroupVariants.children[Groups.Zombies.Zombie];
            zombieVariants.Add(new Node(NPCID.Zombie), zombieVariantChance);
            zombieVariants.Add(new Node(NPCID.SmallZombie), zombieVariantChance);
            zombieVariants.Add(new Node(NPCID.BigZombie), zombieVariantChance);
            zombieVariants.Add(new Node(NPCID.ArmedZombie), Main.expertMode ? zombieVariantChance : 0f);

            Node baldVariants = zombieGroupVariants.children[Groups.Zombies.Bald];
            baldVariants.Add(new Node(NPCID.BaldZombie), 1 / 3f);
            baldVariants.Add(new Node(NPCID.SmallBaldZombie), 1 / 3f);
            baldVariants.Add(new Node(NPCID.BigBaldZombie), 1 / 3f);

            Node pincushionVariants = zombieGroupVariants.children[Groups.Zombies.Pincushion];
            pincushionVariants.Add(new Node(NPCID.PincushionZombie), zombieVariantChance);
            pincushionVariants.Add(new Node(NPCID.SmallPincushionZombie), zombieVariantChance);
            pincushionVariants.Add(new Node(NPCID.BigPincushionZombie), zombieVariantChance);
            pincushionVariants.Add(new Node(NPCID.ArmedZombiePincussion), Main.expertMode ? zombieVariantChance : 0f);

            Node slimedVaritants = zombieGroupVariants.children[Groups.Zombies.Slimed];
            slimedVaritants.Add(new Node(NPCID.SlimedZombie), zombieVariantChance);
            slimedVaritants.Add(new Node(NPCID.SmallSlimedZombie), zombieVariantChance);
            slimedVaritants.Add(new Node(NPCID.BigSlimedZombie), zombieVariantChance);
            slimedVaritants.Add(new Node(NPCID.ArmedZombieSlimed), Main.expertMode ? zombieVariantChance : 0f);

            Node swampVariants = zombieGroupVariants.children[Groups.Zombies.Swamp];
            swampVariants.Add(new Node(NPCID.SwampZombie), zombieVariantChance);
            swampVariants.Add(new Node(NPCID.SmallSwampZombie), zombieVariantChance);
            swampVariants.Add(new Node(NPCID.BigSwampZombie), zombieVariantChance);
            swampVariants.Add(new Node(NPCID.ArmedZombieSwamp), Main.expertMode ? zombieVariantChance : 0f);

            Node twiggyVariants = zombieGroupVariants.children[Groups.Zombies.Twiggy];
            twiggyVariants.Add(new Node(NPCID.TwiggyZombie), zombieVariantChance);
            twiggyVariants.Add(new Node(NPCID.SmallTwiggyZombie), zombieVariantChance);
            twiggyVariants.Add(new Node(NPCID.BigTwiggyZombie), zombieVariantChance);
            twiggyVariants.Add(new Node(NPCID.ArmedZombieTwiggy), Main.expertMode ? zombieVariantChance : 0f);

            Node femaleVariants = zombieGroupVariants.children[Groups.Zombies.Female];
            femaleVariants.Add(new Node(NPCID.FemaleZombie), zombieVariantChance);
            femaleVariants.Add(new Node(NPCID.SmallFemaleZombie), zombieVariantChance);
            femaleVariants.Add(new Node(NPCID.BigFemaleZombie), zombieVariantChance);
            femaleVariants.Add(new Node(NPCID.ArmedZombieCenx), Main.expertMode ? zombieVariantChance : 0f);

            Node torchVariants = zombieGroupVariants.children[Groups.Zombies.Torch];
            torchVariants.Add(new Node(NPCID.TorchZombie), torchZombieChance);
            torchVariants.Add(new Node(NPCID.ArmedTorchZombie), Main.expertMode ? torchZombieChance : 0f);
        }

        private void BuildDemonEyeVariants() {
            Node demonEyeGroupVariants = root.children[Groups.DemonEye];
            demonEyeGroupVariants.Add(new Node(NPCID.DemonEye), 1 / 6f);
            demonEyeGroupVariants.Add(new Node(NPCID.CataractEye), 1 / 6f);
            demonEyeGroupVariants.Add(new Node(NPCID.SleepyEye), 1 / 6f);
            demonEyeGroupVariants.Add(new Node(NPCID.DialatedEye), 1 / 6f);
            demonEyeGroupVariants.Add(new Node(NPCID.GreenEye), 1 / 6f);
            demonEyeGroupVariants.Add(new Node(NPCID.PurpleEye), 1 / 6f);

            Node demonVariants = demonEyeGroupVariants.children[Groups.DemonEyes.Demon];
            demonVariants.Add(new Node(NPCID.DemonEye), 0.5f);
            demonVariants.Add(new Node(NPCID.DemonEye2), 0.5f);

            Node cataractVariants = demonEyeGroupVariants.children[Groups.DemonEyes.Cataract];
            cataractVariants.Add(new Node(NPCID.CataractEye), 0.5f);
            cataractVariants.Add(new Node(NPCID.CataractEye2), 0.5f);

            Node sleepyVariants = demonEyeGroupVariants.children[Groups.DemonEyes.Sleepy];
            sleepyVariants.Add(new Node(NPCID.SleepyEye), 0.5f);
            sleepyVariants.Add(new Node(NPCID.SleepyEye2), 0.5f);

            Node dilatedVariants = demonEyeGroupVariants.children[Groups.DemonEyes.Dilated];
            dilatedVariants.Add(new Node(NPCID.DialatedEye), 0.5f);
            dilatedVariants.Add(new Node(NPCID.DialatedEye2), 0.5f);

            Node greenVariants = demonEyeGroupVariants.children[Groups.DemonEyes.Green];
            greenVariants.Add(new Node(NPCID.GreenEye), 0.5f);
            greenVariants.Add(new Node(NPCID.GreenEye2), 0.5f);

            Node purpleVariants = demonEyeGroupVariants.children[Groups.DemonEyes.Purple];
            purpleVariants.Add(new Node(NPCID.PurpleEye), 0.5f);
            purpleVariants.Add(new Node(NPCID.PurpleEye2), 0.5f);
        }
    }

    public class CursedEarth_Sys : ModSystem {
        public static Tree spawnPool;
        public static int SpawnRate = 500;

        public override void OnModLoad() {
            spawnPool = new Tree();

            spawnPool.Build();
        }
    }

    public class CursedEarth_TE : ModTileEntity {
        private int TickCount = 0;

        public override bool IsTileValidForEntity(int x, int y) {
            Tile tile = Main.tile[x, y];

            return tile.HasTile && tile.TileType == ModContent.TileType<CursedEarth_Tile>();
        }

        public override int Hook_AfterPlacement(int i, int j, int type, int style, int direction, int alternate) {
            if (Main.netMode == NetmodeID.MultiplayerClient)
                NetMessage.SendData(msgType: MessageID.TileEntityPlacement, number: i, number2: j, number3: Type);
            return Place(i, j);
        }

        public override void OnNetPlace() {
            if (Main.netMode == NetmodeID.Server)
                NetMessage.SendData(MessageID.TileEntitySharing, number: ID, number2: Position.X, number3: Position.Y);
        }

        public override void Update() {
            
            if (TickCount % CursedEarth_Sys.SpawnRate == 0) {
                Vector2 pos = Position.ToWorldCoordinates();

                CursedEarth_Sys.spawnPool.Update();
                Console.WriteLine("BRUH");
                Console.WriteLine(CursedEarth_Sys.spawnPool.root.weights[1]);
                Console.WriteLine("---------");
                Console.WriteLine(CursedEarth_Sys.spawnPool.root.weightsPrefixSum[1]);
                Console.WriteLine("BRUH2");

                Tile tile = Framing.GetTileSafely((int)pos.X, (int)pos.Y - 1);
                if (!tile.HasTile || tile.IsActuated)
                NPC.NewNPC(Entity.GetSource_None(), (int)pos.X, (int)pos.Y - 20, CursedEarth_Sys.spawnPool.SelectNPC());
            }

            TickCount = ++TickCount % CursedEarth_Sys.SpawnRate;
        }
    }
}