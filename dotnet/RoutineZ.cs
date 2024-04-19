using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static DQ9TreasureMap.TreasureMapDataTable;

namespace DQ9TreasureMap
{
    static class RoutineZ
    {
        private static bool IsWall(this byte tile)
            => tile == 1 || tile == 3;

        public static unsafe void Execute(byte[] floorInfo, int floor, int enemyRank, int mapType)
        {
            var details2 = new byte[20];

            var floorMap = floorInfo.FloorMap();

            // 「壁でないマスの上/左にある壁マス」の数を数えている…
            floorInfo[1306] = 0;
            floorInfo[1307] = 0;
            for (int y = 1; y < 15; y++)
            {
                for (int x = 1; x < 15; x++)
                {
                    if (floorMap[x + (y << 4)].IsWall()) continue;

                    floorInfo[1306]++;

                    if (!floorMap[x + (y - 1 << 4)].IsWall())
                        floorInfo[1307]++;

                    if (!floorMap[(y << 4) + x - 1].IsWall())
                        floorInfo[1307]++;
                }
            }

            var floorRank = (byte)(enemyRank + floor / 4);

            var idxK = ((mapType - 1) * 12 + (floorRank - 1)) * 9;
            // TableKは[i+0]番目にlength、[i+1]~[i+len]が実際のテーブル という、二次元配列を正規化?したものになっている
            var lenK = TableK[idxK];
            Console.WriteLine($"{floor+1}F {floorRank}");

            floorInfo[1304] = floorRank;
            floorInfo[1305] = (byte)lenK;
            floorInfo[1312] = 0;

            fixed (byte* flootInfoPtr = &floorInfo[0])
            {
                // よくわからん
                ((int*)flootInfoPtr)[327] = (flootInfoPtr[1306] << 4) + 0x13_20;
                int num7 = 0x10_20 - ((flootInfoPtr[1306] << 4) + flootInfoPtr[1307] * 8);
                if (num7 < 0)
                {
                    ((uint*)flootInfoPtr)[327] += (uint)((flootInfoPtr[1307] + (num7 + 7) / 8 - 1) * 8);
                }
                else
                {
                    ((uint*)flootInfoPtr)[327] += (uint)(flootInfoPtr[1307] * 8);
                }


                // よくわからん
                ((uint*)flootInfoPtr)[327] += 4U;
                ((uint*)flootInfoPtr)[327] += (uint)(flootInfoPtr[1307] * 8);
                ((uint*)flootInfoPtr)[327] += (uint)(lenK * 20);


                // ここから敵テーブルの設定だと思う
                for (int i = 1; i <= lenK; i++)
                {
                    int idxM = TableK[idxK + i];
                    var idxL = TableM[idxM];

                    ((uint*)flootInfoPtr)[327] += (uint)(TableL[idxL] + 8);
                }


                // ここかぁ？
                var limit = 11216 >= ((uint*)flootInfoPtr)[327]
                    ? (int)(11216 - ((uint*)flootInfoPtr)[327]) / 20 : 0;
                Console.WriteLine($"Limit: {limit}");

                byte cnt = 0;
                for (int i = 1; i <= lenK && i < limit; i++)
                {
                    var val = TableK[idxK + i];
                    if (38 <= val && val <= 40) continue;

                    Console.WriteLine(val);
                    ((short*)flootInfoPtr)[1314 / 2 + cnt] = (short)val;
                    cnt++;
                }
                flootInfoPtr[1313] = cnt;

                Console.WriteLine();

                // おそらく特殊フロアがあるかどうかを記録している
                if (cnt == 0) 
                    flootInfoPtr[1312] = (byte)(flootInfoPtr[1312] | 1);
                else if (cnt == 1) 
                    flootInfoPtr[1312] = (byte)(flootInfoPtr[1312] | 2);
                else if (lenK >= limit) 
                    flootInfoPtr[1312] = (byte)(flootInfoPtr[1312] | 4);

                // おそらく特殊フロアがあるかどうかを記録している
                if (cnt == 1)
                {
                    details2[14] = flootInfoPtr[1314];
                    details2[15] = flootInfoPtr[1315];
                }
                else
                {
                    details2[12] = (byte)(details2[12] | flootInfoPtr[1312]);
                }
            }

            //return details2;
        }
    }
}
