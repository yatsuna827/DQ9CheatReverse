using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using static DQ9TreasureMap.TreasureMapDataTable;

namespace DQ9TreasureMap
{
    static class RoutineZ
    {
        private static bool IsWall(this byte tile)
            => tile == 1 || tile == 3;

        public static int[] Execute(byte[] floorInfo, int floor, int enemyRank, MapType mapType)
        {
            var details2 = new byte[20];
            var enemyTable = new List<int>();

            var floorMap = floorInfo.FloorMap();
            

            // 床マスの個数
            floorInfo[1306] = 0;
            // 描画される壁面の個数
            // 壁面は床の上or左のものだけ描画される
            floorInfo[1307] = 0;
            for (int y = 1; y < 15; y++)
            {
                for (int x = 1; x < 15; x++)
                {
                    if (floorMap[x + (y << 4)].IsWall()) continue;

                    floorInfo[1306]++;

                    // 床の上側にある壁
                    if (!floorMap[x + (y - 1 << 4)].IsWall())
                        floorInfo[1307]++;

                    // 床の左側にある壁
                    if (!floorMap[(y << 4) + x - 1].IsWall())
                        floorInfo[1307]++;
                }
            }

            var floorRank = (byte)(enemyRank + (floor - 1) / 4);

            var tableK = TableK[(int)mapType][floorRank - 1];

            floorInfo[1304] = floorRank;
            floorInfo[1305] = (byte)tableK.Length;
            floorInfo[1312] = 0;

            // 用途はわからないけどメモリの消費を記録している
            var usingMemories = 0x1320u + (floorInfo[1306] * 16u);

            // なんらかでフロア構造がメモリを食いすぎるときは切り詰められる？
            int temp = 0x1020 - ((floorInfo[1306] * 16) + (floorInfo[1307] * 8));
            if (temp < 0)
            {
                usingMemories += (uint)((floorInfo[1307] + (temp + 7) / 8 - 1) * 8);
            }
            else
            {
                usingMemories += (uint)(floorInfo[1307] * 8);
            }

            // Console.WriteLine($"over: {temp < 0}");

            usingMemories += (uint)(tableK.Length * 20) + (uint)(floorInfo[1307] * 8) + 4U;


            // そのフロアで出てくる可能性のある敵シンボルのデータをロードする処理
            for (int i = 0; i < tableK.Length; i++)
            {
                int idxM = tableK[i];
                var idxL = TableM[idxM];

                // TableLは必要なbyte数？(モデルのサイズなどによって必要なメモリ量が違う？)
                usingMemories += (uint)(TableL[idxL] + 8);
            }

            // フロアデータ用に確保されたメモリの残り容量から、テーブルに読み込めるスロット数の残りを計算する
            // 敵テーブル1スロットあたり20byte使う?
            var remainingMemories = 0x2BD0 - (int)usingMemories;
            var limit = remainingMemories >= 0 ? remainingMemories / 20 : 0;
            // Console.WriteLine($"remaining: {remainingMemories} bytes");

            // テーブルの作成
            byte cnt = 0;
            var table = MemoryMarshal.Cast<byte, short>(floorInfo.AsSpan(1314));
            for (int i = 0; i < tableK.Length && i < limit; i++)
            {
                var val = tableK[i];
                // ひとくいばこ, ミミック, パンドラボックスは除外する
                if (0x26 <= val && val <= 0x28) continue;

                table[cnt] = (short)val;
                cnt++;

                enemyTable.Add(val);
            }
            MemoryMarshal.Cast<byte, uint>(floorInfo.AsSpan())[327] = usingMemories;
            floorInfo[1313] = cnt;

            // おそらく特殊フロアがあるかどうかを記録している
            if (cnt == 0)
                floorInfo[1312] = (byte)(floorInfo[1312] | 1);
            else if (cnt == 1)
                floorInfo[1312] = (byte)(floorInfo[1312] | 2);
            else if (tableK.Length >= limit)
                floorInfo[1312] = (byte)(floorInfo[1312] | 4);

            // おそらく特殊フロアがあるかどうかを記録している
            if (cnt == 1)
            {
                details2[14] = floorInfo[1314];
                details2[15] = floorInfo[1315];
            }
            else
            {
                details2[12] = (byte)(details2[12] | floorInfo[1312]);
            }

            return enemyTable.ToArray();
        }
    }
}
