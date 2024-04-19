using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DQ9TreasureMap
{
    // 未整理
    static class RoutineG
    {
        public static unsafe void Execute(ref uint seed, byte[] _buffer, uint addr)
        {
            var index = (int)(addr - 216) >> 4;
            var strB = _buffer.StructB(index);

            if (strB[12] == 1)
            {
                int addr2;
                int addr3;
                fixed (byte* ptr = _buffer)
                {
                    // strBが参照している2つのstructA型オブジェクトが参照しているstructC型オブジェクトのポインタ
                    addr2 = (int)((uint*)ptr + ((uint*)ptr + addr / 4U)[1] / 4U)[2];
                    addr3 = (int)((uint*)ptr + ((uint*)ptr + addr / 4U)[2] / 4U)[2];
                }

                SubRoutine(_buffer, (uint)addr2, 16U, (uint)addr3, 12U, index, seed.GetRand(8) == 0);
                SubRoutine(_buffer, (uint)addr2, 18U, (uint)addr3, 12U, index, seed.GetRand(8) == 0);
                SubRoutine(_buffer, (uint)addr2, 16U, (uint)addr3, 14U, index, seed.GetRand(8) == 0);
                SubRoutine(_buffer, (uint)addr2, 18U, (uint)addr3, 14U, index, seed.GetRand(8) == 0);
            }
            else if (strB[12] == 2)
            {
                int addr2;
                int addr3;
                fixed (byte* ptr2 = _buffer)
                {
                    addr2 = (int)((uint*)ptr2 + ((uint*)ptr2 + addr / 4U)[1] / 4U)[2];
                    addr3 = (int)((uint*)ptr2 + ((uint*)ptr2 + addr / 4U)[2] / 4U)[2];
                }

                SubRoutine(_buffer, (uint)addr2, 8U, (uint)addr3, 4U, index, seed.GetRand(8) == 0);
                SubRoutine(_buffer, (uint)addr2, 10U, (uint)addr3, 4U, index, seed.GetRand(8) == 0);
                SubRoutine(_buffer, (uint)addr2, 8U, (uint)addr3, 6U, index, seed.GetRand(8) == 0);
                SubRoutine(_buffer, (uint)addr2, 10U, (uint)addr3, 6U, index, seed.GetRand(8) == 0);
            }
        }

        private static void SubRoutine(byte[] _buffer, uint addr1, uint value1, uint addr2, uint value2, int idxB, bool extra)
        {
            // value1/2の値によってL/TかR/Bかが変わる
            // テクニカルなコードしやがってよぉ…
            var num = _buffer[addr1 + value1];
            var num2 = _buffer[addr1 + value1 + 1U];
            var num3 = _buffer[addr2 + value2];
            var num4 = _buffer[addr2 + value2 + 1U];
            if (num == 0 || num2 == 0 || num3 == 0 || num4 == 0) return;

            var floorMap = _buffer.FloorMap();

            var strB = _buffer.StructB(idxB);

            if (strB[12] == 1)
            {
                var top = strB[1];
                for (int y = num2 + 1; y < top + 1; y++)
                {
                    floorMap[(y << 4) + num] = 2;
                }
                for (int y = num4 - 1; y > top; y--)
                {
                    floorMap[(y << 4) + num3] = 2;
                }

                if (num < num3)
                {
                    for (int x = num; x < num3 + 1; x++)
                    {
                        floorMap[x + (top << 4)] = 2;
                    }
                }
                else if (num > num3)
                {
                    for (int x = num3; x < num + 1; x++)
                    {
                        floorMap[x + (top << 4)] = 2;
                    }
                }

                if (!extra) return;

                if (num < num3)
                {
                    for (int m = num3 + 1; m < 16; m++)
                    {
                        int num5 = floorMap[m + (top << 4)];
                        if (num5 != 1 && num5 != 3)
                        {
                            for (int n = num3 + 1; n < m; n++)
                            {
                                floorMap[n + (top << 4)] = 2;
                            }
                            break;
                        }
                    }
                    for (int num6 = num - 1; num6 >= 0; num6--)
                    {
                        int num7 = floorMap[num6 + (top << 4)];
                        if (num7 != 1 && num7 != 3)
                        {
                            for (int num8 = num - 1; num8 > num6; num8--)
                            {
                                floorMap[num8 + (top << 4)] = 2;
                            }
                            break;
                        }
                    }
                }
                else if (num >= num3)
                {
                    for (int num9 = num + 1; num9 < 16; num9++)
                    {
                        int num10 = floorMap[num9 + (top << 4)];
                        if (num10 != 1 && num10 != 3)
                        {
                            for (int num11 = num + 1; num11 < num9; num11++)
                            {
                                floorMap[num11 + (top << 4)] = 2;
                            }
                            break;
                        }
                    }
                    for (int num12 = num3 - 1; num12 >= 0; num12--)
                    {
                        int num13 = floorMap[num12 + (top << 4)];
                        if (num13 != 1 && num13 != 3)
                        {
                            for (int num14 = num3 - 1; num14 > num12; num14--)
                            {
                                floorMap[num14 + (top << 4)] = 2;
                            }
                            break;
                        }
                    }
                }
            }
            else if (strB[12] == 2)
            {
                var left = strB[0];
                for (int num15 = num + 1; num15 < left + 1; num15++)
                {
                    _buffer[num15 + (num2 << 4) + 792] = 2;
                }
                for (int num16 = num3 - 1; num16 > left; num16--)
                {
                    _buffer[num16 + (num4 << 4) + 792] = 2;
                }
                if (num2 < num4)
                {
                    for (int num17 = num2; num17 < num4 + 1; num17++)
                    {
                        _buffer[(num17 << 4) + 792 + left] = 2;
                    }
                }
                else if (num2 > num4)
                {
                    for (int num18 = num4; num18 < num2 + 1; num18++)
                    {
                        _buffer[(num18 << 4) + 792 + left] = 2;
                    }
                }
                
                if (!extra) return;

                if (num2 < num4)
                {
                    for (int num19 = num4 + 1; num19 < 16; num19++)
                    {
                        int num20 = _buffer[(num19 << 4) + 792 + left];
                        if (num20 != 1 && num20 != 3)
                        {
                            for (int num21 = num4 + 1; num21 < num19; num21++)
                            {
                                _buffer[(num21 << 4) + 792 + left] = 2;
                            }
                            break;
                        }
                    }
                    for (int num22 = num2 - 1; num22 >= 0; num22--)
                    {
                        int num23 = _buffer[(num22 << 4) + 792 + left];
                        if (num23 != 1 && num23 != 3)
                        {
                            for (int num24 = num2 - 1; num24 > num22; num24--)
                            {
                                _buffer[(num24 << 4) + 792 + left] = 2;
                            }
                        }
                    }
                }
                else
                {
                    int num25;
                    if (num2 < num4)
                    {
                        num25 = num2;
                    }
                    else
                    {
                        num25 = num2 + 1;
                    }
                    while (num25 < 16)
                    {
                        int num26 = _buffer[(num25 << 4) + 792 + left];
                        if (num26 != 1 && num26 != 3)
                        {
                            for (int num27 = num2 + 1; num27 < num25; num27++)
                            {
                                _buffer[(num27 << 4) + 792 + left] = 2;
                            }
                            break;
                        }
                        num25++;
                    }
                    for (num25 = num4 - 1; num25 >= 0; num25--)
                    {
                        int num28 = _buffer[(num25 << 4) + 792 + left];
                        if (num28 != 1 && num28 != 3)
                        {
                            for (int num29 = num4 - 1; num29 > num25; num29--)
                            {
                                _buffer[(num29 << 4) + 792 + left] = 2;
                            }
                            break;
                        }
                    }
                }
            }
        }

    }
}
