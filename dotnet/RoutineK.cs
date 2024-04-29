using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DQ9TreasureMap
{
    // 未整理
    static class PlaceTreasureBox
    {
        public static void Execute(ref uint seed, byte[] floorInfo, byte boxes)
        {
            if (boxes == 0) return;

            var floorMap = floorInfo.FloorMap();

            int cnt = 0;
            int boxIdx = 0;
            for (; ; )
            {
                var strC = floorInfo.StructC((int)seed.GetRand(floorInfo[23]));

                var x = (int)seed.GetRand(strC[0], strC[2]);
                var y = (int)seed.GetRand(strC[1], strC[3]);
                var tile = (int)floorMap[x + (y * 16)];

                if ((cnt < 100 && (floorInfo[0] == 1 || floorInfo[0] == 3)) || tile == 4 || tile == 5 || tile == 6)
                {
                    cnt++;
                }
                else
                {
                    // 0x06(宝箱)を置く
                    floorMap[x + (y * 16)] = 6;
                    floorInfo[13 + boxIdx * 2] = (byte)x;
                    floorInfo[14 + boxIdx * 2] = (byte)y;

                    boxIdx++;
                    if (boxIdx >= boxes) break;
                }
            }
        }

    }
}
