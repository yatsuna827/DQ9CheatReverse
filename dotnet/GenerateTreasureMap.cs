using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;

using static DQ9TreasureMap.TreasureMapDataTable;

namespace DQ9TreasureMap
{
    class GenerateTreasureMap
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

        public static byte[] CalculateDetail(uint mapSeed, uint mapRank)
        {
            var seed = mapSeed;
            var _details = new byte[20];

            for (int j = 0; j < 12; j++)
                _ = seed.GetRand(100);

            // マップタイプID(1-o(ry)
            _details[3] = (byte)Seek1(ref seed, TableA);
            // フロア数(0-oriented?)
            _details[1] = (byte)Seek2(ref seed, TableB, mapRank);
            // 敵ランク
            _details[2] = (byte)Seek2(ref seed, TableC, mapRank);
            // ボスID（1-oritented注意）
            _details[0] = (byte)Seek4(ref seed, mapRank, TableD, TableE); 

            // 用途不明
            // どうも宝箱ランクの決定と同じテーブルを見ているようだが…？
            _details[8] = (byte)seed.GetRand(1, 2);
            _details[9] = (byte)seed.GetRand(1, 2);
            _details[10] = (byte)seed.GetRand(1, 3);
            _details[11] = (byte)seed.GetRand(1, 4);
            _details[12] = (byte)seed.GetRand(2, 5);
            _details[13] = (byte)seed.GetRand(2, 6);
            _details[14] = (byte)seed.GetRand(3, 7);
            _details[15] = (byte)seed.GetRand(3, 8);
            _details[16] = (byte)seed.GetRand(4, 9);
            _details[17] = (byte)seed.GetRand(5, 9);
            _details[18] = (byte)seed.GetRand(1, 10);
            _details[19] = (byte)seed.GetRand(4, 10);

            // 5: 地図名1 のIndex
            // 6: 地図名2 のIndex
            // 7: 地図名3 のIndex
            _details[5] = (byte)Seek2(ref seed, TableH, _details[2]);
            _details[6] = (byte)Seek2(ref seed, TableI, _details[0]);
            _details[7] = (byte)Seek2(ref seed, TableG, _details[1]);

            // 地図レベル
            var treasureMapLevel = ((_details[0] + _details[1] + _details[2] - 4) * 3 + (int)seed.GetRand(11) - 5).Clamp(1, 99);
            _details[4] = (byte)treasureMapLevel;

            return _details;
        }

        // テーブルを前から走査していって、和がrを超えない最大の項を返す
        //   要するにエンカウントテーブルみたいな感じの構造体
        // 引数にはTableAが渡されてきていて、size=5らしい
        private static uint Seek1(ref uint seed, byte[] table)
        {
            var r = seed.GetRand(100u);

            var n = 0u;
            for (int i = 0; i < table.Length; i+=4)
            {
                n += table[i + 1];
                if (r < n) return table[i];
            }

            return 0u;
        }

        // tableは4個ずつ1まとまりのデータが並んでいる
        // valueがt[0]~t[1]の範囲ならt[2]~t[3]の乱数を発生させる、ということらしい
        // そういうことするなよ…という気持ち
        private static uint Seek2(ref uint seed, byte[] table, uint value)
        {
            for (int i = 0; i < table.Length; i += 4)
            {
                if (value >= table[i] && value <= table[i + 1])
                {
                    var (min, max) = (table[i + 2], table[i + 3]);
                    return seed.GetRand(min, max);
                }
            }

            return 0u;
        }

        // なにこれ…。
        // たぶんデータ構造がカスなせいでゴミみたいなコードを書いている
        private static uint Seek4(ref uint seed, uint mapRank, byte[] table1, byte[] table2)
        {
            for (int i = 0; i < table1.Length; i += 4)
            {
                if (mapRank >= table1[i] && mapRank <= table1[i + 1])
                {
                    var (min, max) = (table1[i + 2], table1[i + 3]);

                    var n = 0u;
                    for (int k = min; k <= max; k++)
                        n += table2[(k - 1) * 2 + 1];

                    var r = seed.GetRand(n);

                    var m = 0u;
                    for (uint k = min; k <= max; k++)
                    {
                        m += table2[(k - 1) * 2 + 1];
                        if (r < m) return k;
                    }
                    break;
                }
            }

            return 0u;
        }


        public static byte[][] CreateDungeonDetails(uint mapSeed, int totalFloors, int rank, int mapType)
        {
            var _dungeonInfo = new byte[16][];
            for (int i = 0; i < 16; i++) _dungeonInfo[i] = new byte[1336];

            for (int f = 0; f < totalFloors; f++)
            {
                CreateDungeonDetails(mapSeed, f, rank, mapType, _dungeonInfo[f]);
            }

            // 宝箱があるのは3階以降
            // for (uint f = 2; f < totalFloors; f++) GenerateTreasureBoxRank(mapSeed, f, _details[2], _dungeonInfo);

            return _dungeonInfo;
        }

        public static void CreateDungeonDetails(uint mapSeed, int floor, int rank, int mapType, byte[] floorInfo)
        {
            // floor は 0-indexed なので+1して1-indexedの階層に直す
            var floorNumber = (uint)floor + 1u;
            var seed = mapSeed + floorNumber;

            var floorSize =
                floor < 4 ? (seed % 5) + 10 :
                floor < 8 ? (seed % 4) + 12 :
                floor < 12 ? (seed % 3) + 14 : 16;

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

            // 宝箱
            if (floorInfo[0] <= 2)
            {
                floorInfo[8] = 0;
            }
            else
            {
                RoutineK.Execute(ref seed, floorInfo);
            }

            RoutineZ.Execute(floorInfo, floor, rank, mapType);
        }


        private static void GenerateTreasureBoxRank(uint mapSeed, int floor, uint baseRank, byte[,] _dungeonInfo)
        {
            var floorNumber = (uint)floor + 1u;
            var seed = mapSeed + floorNumber;
            // ランク 4階ごとに+1されていくらしい
            // 1-indexedなので-1する
            var rank = baseRank + floor / 4 - 1;

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
