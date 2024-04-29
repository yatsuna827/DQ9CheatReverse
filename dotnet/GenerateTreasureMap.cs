using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;

using static DQ9TreasureMap.TreasureMapDataTable;

namespace DQ9TreasureMap
{
    enum MapType
    {
        洞窟 = 0,
        遺跡 = 1,
        氷 = 2,
        水 = 3,
        火山 = 4
    }

    class TreasureMap
    {
        public ushort Seed { get; }
        public ushort MapRank { get; }

        public MapType MapType { get; }
        public byte TotalFloors { get; }

        public byte BaseRank { get; }

        public string Boss { get; }

        public string Name { get; }
        public byte Level { get; }

        public TreasureMap(ushort seed, ushort mapRank, MapType mapType, byte totalFloors, byte baseRank, string boss, string name, byte level)
        {
            Seed = seed;
            MapRank = mapRank;

            MapType = mapType;
            TotalFloors = totalFloors;
            BaseRank = baseRank;
            Boss = boss;
            Name = name;
            Level = level;
        }

        public override string ToString()
        {
            return $"{Name} {MapType} Lv.{Level} {TotalFloors}F {BaseRank} {Boss}";
        }
    }

    static class GenerateTreasureMap
    {
        // 16階層分のフロアデータ
        // 0: 階層番号
        // 1: なにかのフラグ？
        // 2: フロアのWidth (MapSeed + Floor) % n + mの形で決まっている
        // 3: フロアのHeight
        // 4: 階段のx座標
        // 5: 階段のy座標
        // 6:
        // 7: 

        // 8: 宝箱の個数
        //  9, 10, 11, 12: 宝箱のランク(つまり1フロア当たり最大で4個？)
        // 13, 15, 17, 19: 宝箱のx座標
        // 14, 16, 18, 20: 宝箱のy座標

        // 21: 1で初期化され、routineFの中でインクリメントされる, 最大15
        // 22: 0で初期化され、routineFの中でインクリメントされる, 最大14?
        // 23: 謎。

        // 謎だけど12byteで1セット
        // 24:
        // 25:
        // 26: Width-2が格納される
        // 27: Height-2が格納される
        // 28: routineAを実行してあったら立つフラグ
        // 29: routineEを実行してあったら立つフラグ
        // 31:
        // 32:
        // 33,34,35,36: なんかのぽいんた
        // 
        // 792 ~ 1047: フロア構造データ, 左上からx軸方向に16マスずつ
        // - 792から256(=16*16)byteぶん確保されている
        // - デフォルトは01で埋められる

        public static TreasureMap Get(ushort mapSeed, ushort mapRank)
        {
            var seed = (uint)mapSeed;

            for (int j = 0; j < 12; j++)
                _ = seed.GetRand(100);

            // マップタイプID(1-o(ry)
            var mapType = seed.GenerateMapType();
            // フロア数(0-oriented?)
            var totalFloors = (byte)seed.GenerateTotalFloors(mapRank);
            // 敵ランク
            var rank = (byte)seed.GenerateEnemyRank(mapRank);
            // ボスID（1-oritented注意）
            var bossId = (byte)seed.GenerateBossId(mapRank);

            // 用途不明
            // どうも宝箱ランクの決定と同じテーブルを見ているようだが…？
            {
                seed.GetRand(1, 2);
                seed.GetRand(1, 2);
                seed.GetRand(1, 3);
                seed.GetRand(1, 4);
                seed.GetRand(2, 5);
                seed.GetRand(2, 6);
                seed.GetRand(3, 7);
                seed.GetRand(3, 8);
                seed.GetRand(4, 9);
                seed.GetRand(5, 9);
                seed.GetRand(1, 10);
                seed.GetRand(4, 10);
            }

            var name1 = seed.GenerateMapName1(rank);
            var name2 = seed.GenerateMapName2(bossId);
            var name3 = seed.GenerateMapName3(totalFloors, mapType);

            // 地図レベル
            var treasureMapLevel = (byte)((bossId + totalFloors + rank - 3) * 3 + (int)seed.GetRand(11) - 5).Clamp(1, 99);

            return new TreasureMap(mapSeed, mapRank, mapType, totalFloors, rank, BossTable[bossId], $"{name1}{name2}の{name3}", treasureMapLevel);
        }

        public static TreasureMap Generate(ushort timer0, ushort rankBase)
        {
            var seed = timer0 - 0x802u;

            var rand = seed.GetRand();
            int nextRank = (int)(rankBase + (rand % (uint)(rankBase / 10 * 2 + 1)) - (rankBase / 10)).Clamp(1, 248);
            Console.WriteLine($"MapRank: {nextRank:X2}");

            var mapSeed = seed.GetRand() & 0x7FFFu;
            Console.WriteLine($"MapSeed:{mapSeed:X4}");
            var location = seed.GetRand(1, 150);
            Console.WriteLine($"Location: {location:X}");

            return Get((ushort)mapSeed, (ushort)(nextRank));
        }

        public static string GenerateMetadata(uint seed, ushort rankBase)
        {
            var mapRank = (uint)(rankBase + seed.GetRand((uint)(rankBase / 10 * 2 + 1)) - (rankBase / 10)).Clamp(1, 248);

            var mapSeed = seed.GetRand() & 0x7FFFu;

            var location = seed.GetRand(1, 150);

            seed = mapSeed * 0xC8333031u + 0x69ACC4C4u;

            // マップタイプID(1-o(ry)
            var mapType = seed.GenerateMapType();
            // フロア数(0-oriented?)
            var totalFloors = (byte)seed.GenerateTotalFloors(mapRank);
            // 敵ランク
            var rank = (byte)seed.GenerateEnemyRank(mapRank);
            // ボスID（1-oritented注意）
            var bossId = (byte)seed.GenerateBossId(mapRank);

            // 用途不明
            seed = seed * 0xC8333031u + 0x69ACC4C4u;

            var name1 = seed.GenerateMapName1(rank);
            var name2 = seed.GenerateMapName2(bossId);
            var name3 = seed.GenerateMapName3(totalFloors, mapType);

            // 地図レベル
            var treasureMapLevel = (byte)((bossId + totalFloors + rank - 3) * 3 + (int)seed.GetRand(11) - 5).Clamp(1, 99);

            return $"{name1}{name2}の{name3} Lv.{treasureMapLevel} {location:X2}";
        }


        private static MapType GenerateMapType(ref this uint seed)
        {
            var r = seed.GetRand(100);
            if (r < 30) return MapType.洞窟; // 土
            if (r < 70) return MapType.遺跡; // 遺跡
            if (r < 80) return MapType.氷; // 氷
            if (r < 90) return MapType.水; // 水
            return MapType.火山; // 火山
        }

        private static uint GenerateTotalFloors(ref this uint seed, uint mapRank)
        {
            if (mapRank < 56) return seed.GetRand(2, 4);
            if (mapRank < 76) return seed.GetRand(4, 6);
            if (mapRank < 101) return seed.GetRand(6, 10);
            if (mapRank < 121) return seed.GetRand(8, 12);
            if (mapRank < 141) return seed.GetRand(10, 14);
            if (mapRank < 181) return seed.GetRand(10, 16);
            if (mapRank < 201) return seed.GetRand(11, 16);
            if (mapRank < 221) return seed.GetRand(12, 16);
            return seed.GetRand(14, 16);
        }

        private static uint GenerateEnemyRank(ref this uint seed, uint mapRank)
        {
            if (mapRank < 56) return seed.GetRand(1, 3);
            if (mapRank < 76) return seed.GetRand(2, 4);
            if (mapRank < 101) return seed.GetRand(3, 5);
            if (mapRank < 121) return seed.GetRand(4, 6);
            if (mapRank < 141) return seed.GetRand(4, 6);
            if (mapRank < 181) return seed.GetRand(5, 7);
            if (mapRank < 201) return seed.GetRand(6, 9);
            if (mapRank < 221) return seed.GetRand(8, 9);

            seed.Advance(); // ここだけはmin==maxでもseedが進む
            return 9; // 固定値
        }

        private static uint GenerateBossId(ref this uint seed, uint mapRank)
        {
            (int, int) f()
            {
                if (mapRank < 61) return (0, 2);
                if (mapRank < 81) return (1, 4);
                if (mapRank < 101) return (2, 6);
                if (mapRank < 121) return (3, 6);
                if (mapRank < 141) return (4, 8);
                if (mapRank < 161) return (5, 8);
                if (mapRank < 181) return (6, 9);
                if (mapRank < 201) return (7, 11);
                return (0, 11);
            }

            var (l, r) = f();

            var nums = new uint[12] { 100, 100, 75, 75, 50, 50, 30, 20, 20, 20, 10, 10 };

            // num[l] ~ num[r]
            var span = nums.Skip(l).Take(r - l + 1).ToArray();
            var rand = seed.GetRand((uint)span.Sum(_ => _));

            for (int i = 0; i < span.Length; i++)
            {
                if (rand < span[i]) return (uint)(l + i);

                rand -= span[i];
            }

            throw new Exception("Never");
        }

        public static string[] BossTable = [
            "黒竜丸", "ハヌマーン", "スライムジェネラル",
            "Sキラーマシン", "イデアラゴン", "ブラッドナイト",
            "アトラス", "怪力軍曹イボイノス", "邪眼皇帝アウルート", "魔剣神レパルド", "破壊神フォロボス", "グレイナル"
        ];

        private static string GenerateMapName1(ref this uint seed, uint enemyRank)
        {
            var table = new string[]
            {
                "はかなき",
                "ちいさな",
                "うす暗き",
                "ゆらめく",
                "ざわめく",
                "ねむれる",

                "怒れる",
                "呪われし",

                "放たれし",
                "けだかき",
                "わななく",
                "残された",
                "見えざる",
                "あらぶる",
                "とどろく",
                "大いなる"
            };

            if (enemyRank < 3) return table[0 + seed.GetRand(5)];
            if (enemyRank < 5) return table[3 + seed.GetRand(5)];
            if (enemyRank < 7) return table[6 + seed.GetRand(6)];
            if (enemyRank < 9) return table[6 + seed.GetRand(10)];
            return table[11 + seed.GetRand(5)];
        }

        private static string GenerateMapName2(ref this uint seed, uint bossId)
        {
            var table = new[]
            {
                "花", "岩", "風",
                "空", "獣", "夢",
                "影", "大地", "運命",
                "魂", "闇", "光",
                "魔神", "星々", "悪霊", "神々"
            };

            // 黒竜丸, ハヌマーン, スライムジェネラル
            if (bossId < 3) return table[0 + seed.GetRand(6)]; // 花,岩,風,空,獣,夢

            // Sキラーマシン, イデアラゴン, ブラッドナイト
            if (bossId < 6) return table[3 + seed.GetRand(6)]; // 空,獣,夢, 影, 大地, 運命

            // アトラス, イボイノス, アウルート
            if (bossId < 9) return table[6 + seed.GetRand(6)]; // 影, 大地, 運命, 魂, 闇, 光

            // レパルド, フォロボス, グレイナル
            return table[9 + seed.GetRand(7)]; // 魂, 闇, 光, 魔人, 星々, 悪霊, 神々
        }

        private static string GenerateMapName3(ref this uint seed, uint totalFloors, MapType mapType)
        {
            var masterTable = new string[][]
            {
                // 自然タイプ
                ["洞くつ", "坑道", "アジト", "道", "墓場", "巣", "世界", "奈落"],
                // 遺跡タイプ
                ["地下道", "坑道", "アジト", "道", "墓場", "遺跡", "世界", "迷宮"],
                // 氷タイプ
                ["洞くつ", "雪道", "氷穴", "雪原", "墓場", "凍土", "世界", "氷河"],
                // 水タイプ
                ["洞くつ", "沼地", "地底湖", "湿原", "墓場", "水脈", "世界", "眠る地"],
                // 火山タイプ
                ["洞くつ", "坑道", "火口", "牢ごく", "墓場", "巣", "世界", "じごく"],
            };
            var table = masterTable[(int)mapType];

            if (totalFloors < 4) return table[0 + seed.GetRand(2)];
            if (totalFloors < 6) return table[0 + seed.GetRand(3)];
            if (totalFloors < 8) return table[0 + seed.GetRand(4)];
            if (totalFloors < 10) return table[1 + seed.GetRand(4)];
            if (totalFloors < 12) return table[1 + seed.GetRand(5)];
            if (totalFloors < 14) return table[2 + seed.GetRand(5)];
            if (totalFloors < 16) return table[3 + seed.GetRand(5)];
            return table[5 + seed.GetRand(3)];
        }


        public static byte[][] CreateDungeonDetails(this TreasureMap treasureMap)
        {
            var _dungeonInfo = new byte[16][];
            for (int i = 0; i < 16; i++) _dungeonInfo[i] = new byte[1336];

            for (int f = 1; f <= treasureMap.TotalFloors; f++)
            {
                CreateDungeonDetails(treasureMap.Seed, f, treasureMap.BaseRank, treasureMap.MapType, _dungeonInfo[f-1]);
            }

            // 宝箱があるのは3階以降
            // for (uint f = 2; f < totalFloors; f++) GenerateTreasureBoxRank(mapSeed, f, _details[2], _dungeonInfo);

            return _dungeonInfo;
        }

        private static void CreateDungeonDetails(uint mapSeed, int floor, int rank, MapType mapType, byte[] floorInfo)
        {
            var floorNumber = (uint)floor;
            var seed = mapSeed + floorNumber;

            var floorSize =
                floor <= 4 ? (seed % 5) + 10 :
                floor <= 8 ? (seed % 4) + 12 :
                floor <= 12 ? (seed % 3) + 14 : 16;

            floorInfo[0] = (byte)floorNumber;
            floorInfo[1] = 0;
            floorInfo[2] = (byte)floorSize;
            floorInfo[3] = (byte)floorSize;
            floorInfo[8] = 0;

            floorInfo.FloorMap().Fill(1);

            floorInfo[21] = 1;
            floorInfo[22] = 0;
            floorInfo[23] = 0;

            // フロアを大まかな区画に分割する
            RoutineF.Execute(ref seed, floorInfo, floorSize);

            // 道を作ったりする
            for (int i = 0; i < floorInfo[21]; i++)
            {
                var created = RoutineC.Execute(ref seed, floorInfo, i, floorInfo[23]);
                if (created) floorInfo[23]++;
            }

            // 
            for (int i = 0; i < floorInfo[21]; i++)
            {
                RoutineX.Execute(ref seed, floorInfo, i);
            }

            for (int i = 0; i < floorInfo[22]; i++)
            {
                RoutineG.Execute(ref seed, floorInfo, (uint)((i << 4) + 216));
            }

            // 外周を壁で埋める
            for (int i = 0; i < floorInfo[2]; i++)
            {
                floorInfo[i + 792] = 1;
                floorInfo[((floorInfo[3] - 1) << 4) + i + 792] = 1;
            }
            for (int i = 0; i < floorInfo[3]; i++)
            {
                floorInfo[(i << 4) + 792] = 1;
                floorInfo[(i << 4) + 792 + floorInfo[2] - 1] = 1;
            }

            RoutineJ.Execute(ref seed, floorInfo);

            // 宝箱の配置決定
            var boxes = floorInfo[0] >= 3 ? (byte)(seed.GetRand(3) + 1) : (byte)0;
            floorInfo[8] = boxes;
            PlaceTreasureBox.Execute(ref seed, floorInfo, boxes);

            // 諸々をメモリにロードする
            // シンボル出現テーブルの処理もこの中
            var enemyTable = RoutineZ.Execute(floorInfo, floor, rank, mapType);

            //Console.WriteLine($"{floor}F");
            foreach(var idx  in enemyTable)
            {
                //Console.WriteLine(Monsters[idx]);
            }
            //Console.WriteLine();
        }


        private static void GenerateTreasureBoxRank(uint mapSeed, int floor, uint baseRank, byte[,] _dungeonInfo)
        {
            var seed = mapSeed + (uint)floor;
            // ランク 4階ごとに+1されていくらしい
            // 1-indexedなので-1する
            var rank = baseRank + (floor - 1) / 4 - 1;

            // 宝箱の数*2回 乱数を進める
            seed.Advance(_dungeonInfo[floor, 8] * 2u);

            for (int i = 0; i < _dungeonInfo[floor, 8]; i++)
            {
                _dungeonInfo[floor, 9 + i] = (byte)GetItemRank(ref seed, TableN[rank * 4 + 1], TableN[rank * 4 + 2]);
            }
        }
        private static uint GetItemRank(ref uint seed, uint value1, uint value2)
        {
            int num = (int)seed.GetRand();
            float num2 = num;
            num2 -= 1f;
            int num3 = (int)(value2 - value1 + 1U);
            num2 = num3 * num2 / 32767f;
            return (uint)num2 + value1;
        }
    }

    static class TreasureMapData
    {
        public static string[] TreasureMapName1 = new string[16] {
    "はかなき",
    "ちいさな",
    "うす暗き",
    "ゆらめく",
    "ざわめく",
    "ねむれる",
    "怒れる",
    "呪われし",
    "放たれし",
    "けだかき",
    "わななく",
    "残された",
    "見えざる",
    "あらぶる",
    "とどろく",
    "大いなる"
  };

        public static string[] TreasureMapName2 = new string[16] {
    "花の",
    "岩の",
    "風の",
    "空の",
    "獣の",
    "夢の",
    "影の",
    "大地の",
    "運命の",
    "魂の",
    "闇の",
    "光の",
    "魔神の",
    "星々の",
    "悪霊の",
    "神々の"
  };
    
  public static string[][] TreasureMapName3 = new string[5][]
  {
    // 0,1,2,3,4,5,6,7
    // 自然タイプ
    new string[8] { "洞くつ", "坑道", "アジト", "道", "墓場", "巣", "世界", "奈落" },
    // 8,1,2,3,4,9,6,10
    // 遺跡タイプ
    new string[8] { "地下道", "坑道", "アジト", "道", "墓場", "遺跡", "世界", "迷宮" },
    // 0,11,12,13,4,14,6,15
    // 氷タイプ
    new string[8] { "洞くつ", "雪道", "氷穴", "雪原", "墓場", "凍土", "世界", "氷河" },
    // 0,16,17,18,19,6,20,
    // 水タイプ
    new string[8] { "洞くつ", "沼地", "地底湖", "湿原", "墓場", "水脈", "世界", "眠る地" },
    // 0,1,21,22,4,5,6,23
    // 火山タイプ
    new string[8] { "洞くつ", "坑道", "火口", "牢ごく", "墓場", "巣", "世界", "じごく" },
  };

    }

    public static class LCG
    {
        public static uint Advance(ref this uint seed)
        {
            return seed = (seed * 0x41c64e6du + 0x3039u);
        }

        public static uint Advance(ref this uint seed, uint n)
        {
            for (int i=0;i<n;i++)
                seed = (seed * 0x41c64e6du + 0x3039u);

            return seed;
        }

        public static uint GetRand(ref this uint seed)
        {
            return (seed.Advance() >> 16) & 0x7FFF;
        }

        public static uint GetRand(ref this uint seed, uint m)
        {
            if (m == 1) return 0;

            var rand = seed.GetRand();
            if (m == 0) return 0;

            return rand % m;
        }

        public static uint GetRand(ref this uint seed, uint min, uint max)
        {
            return min + seed.GetRand(max - min + 1u);
        }
    }

    public static class Util
    {
        public static Span<byte> StructA(this byte[] floorInfo, int index)
            => floorInfo.AsSpan(24 + index * 12, 12);
        public static Span<byte> StructB(this byte[] floorInfo, int index)
            => floorInfo.AsSpan(216 + index * 16, 16);
        public static Span<byte> StructC(this byte[] floorInfo, int index)
            => floorInfo.AsSpan(472 + index * 20, 20);
        public static Span<byte> FloorMap(this byte[] floorInfo)
            => floorInfo.AsSpan(792, 0x100);

        public static T Clamp<T>(this T n, T min, T max)
            where T: IComparable<T>
        {
            if (n.CompareTo(min) < 0) return min;
            if (n.CompareTo(max) > 0) return max;
            return n;
        }
    }

}
