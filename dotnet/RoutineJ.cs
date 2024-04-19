using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DQ9TreasureMap
{
    // 未整理
    static class RoutineJ
    {
        private static bool IsWall(this byte tile)
            => tile == 1 || tile == 3;

        private static bool IsValidTile(this (int x, int y) p, ref Span<byte> floorMap)
        {
            var (x, y) = p;

            var upperIsWall = floorMap[(y - 1 << 4) + x].IsWall();
            var underIsWall = floorMap[(y + 1 << 4) + x].IsWall();
            var leftIsWall = floorMap[(y << 4) + x - 1].IsWall();
            var rightIsWall = floorMap[(y << 4) + x + 1].IsWall();

            if (upperIsWall && underIsWall && leftIsWall && rightIsWall) return false;
            if (upperIsWall && rightIsWall) return false;
            if (upperIsWall && leftIsWall) return false;
            if (underIsWall && rightIsWall) return false;
            if (underIsWall && leftIsWall) return false;

            return true;
        }

        public static void Execute(ref uint seed, byte[] _buffer)
        {
            var floorMap = _buffer.FloorMap();

            int num = 0;
            int num2 = 0;
            int x, y;
            for (; ; )
            {
                var index1 = (int)seed.GetRand(_buffer[23]);
                var strC1 = _buffer.StructC(index1);

                x = (int)seed.GetRand(strC1[0], strC1[2]);
                y = (int)seed.GetRand(strC1[1], strC1[3]);

                if (num2 < 100)
                {
                    if (!(x, y).IsValidTile(ref floorMap))
                    {
                        num2++;
                        continue;
                    }

                    if (SubRoutine(ref floorMap, x, y))
                    {
                        num2++;
                        continue;
                    }
                }

                // 昇り階段
                floorMap[x + (y << 4)] = 4;
                _buffer[4] = (byte)x;
                _buffer[5] = (byte)y;

                var ASC = (x, y);

                for (; ; )
                {
                    var index2 = (int)seed.GetRand(_buffer[23]);
                    if (index2 == index1 && num < 25)
                    {
                        num++;
                        continue;
                    }

                    var strC2 = _buffer.StructC(index2);
                    x = (int)seed.GetRand(strC2[0], strC2[2]);
                    y = (int)seed.GetRand(strC2[1], strC2[3]);

                    if (index1 != y || x != ASC.x || y != ASC.y) break;
                }


                if (!(x, y).IsValidTile(ref floorMap))
                {
                    num2++;
                }
                else
                {
                    var underIsWall = floorMap[x + (y + 1 << 4)].IsWall();
                    var leftIsWall = floorMap[x + (y << 4) - 1].IsWall();
                    if (!underIsWall || !leftIsWall) break;

                    num2++;
                }
            }
            
            // 下り階段
            floorMap[x + (y << 4)] = 5;
            _buffer[6] = (byte)x;
            _buffer[7] = (byte)y;
        }

        private static bool SubRoutine(ref Span<byte> floorMap, int x, int y)
        {
            if (floorMap[(x + (y << 4))].IsWall()) return false;

            byte flags = 0;
            if (floorMap[(x + (y - 1 << 4) - 1)].IsWall())
                flags |= 0b_10000000;

            if (floorMap[(x + (y - 1 << 4) + 1)].IsWall())
                flags |= 0b_00100000;

            if (floorMap[(x + (y + 1 << 4) + 1)].IsWall())
                flags |= 0b_00001000;

            if (floorMap[(x + (y + 1 << 4) - 1)].IsWall())
                flags |= 0b_00000010;


            if (floorMap[(x + (y - 1 << 4))].IsWall())
                flags |= 0b_11100000;

            if (floorMap[(x + (y << 4) + 1)].IsWall())
                flags |= 0b_00111000;

            if (floorMap[(x + (y + 1 << 4))].IsWall())
                flags |= 0b_00001110;

            if (floorMap[(x + (y << 4) - 1)].IsWall())
                flags |= 0b_10000011;

            return flags == 234 
                || flags == 171 
                || flags == 174 
                || flags == 186 
                || flags == 163 
                || flags == 142 
                || flags == 58 
                || flags == 232 
                || flags == 184 
                || flags == 226 
                || flags == 139 
                || flags == 46;
        }

    }
}
