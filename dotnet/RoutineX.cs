using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DQ9TreasureMap
{
    // GenerateFloorMap
    static class RoutineX
    {
        public static unsafe void Execute(ref uint seed, byte[] floorInfo, int index)
        {
            var strA = floorInfo.StructA(index);

            int ptrToC;
            fixed (byte* ptr = &strA[8]) { ptrToC = *(int*)ptr; }
            var strC = floorInfo.StructC((ptrToC - 472) / 20);
            var L = (int)strC[0];
            var R = (int)strC[2];
            var T = (int)strC[1];
            var B = (int)strC[3];

            var floorMap = floorInfo.FloorMap();

            // StructCが保持している領域の中央
            var midX = (uint)((R - L + 1) / 2);
            var midY = (uint)((B - T + 1) / 2);

            var flag = (floorInfo[1] == 0 && seed.GetRand(16) == 0);
            if (flag) floorInfo[1] = 1;

            var height1 = (uint)(flag ? T : T - (strA[1] - 1));
            for (int x = L; x <= R; x++)
            {
                var tile = floorMap[x + (T << 4)];
                if (tile == 1 || tile == 3) continue;

                if (seed.GetRand(2) != 0)
                {
                    var len = (int)seed.GetRand(height1);
                    for (int y = 1; y <= len; y++)
                    {
                        if (floorMap[x + (T - y << 4)] != 8)
                            floorMap[x + (T - y << 4)] = 0;
                    }
                }
                else
                {
                    var upper = floorMap[x + (T - 1 << 4)];
                    if (tile == 8 || upper == 1 || upper == 8) continue;

                    var len = (int)seed.GetRand(midY + 1);
                    for (int y = T; y < T + len; y++)
                    {
                        if (floorMap[x + (y << 4)] == 8 || floorMap[x + (y + 1 << 4)] == 1
                            || (floorMap[(x + 1) + (y << 4)] != 1 && floorMap[(x + 1) + (y + 1 << 4)] == 1)
                            || (floorMap[(x - 1) + (y << 4)] != 1 && floorMap[(x - 1) + (y + 1 << 4)] == 1)) break;

                        floorMap[x + (y << 4)] = 1;
                    }
                }
            }

            var height2 = (uint)(flag ? (floorInfo[3] - B) : (strA[3] + 1 - B));
            for (int x = L; x <= R; x++)
            {
                var tile = floorMap[x + (B << 4)];
                if (tile == 1 || tile == 3) continue;

                if (seed.GetRand(2) != 0)
                {
                    var len = (int)seed.GetRand(height2);
                    for (int y = B + 1; y <= B + len; y++)
                    {
                        if (floorMap[x + (y << 4)] != 8)
                            floorMap[x + (y << 4)] = 0;
                    }
                }
                else
                {
                    var under = floorMap[x + (B + 1 << 4)];
                    if (tile == 8 || under == 1 || under == 8) continue;

                    var rand = (int)seed.GetRand(midY + 1);
                    for (int y = 0; y < rand; y++)
                    {
                        if (floorMap[x + (B - y << 4)] == 8 || floorMap[x + (B - y - 1 << 4)] == 1
                        || (floorMap[(x + 1) + ((B - y) << 4)] != 1 && floorMap[(x + 1) + (B - y - 1 << 4)] == 1)
                        || (floorMap[(x - 1) + ((B - y) << 4)] != 1 && floorMap[(x - 1) + (B - y - 1 << 4)] == 1)) break;

                        floorMap[x + ((B - y) << 4)] = 1;
                    }
                }
            }

            var width1 = (uint)(flag ? L : L - (strA[0] - 1));
            for (int y = T; y <= B; y++)
            {
                var tile = floorMap[(y << 4) + L];
                if (tile == 1 || tile == 3) continue;

                if (seed.GetRand(0, 1) != 0)
                {
                    var len = seed.GetRand(width1);
                    for (int k = 1; k <= len; k++)
                    {
                        if (floorMap[(y << 4) + L - k] != 8)
                            floorMap[(y << 4) + L - k] = 0;
                    }
                }
                else
                {
                    int left = floorMap[(L - 1) + (y << 4)];
                    if (tile == 8 || left == 1 || left == 8) continue;

                    int len = (int)seed.GetRand(midX + 1);
                    for (int x = L; x < L + len; x++)
                    {
                        if (floorMap[x + (y << 4)] == 8 || floorMap[x + 1 + (y << 4)] == 1
                            || (floorMap[x + ((y + 1) << 4)] != 1 && floorMap[(x + 1) + (y + 1 << 4)] == 1)
                            || (floorMap[x + ((y - 1) << 4)] != 1 && floorMap[(x + 1) + ((y - 1) << 4)] == 1)) break;

                        floorMap[x + (y << 4)] = 1;
                    }
                }
            }

            var width2 = (uint)(flag ? (floorInfo[2] - R) : (strA[2] + 1 - R));
            for (int y = T; y <= B; y++)
            {
                var tile = floorMap[R + (y << 4)];
                if (tile == 1 || tile == 3) continue;

                if (seed.GetRand(0, 1) != 0)
                {
                    var len = (int)seed.GetRand(width2);
                    for (int x = R; x <= R + len; x++)
                    {
                        if (floorMap[x + (y << 4)] != 8)
                            floorMap[x + (y << 4)] = 0;
                    }
                }
                else
                {
                    // Tを参照してるのはさすがにバグでは…？
                    int right = floorMap[(T + 1) + (y << 4)];
                    if (tile == 8 || right == 1 || right == 8) continue;

                    var len = (int)seed.GetRand(0, midX);
                    for (int x = 0; x < len; x++)
                    {
                        if (floorMap[(y << 4) + R - x] == 8 || floorMap[(y << 4) + R - x - 1] == 1
                            || (floorMap[((y + 1) << 4) + R - x] != 1 && floorMap[((y + 1) << 4) + R - x - 1] == 1)
                            || (floorMap[((y - 1) << 4) + R - x] != 1 && floorMap[((y - 1) << 4) + R - x - 1] == 1)) break;

                        floorMap[(y << 4) + R - x] = 1;
                    }
                }
            }
        }

    }
}
