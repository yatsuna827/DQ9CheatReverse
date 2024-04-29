using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace DQ9TreasureMap
{
    static class RoutineF
    {
        public static void Execute(ref uint seed, byte[] floorInfo, uint floorSize)
        {
            var first = floorInfo.StructA(0);
            first[0] = 1;
            first[1] = 1;
            first[2] = (byte)(floorSize - 2);
            first[3] = (byte)(floorSize - 2);
            first[4] = 0;
            first[5] = 0;

            Dispatch(ref seed, floorInfo, 0);
        }

        private static void Dispatch(ref uint seed, byte[] floorInfo, int index)
        {
            if (floorInfo[21] >= 15) return;

            var strA = floorInfo.StructA(index);

            if (strA[5] != 0)
            {
                var hasSeen = !ProcessVertical(ref seed, floorInfo, index, floorInfo[21], floorInfo[22]);
                if (hasSeen) return;
            }
            else if (strA[4] != 0)
            {
                var hasSeen = !ProcessHorizontal(ref seed, floorInfo, index, floorInfo[21], floorInfo[22]);
                if (hasSeen) return;
            }
            else
            {
                var result = (seed.GetRand(2) == 0)
                    ? ProcessVertical(ref seed, floorInfo, index, floorInfo[21], floorInfo[22])
                    : ProcessHorizontal(ref seed, floorInfo, index, floorInfo[21], floorInfo[22]);

                if (!result) return;
            }

            Next(ref seed, floorInfo, index);
        }

        private static bool ProcessVertical(ref uint seed, byte[] floorInfo, int idx, int idxA, int idxB)
        {
            var current = floorInfo.StructA(idx);
            if (current[4] != 0) return false;

            var height = (uint)(current[3] - current[1] + 1);
            if (height <= 6) return false;

            var y = current[1] + (int)seed.GetRand(height - 6) + 3;
            for (int x = current[0]; x <= current[2]; x++)
                floorInfo.FloorMap()[x + (y << 4)] = 3;

            var strA = floorInfo.StructA(idxA);
            var strB = floorInfo.StructB(idxB);

            strB[0] = current[0];
            strB[1] = (byte)y;
            strB[2] = current[2];
            strB[3] = (byte)y;

            strA[0] = current[0];
            strA[1] = (byte)(y + 1);
            strA[2] = current[2];
            strA[3] = current[3];

            strA[4] = 0;
            strA[5] = 0;
            current[3] = (byte)(y - 1);
            current[4] = 1;

            var ptr = MemoryMarshal.Cast<byte, int>(strB);
            ptr[1] = 24 + idx * 12;
            ptr[2] = 24 + idxA * 12;

            strB[12] = 1;

            return true;
        }

        private static bool ProcessHorizontal(ref uint seed, byte[] floorInfo, int idx, int idxA, int idxB)
        {
            var current = floorInfo.StructA(idx);
            if (current[5] != 0) return false;

            var width = (uint)(current[2] - current[0] + 1);
            if (width <= 6) return false;

            var x = current[0] + (int)seed.GetRand(width - 6) + 3;
            for (int y = current[1]; y <= current[3]; y++)
                floorInfo.FloorMap()[x + (y << 4)] = 3;

            var strA = floorInfo.StructA(idxA);
            var strB = floorInfo.StructB(idxB);

            strB[0] = (byte)x;
            strB[1] = current[1];
            strB[2] = (byte)x;
            strB[3] = current[3];

            strA[0] = (byte)(x + 1);
            strA[1] = current[1];
            strA[2] = current[2];
            strA[3] = current[3];

            strA[4] = 0;
            strA[5] = 0;
            current[2] = (byte)(x - 1);
            current[5] = 1;

            var ptr = MemoryMarshal.Cast<byte, int>(strB);
            ptr[1] = 24 + idx * 12;
            ptr[2] = 24 + idxA * 12;

            strB[12] = 2;
            return true;
        }

        private static void Next(ref uint seed, byte[] floorInfo, int idx)
        {
            int next = floorInfo[21];

            floorInfo[21]++;
            floorInfo[22]++;

            if (seed.GetRand(2) == 0)
            {
                Dispatch(ref seed, floorInfo, next);
                Dispatch(ref seed, floorInfo, idx);
            }
            else
            {
                Dispatch(ref seed, floorInfo, idx);
                Dispatch(ref seed, floorInfo, next);
            }
        }

    }

}
