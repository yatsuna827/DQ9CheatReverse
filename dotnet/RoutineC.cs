using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DQ9TreasureMap
{
    static class RoutineC
    {
        public static unsafe bool Execute(ref uint seed, byte[] floorInfo, int idxA, int idxC)
        {
            var strA = floorInfo.StructA(idxA);
            var strC = floorInfo.StructC(idxC);

            var L = (uint)strA[0];
            var R = (uint)strA[2];
            var T = (uint)strA[1];
            var B = (uint)strA[3];

            if (R - L + 1 < 3) return false;
            if (B - T + 1 < 3) return false;

            var (left, top) = seed.GetRand(2) != 0
                ? (seed.GetRand(L + 0, L + (R - L + 1) / 3), seed.GetRand(T + 0, T + (B - T + 1) / 3))
                : (seed.GetRand(L + 1, L + (R - L + 1) / 3), seed.GetRand(T + 1, T + (B - T + 1) / 3));

            var (right, bottom) = seed.GetRand(2) != 0
                ? (seed.GetRand(L + (R - L + 1) / 3 * 2, R - 0), seed.GetRand(T + (B - T + 1) / 3 * 2, B - 0))
                : (seed.GetRand(L + (R - L + 1) / 3 * 2, R - 1), seed.GetRand(T + (B - T + 1) / 3 * 2, B - 1));

            strC[0] = (byte)left;
            strC[1] = (byte)top;
            strC[2] = (byte)right;
            strC[3] = (byte)bottom;
            strC[4..].Fill(0);

            fixed (byte* ptr = &strA[8])
            {
                *(int*)ptr = 472 + idxC * 20;
            }

            for (var y = (int)top; y <= bottom; y++)
                for (var x = (int)left; x <= right; x++)
                    floorInfo.FloorMap()[x + (y << 4)] = 0;

            SubRoutine(ref seed, floorInfo, idxC);
            return true;
        }

        private static void SubRoutine(ref uint seed, byte[] floorInfo, int index)
        {
            var strC = floorInfo.StructC(index);

            var L = (uint)strC[0];
            var R = (uint)strC[2];
            var T = (uint)strC[1];
            var B = (uint)strC[3];

            if (L == 0 || R == 0 || T == 0 || B == 0) return;

            var floorMap = floorInfo.FloorMap();

            var WIDTH = R - L + 1;
            if (WIDTH < 5)
            {
                var x1 = strC[12] = (byte)seed.GetRand(L, R);
                var y1 = strC[13] = (byte)T;
                var x2 = strC[16] = (byte)seed.GetRand(L, R);
                var y2 = strC[17] = (byte)B;

                floorMap[x1 + (y1 << 4)] = 8;
                floorMap[x2 + (y2 << 4)] = 8;
            }
            else
            {
                var mid = L + WIDTH / 2 - 1;

                var x1 = strC[12] = (byte)seed.GetRand(L, mid);
                var y1 = strC[13] = (byte)T;
                var x2 = strC[14] = (byte)seed.GetRand(mid + 1, R);
                var y2 = strC[15] = (byte)T;
                var x3 = strC[16] = (byte)seed.GetRand(L, mid);
                var y3 = strC[17] = (byte)B;
                var x4 = strC[18] = (byte)seed.GetRand(mid + 1, R);
                var y4 = strC[19] = (byte)B;

                floorMap[x1 + (y1 << 4)] = 8;
                floorMap[x2 + (y2 << 4)] = 8;
                floorMap[x3 + (y3 << 4)] = 8;
                floorMap[x4 + (y4 << 4)] = 8;
            }

            var HEIGHT = B - T + 1;
            if (HEIGHT < 5)
            {
                var x1 = strC[4] = (byte)L;
                var y1 = strC[5] = (byte)seed.GetRand(T, B);
                var x2 = strC[8] = (byte)R;
                var y2 = strC[9] = (byte)seed.GetRand(T, B);

                floorMap[x1 + (y1 << 4)] = 8;
                floorMap[x2 + (y2 << 4)] = 8;
            }
            else
            {
                var mid = T + HEIGHT / 2 - 1;

                var x1 = strC[4] = (byte)L;
                var y1 = strC[5] = (byte)seed.GetRand(T, mid);
                var x2 = strC[6] = (byte)L;
                var y2 = strC[7] = (byte)seed.GetRand(mid + 1, B);
                var x3 = strC[8] = (byte)R;
                var y3 = strC[9] = (byte)seed.GetRand(T, mid);
                var x4 = strC[10] = (byte)R;
                var y4 = strC[11] = (byte)seed.GetRand(mid + 1, B);

                floorMap[x1 + (y1 << 4)] = 8;
                floorMap[x2 + (y2 << 4)] = 8;
                floorMap[x3 + (y3 << 4)] = 8;
                floorMap[x4 + (y4 << 4)] = 8;
            }
        }

    }

}
