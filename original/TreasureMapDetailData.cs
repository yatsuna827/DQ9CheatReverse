// TreasureMapDetailData

using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace DQ9_Cheat.GameData
{
	internal class TreasureMapDetailData
	{
		public TreasureMapDetailData()
		{
			if (TreasureMapDetailData._dungeonInfo == null)
			{
				TreasureMapDetailData._dungeonInfo = new byte[16, 1336];
			}
			for (int i = 0; i < 16; i++)
			{
				this._treasureBoxInfoList[i] = new List<TreasureBoxInfo>();
			}
			if (TreasureMapDetailData._candidateRank == null)
			{
				TreasureMapDetailData._candidateRank = new List<byte>();
				for (int j = 1; j <= 99; j++)
				{
					for (int k = 0; k <= 50; k += 5)
					{
						for (int l = 1; l <= 99; l++)
						{
							byte item = (byte)(j + k + l);
							if (!TreasureMapDetailData._candidateRank.Contains(item))
							{
								TreasureMapDetailData._candidateRank.Add(item);
							}
						}
					}
				}
			}
		}

		public ushort MapSeed
		{
			get
			{
				return this._mapSeed;
			}
			set
			{
				this._mapSeed = value;
			}
		}

		public byte MapRank
		{
			get
			{
				return this._mapRank;
			}
			set
			{
				this._mapRank = value;
			}
		}

		public string MapName
		{
			get
			{
				if (this.MapRank < 2 || this.MapRank > 248)
				{
					return "不明な地図";
				}
				return string.Format("{0}{1}{2} LV{3} の地図", new object[]
				{
					TreasureMapDataTable.TreasureMapName1_Table[(int)(this._details[5] - 1)],
					TreasureMapDataTable.TreasureMapName2_Table[(int)(this._details[6] - 1)],
					TreasureMapDataTable.TreasureMapName3_Table[(int)this._name3Index],
					this._details[4]
				});
			}
		}

		public string MapName1
		{
			get
			{
				if (this.MapRank < 2 || this.MapRank > 248)
				{
					return "不明";
				}
				return TreasureMapDataTable.TreasureMapName1_Table[(int)(this._details[5] - 1)];
			}
		}

		public byte MapName1Index
		{
			get
			{
				return this._details[5] - 1;
			}
		}

		public string MapName2
		{
			get
			{
				if (this.MapRank < 2 || this.MapRank > 248)
				{
					return "不明";
				}
				return TreasureMapDataTable.TreasureMapName2_Table[(int)(this._details[6] - 1)];
			}
		}

		public byte MapName2Index
		{
			get
			{
				return this._details[6] - 1;
			}
		}

		public string MapName3
		{
			get
			{
				if (this.MapRank < 2 || this.MapRank > 248)
				{
					return "不明";
				}
				return TreasureMapDataTable.TreasureMapName3_Table[(int)this._name3Index];
			}
		}

		public byte MapName3Index
		{
			get
			{
				return this._name3Index;
			}
		}

		public int MapLevel
		{
			get
			{
				if (this.MapRank < 2 || this.MapRank > 248)
				{
					return 0;
				}
				return (int)this._details[4];
			}
		}

		private uint GetItemRank(uint value1, uint value2)
		{
			int num = (int)this.GenerateRandom();
			float num2 = (float)num;
			num2 -= 1f;
			int num3 = (int)(value2 - value1 + 1U);
			num2 = (float)num3 * num2 / 32767f;
			return (uint)num2 + value1;
		}

		private unsafe void GenerateFloorMap(int floor, uint address)
		{
			int num;
			fixed (byte* ptr = &checked(TreasureMapDetailData._dungeonInfo[(int)((IntPtr)(unchecked((long)floor))), (int)((IntPtr)(unchecked((ulong)(address + 8U))))]))
			{
				num = *(int*)ptr;
			}
			int value = (int)((TreasureMapDetailData._dungeonInfo[floor, num + 2] - TreasureMapDetailData._dungeonInfo[floor, num] + 1) / 2);
			int value2 = (int)((TreasureMapDetailData._dungeonInfo[floor, num + 3] - TreasureMapDetailData._dungeonInfo[floor, num + 1] + 1) / 2);
			int num2 = 0;
			if (TreasureMapDetailData._dungeonInfo[floor, 1] == 0 && this.GenerateRandom(0U, 15U) == 0U)
			{
				num2 = 1;
			}
			int value3;
			int value4;
			int value5;
			int num3;
			if (num2 == 0)
			{
				value3 = (int)(TreasureMapDetailData._dungeonInfo[floor, (int)(address + 3U)] - TreasureMapDetailData._dungeonInfo[floor, num + 3]);
				value4 = (int)(TreasureMapDetailData._dungeonInfo[floor, num] - TreasureMapDetailData._dungeonInfo[floor, (int)address]);
				value5 = (int)(TreasureMapDetailData._dungeonInfo[floor, (int)(address + 2U)] - TreasureMapDetailData._dungeonInfo[floor, num + 2]);
				num3 = (int)(TreasureMapDetailData._dungeonInfo[floor, num + 1] - TreasureMapDetailData._dungeonInfo[floor, (int)(address + 1U)]);
			}
			else
			{
				value3 = (int)(TreasureMapDetailData._dungeonInfo[floor, 3] - TreasureMapDetailData._dungeonInfo[floor, num + 3] - 1);
				value4 = (int)(TreasureMapDetailData._dungeonInfo[floor, num] - 1);
				value5 = (int)(TreasureMapDetailData._dungeonInfo[floor, 2] - TreasureMapDetailData._dungeonInfo[floor, num + 2] - 1);
				num3 = (int)(TreasureMapDetailData._dungeonInfo[floor, num + 1] - 1);
				TreasureMapDetailData._dungeonInfo[floor, 1] = 1;
			}

			for (int i = (int)TreasureMapDetailData._dungeonInfo[floor, num]; i <= (int)TreasureMapDetailData._dungeonInfo[floor, num + 2]; i++)
			{
				int num4 = (int)TreasureMapDetailData._dungeonInfo[floor, i + ((int)TreasureMapDetailData._dungeonInfo[floor, num + 1] << 4) + 792];
				if (num4 != 1 && num4 != 3)
				{
					if (this.GenerateRandom(0U, 1U) == 0U)
					{
						if (num4 != 8)
						{
							int num5 = (int)TreasureMapDetailData._dungeonInfo[floor, i + ((int)(TreasureMapDetailData._dungeonInfo[floor, num + 1] - 1) << 4) + 792];
							if (num5 != 1 && num5 != 8)
							{
								num4 = (int)this.GenerateRandom(0U, (uint)value2);
								int num6 = i - 1;
								for (int j = 0; j < num4; j++)
								{
									if (TreasureMapDetailData._dungeonInfo[floor, i + ((int)TreasureMapDetailData._dungeonInfo[floor, num + 1] + j << 4) + 792] == 8 || TreasureMapDetailData._dungeonInfo[floor, i + ((int)TreasureMapDetailData._dungeonInfo[floor, num + 1] + j + 1 << 4) + 792] == 1 || (TreasureMapDetailData._dungeonInfo[floor, ((int)TreasureMapDetailData._dungeonInfo[floor, num + 1] + j << 4) + i + 1 + 792] != 1 && TreasureMapDetailData._dungeonInfo[floor, ((int)TreasureMapDetailData._dungeonInfo[floor, num + 1] + j + 1 << 4) + i + 1 + 792] == 1) || (TreasureMapDetailData._dungeonInfo[floor, ((int)TreasureMapDetailData._dungeonInfo[floor, num + 1] + j << 4) + num6 + 792] != 1 && TreasureMapDetailData._dungeonInfo[floor, num6 + ((int)TreasureMapDetailData._dungeonInfo[floor, num + 1] + j + 1 << 4) + 792] == 1))
									{
										break;
									}
									TreasureMapDetailData._dungeonInfo[floor, i + ((int)TreasureMapDetailData._dungeonInfo[floor, num + 1] + j << 4) + 792] = 1;
								}
							}
						}
					}
					else
					{
						num4 = (int)this.GenerateRandom(0U, (uint)num3);
						for (int k = 0; k < num4; k++)
						{
							int num7 = i + ((int)(TreasureMapDetailData._dungeonInfo[floor, num + 1] - 1) - k << 4) + 792;
							if (TreasureMapDetailData._dungeonInfo[floor, num7] != 8)
							{
								TreasureMapDetailData._dungeonInfo[floor, num7] = 0;
							}
						}
					}
				}
			}
			for (int l = (int)TreasureMapDetailData._dungeonInfo[floor, num]; l <= (int)TreasureMapDetailData._dungeonInfo[floor, num + 2]; l++)
			{
				num3 = (int)TreasureMapDetailData._dungeonInfo[floor, l + ((int)TreasureMapDetailData._dungeonInfo[floor, num + 3] << 4) + 792];
				if (num3 != 1 && num3 != 3)
				{
					if (this.GenerateRandom(0U, 1U) != 0U)
					{
						num3 = (int)this.GenerateRandom(0U, (uint)value3);
						for (int m = 0; m < num3; m++)
						{
							if (TreasureMapDetailData._dungeonInfo[floor, l + ((int)TreasureMapDetailData._dungeonInfo[floor, num + 3] + m + 1 << 4) + 792] != 8)
							{
								TreasureMapDetailData._dungeonInfo[floor, l + ((int)TreasureMapDetailData._dungeonInfo[floor, num + 3] + m + 1 << 4) + 792] = 0;
							}
						}
					}
					else if (num3 != 8)
					{
						int num8 = (int)TreasureMapDetailData._dungeonInfo[floor, l + ((int)(TreasureMapDetailData._dungeonInfo[floor, num + 3] + 1) << 4) + 792];
						if (num8 != 1 && num8 != 8)
						{
							int num9 = (int)this.GenerateRandom(0U, (uint)value2);
							num3 = l - 1;
							int num10 = 0;
							while (num10 < num9 && TreasureMapDetailData._dungeonInfo[floor, l + ((int)TreasureMapDetailData._dungeonInfo[floor, num + 3] - num10 << 4) + 792] != 8 && TreasureMapDetailData._dungeonInfo[floor, l + ((int)TreasureMapDetailData._dungeonInfo[floor, num + 3] - num10 - 1 << 4) + 792] != 1 && (TreasureMapDetailData._dungeonInfo[floor, ((int)TreasureMapDetailData._dungeonInfo[floor, num + 3] - num10 << 4) + l + 1 + 792] == 1 || TreasureMapDetailData._dungeonInfo[floor, ((int)TreasureMapDetailData._dungeonInfo[floor, num + 3] - num10 - 1 << 4) + l + 1 + 792] != 1) && (TreasureMapDetailData._dungeonInfo[floor, num3 + ((int)TreasureMapDetailData._dungeonInfo[floor, num + 3] - num10 << 4) + 792] == 1 || TreasureMapDetailData._dungeonInfo[floor, num3 + ((int)TreasureMapDetailData._dungeonInfo[floor, num + 3] - num10 - 1 << 4) + 792] != 1))
							{
								TreasureMapDetailData._dungeonInfo[floor, l + ((int)TreasureMapDetailData._dungeonInfo[floor, num + 3] - num10 << 4) + 792] = 1;
								num10++;
							}
						}
					}
				}
			}
			for (int n = (int)TreasureMapDetailData._dungeonInfo[floor, num + 1]; n <= (int)TreasureMapDetailData._dungeonInfo[floor, num + 3]; n++)
			{
				num3 = (int)TreasureMapDetailData._dungeonInfo[floor, (n << 4) + 792 + (int)TreasureMapDetailData._dungeonInfo[floor, num]];
				if (num3 != 1 && num3 != 3)
				{
					if (this.GenerateRandom(0U, 1U) != 0U)
					{
						num3 = (int)this.GenerateRandom(0U, (uint)value4);
						for (int num11 = 0; num11 < num3; num11++)
						{
							if (TreasureMapDetailData._dungeonInfo[floor, (n << 4) + 792 + (int)TreasureMapDetailData._dungeonInfo[floor, num] - 1 - num11] != 8)
							{
								TreasureMapDetailData._dungeonInfo[floor, (n << 4) + 792 + (int)TreasureMapDetailData._dungeonInfo[floor, num] - 1 - num11] = 0;
							}
						}
					}
					else if (num3 != 8)
					{
						int num12 = (int)TreasureMapDetailData._dungeonInfo[floor, (n << 4) + 792 + (int)TreasureMapDetailData._dungeonInfo[floor, num] - 1];
						if (num12 != 1 && num12 != 8)
						{
							int num13 = (int)this.GenerateRandom(0U, (uint)value);
							num3 = n - 1;
							int num14 = 0;
							while (num14 < num13 && TreasureMapDetailData._dungeonInfo[floor, num14 + (n << 4) + (int)TreasureMapDetailData._dungeonInfo[floor, num] + 792] != 8 && TreasureMapDetailData._dungeonInfo[floor, num14 + (n << 4) + (int)TreasureMapDetailData._dungeonInfo[floor, num] + 792 + 1] != 1 && (TreasureMapDetailData._dungeonInfo[floor, num14 + (n + 1 << 4) + (int)TreasureMapDetailData._dungeonInfo[floor, num] + 792] == 1 || TreasureMapDetailData._dungeonInfo[floor, num14 + (n + 1 << 4) + (int)TreasureMapDetailData._dungeonInfo[floor, num] + 792 + 1] != 1) && (TreasureMapDetailData._dungeonInfo[floor, num14 + (num3 << 4) + (int)TreasureMapDetailData._dungeonInfo[floor, num] + 792] == 1 || TreasureMapDetailData._dungeonInfo[floor, num14 + (num3 << 4) + (int)TreasureMapDetailData._dungeonInfo[floor, num] + 792 + 1] != 1))
							{
								TreasureMapDetailData._dungeonInfo[floor, num14 + (n << 4) + (int)TreasureMapDetailData._dungeonInfo[floor, num] + 792] = 1;
								num14++;
							}
						}
					}
				}
			}
			for (int num15 = (int)TreasureMapDetailData._dungeonInfo[floor, num + 1]; num15 <= (int)TreasureMapDetailData._dungeonInfo[floor, num + 3]; num15++)
			{
				num3 = (int)TreasureMapDetailData._dungeonInfo[floor, (num15 << 4) + 792 + (int)TreasureMapDetailData._dungeonInfo[floor, num + 2]];
				if (num3 != 1 && num3 != 3)
				{
					if (this.GenerateRandom(0U, 1U) != 0U)
					{
						num3 = (int)this.GenerateRandom(0U, (uint)value5);
						for (int num16 = 0; num16 < num3; num16++)
						{
							if (TreasureMapDetailData._dungeonInfo[floor, num16 + (num15 << 4) + (int)TreasureMapDetailData._dungeonInfo[floor, num + 2] + 792 + 1] != 8)
							{
								TreasureMapDetailData._dungeonInfo[floor, num16 + (num15 << 4) + (int)TreasureMapDetailData._dungeonInfo[floor, num + 2] + 792 + 1] = 0;
							}
						}
					}
					else if (num3 != 8)
					{
						int num17 = (int)TreasureMapDetailData._dungeonInfo[floor, (int)TreasureMapDetailData._dungeonInfo[floor, num + 1] + (num15 << 4) + 792 + 1];
						if (num17 != 1 && num17 != 8)
						{
							int num18 = (int)this.GenerateRandom(0U, (uint)value);
							num3 = num15 - 1;
							int num19 = 0;
							while (num19 < num18 && TreasureMapDetailData._dungeonInfo[floor, (num15 << 4) + 792 + (int)TreasureMapDetailData._dungeonInfo[floor, num + 2] - num19] != 8 && TreasureMapDetailData._dungeonInfo[floor, (num15 << 4) + 792 + (int)TreasureMapDetailData._dungeonInfo[floor, num + 2] - num19 - 1] != 1 && (TreasureMapDetailData._dungeonInfo[floor, (num15 + 1 << 4) + 792 + (int)TreasureMapDetailData._dungeonInfo[floor, num + 2] - num19] == 1 || TreasureMapDetailData._dungeonInfo[floor, (num15 + 1 << 4) + 792 + (int)TreasureMapDetailData._dungeonInfo[floor, num + 2] - num19 - 1] != 1) && (TreasureMapDetailData._dungeonInfo[floor, (num3 << 4) + 792 + (int)TreasureMapDetailData._dungeonInfo[floor, num + 2] - num19] == 1 || TreasureMapDetailData._dungeonInfo[floor, (num3 << 4) + 792 + (int)TreasureMapDetailData._dungeonInfo[floor, num + 2] - num19 - 1] != 1))
							{
								TreasureMapDetailData._dungeonInfo[floor, (num15 << 4) + 792 + (int)TreasureMapDetailData._dungeonInfo[floor, num + 2] - num19] = 1;
								num19++;
							}
						}
					}
				}
			}
		}

		// floor, 792, 1, 256
  	// address ~ address + value2 を value で埋める処理
		private unsafe void routine1(int floor, uint address, uint value1, uint value2)
		{
			if (value2 == 0U) return;

			uint num = value1;
			if (value2 >= 4U)
			{
				num = (value1 << 8) + value1;
				uint num2 = num;
				num <<= 16;
				num += num2;
				for (num2 = value2 >> 2; num2 > 0U; num2 -= 1U)
				{
					fixed (byte* ptr = &checked(TreasureMapDetailData._dungeonInfo[(int)((IntPtr)(unchecked((long)floor))), (int)((IntPtr)(unchecked((ulong)address)))]))
					{
						*(int*)ptr = (int)num;
					}
					address += 4U;
				}
			}

			if (value2 < 4U || (value2 & 3U) != 0U)
			{
				for (value2 &= 3U; value2 > 0U; value2 -= 1U)
				{
					checked(TreasureMapDetailData._dungeonInfo[(int)((IntPtr)(unchecked((long)floor))), (int)((IntPtr)(unchecked((ulong)address)))]) = (byte)num;
					address += 1U;
				}
			}
		}

		// value1 ~ value4をaddress ~ address+3 に格納するだけ
		private void SetValue(int floor, uint address, byte value1, byte value2, byte value3, byte value4)
		{
			checked
			{
				TreasureMapDetailData._dungeonInfo[(int)((IntPtr)(unchecked((long)floor))), (int)((IntPtr)(unchecked((ulong)address)))] = value1;
				TreasureMapDetailData._dungeonInfo[(int)((IntPtr)(unchecked((long)floor))), (int)((IntPtr)(unchecked((ulong)(address + 1U))))] = value2;
				TreasureMapDetailData._dungeonInfo[(int)((IntPtr)(unchecked((long)floor))), (int)((IntPtr)(unchecked((ulong)(address + 2U))))] = value3;
				TreasureMapDetailData._dungeonInfo[(int)((IntPtr)(unchecked((long)floor))), (int)((IntPtr)(unchecked((ulong)(address + 3U))))] = value4;
			}
		}

		private unsafe bool routineA(int floor, uint address, uint value1, uint value2)
		{
			int num = (int)(checked(TreasureMapDetailData._dungeonInfo[(int)((IntPtr)(unchecked((long)floor))), (int)((IntPtr)(unchecked((ulong)(address + 3U))))]) + 1 - checked(TreasureMapDetailData._dungeonInfo[(int)((IntPtr)(unchecked((long)floor))), (int)((IntPtr)(unchecked((ulong)(address + 1U))))]));
			if (num < 7)
			{
				return false;
			}
			if (checked(TreasureMapDetailData._dungeonInfo[(int)((IntPtr)(unchecked((long)floor))), (int)((IntPtr)(unchecked((ulong)(address + 4U))))]) != 0)
			{
				return false;
			}
			num = (int)this.GenerateRandom(0U, (uint)(num - 7));
			int num2 = (int)checked(TreasureMapDetailData._dungeonInfo[(int)((IntPtr)(unchecked((long)floor))), (int)((IntPtr)(unchecked((ulong)(address + 1U))))]) + num + 3;
			for (int i = (int)checked(TreasureMapDetailData._dungeonInfo[(int)((IntPtr)(unchecked((long)floor))), (int)((IntPtr)(unchecked((ulong)address)))]); i < (int)(checked(TreasureMapDetailData._dungeonInfo[(int)((IntPtr)(unchecked((long)floor))), (int)((IntPtr)(unchecked((ulong)(address + 2U))))]) + 1); i++)
			{
				TreasureMapDetailData._dungeonInfo[floor, (num2 << 4) + i + 792] = 3;
			}
			this.SetValue(floor, value2, checked(TreasureMapDetailData._dungeonInfo[(int)((IntPtr)(unchecked((long)floor))), (int)((IntPtr)(unchecked((ulong)address)))]), (byte)num2, checked(TreasureMapDetailData._dungeonInfo[(int)((IntPtr)(unchecked((long)floor))), (int)((IntPtr)(unchecked((ulong)(address + 2U))))]), (byte)num2);
			checked
			{
				this.SetValue(floor, value1, TreasureMapDetailData._dungeonInfo[(int)((IntPtr)(unchecked((long)floor))), (int)((IntPtr)(unchecked((ulong)address)))], unchecked((byte)(num2 + 1)), TreasureMapDetailData._dungeonInfo[(int)((IntPtr)(unchecked((long)floor))), (int)((IntPtr)(unchecked((ulong)(address + 2U))))], TreasureMapDetailData._dungeonInfo[(int)((IntPtr)(unchecked((long)floor))), (int)((IntPtr)(unchecked((ulong)(address + 3U))))]);
				TreasureMapDetailData._dungeonInfo[(int)((IntPtr)(unchecked((long)floor))), (int)((IntPtr)(unchecked((ulong)(value1 + 4U))))] = 0;
				TreasureMapDetailData._dungeonInfo[(int)((IntPtr)(unchecked((long)floor))), (int)((IntPtr)(unchecked((ulong)(value1 + 5U))))] = 0;
				TreasureMapDetailData._dungeonInfo[(int)((IntPtr)(unchecked((long)floor))), (int)((IntPtr)(unchecked((ulong)(address + 3U))))] = unchecked((byte)(num2 - 1));
				TreasureMapDetailData._dungeonInfo[(int)((IntPtr)(unchecked((long)floor))), (int)((IntPtr)(unchecked((ulong)(address + 4U))))] = 1;
				fixed (byte* ptr = &TreasureMapDetailData._dungeonInfo[(int)((IntPtr)(unchecked((long)floor))), (int)((IntPtr)(unchecked((ulong)value2)))])
				{
					unchecked
					{
						((int*)ptr)[1] = (int)address;
						((int*)ptr)[2] = (int)value1;
					}
				}
				TreasureMapDetailData._dungeonInfo[(int)((IntPtr)(unchecked((long)floor))), (int)((IntPtr)(unchecked((ulong)(value2 + 12U))))] = 1;
				return true;
			}
		}

		private void routineB(int floor, uint address)
		{
			int num = (int)TreasureMapDetailData._dungeonInfo[floor, 21];
			ref byte ptr = ref TreasureMapDetailData._dungeonInfo[floor, 21];
			ptr += 1;
			ref byte ptr2 = ref TreasureMapDetailData._dungeonInfo[floor, 22];
			ptr2 += 1;
			if ((this.GenerateRandom() & 1U) != 0U)
			{
				this.routineF(floor, address);
				this.routineF(floor, (uint)(num * 12 + 24));
				return;
			}
			this.routineF(floor, (uint)(num * 12 + 24));
			this.routineF(floor, address);
		}

		private unsafe bool routineC(int floor, uint address1, uint address2)
		{
			int num = (int)(checked(TreasureMapDetailData._dungeonInfo[(int)((IntPtr)(unchecked((long)floor))), (int)((IntPtr)(unchecked((ulong)(address1 + 2U))))]) + 1 - checked(TreasureMapDetailData._dungeonInfo[(int)((IntPtr)(unchecked((long)floor))), (int)((IntPtr)(unchecked((ulong)address1)))]));
			if (num < 3)
			{
				return false;
			}
			num = (int)(checked(TreasureMapDetailData._dungeonInfo[(int)((IntPtr)(unchecked((long)floor))), (int)((IntPtr)(unchecked((ulong)(address1 + 3U))))]) + 1 - checked(TreasureMapDetailData._dungeonInfo[(int)((IntPtr)(unchecked((long)floor))), (int)((IntPtr)(unchecked((ulong)(address1 + 1U))))]));
			if (num < 3)
			{
				return false;
			}
			int num2;
			int num3;
			int num4;
			int num5;
			checked
			{
				num2 = (int)TreasureMapDetailData._dungeonInfo[(int)((IntPtr)(unchecked((long)floor))), (int)((IntPtr)(unchecked((ulong)address1)))];
				num3 = (int)TreasureMapDetailData._dungeonInfo[(int)((IntPtr)(unchecked((long)floor))), (int)((IntPtr)(unchecked((ulong)(address1 + 1U))))];
				num4 = (int)TreasureMapDetailData._dungeonInfo[(int)((IntPtr)(unchecked((long)floor))), (int)((IntPtr)(unchecked((ulong)(address1 + 2U))))];
				num5 = (int)TreasureMapDetailData._dungeonInfo[(int)((IntPtr)(unchecked((long)floor))), (int)((IntPtr)(unchecked((ulong)(address1 + 3U))))];
			}
			int num6;
			int num7;
			if (this.GenerateRandom(0U, 1U) != 0U)
			{
				num6 = (int)this.GenerateRandom((uint)num2, (uint)(num2 + (num4 - num2 + 1) / 3));
				num7 = (int)this.GenerateRandom((uint)num3, (uint)(num3 + (num5 - num3 + 1) / 3));
			}
			else
			{
				num6 = (int)this.GenerateRandom((uint)(num2 + 1), (uint)(num2 + (num4 - num2 + 1) / 3));
				num7 = (int)this.GenerateRandom((uint)(num3 + 1), (uint)(num3 + (num5 - num3 + 1) / 3));
			}
			int num8;
			int num9;
			if (this.GenerateRandom(0U, 1U) != 0U)
			{
				num8 = (int)this.GenerateRandom((uint)(num2 + (num4 - num2 + 1) / 3 * 2), (uint)num4);
				num9 = (int)this.GenerateRandom((uint)(num3 + (num5 - num3 + 1) / 3 * 2), (uint)num5);
			}
			else
			{
				num8 = (int)this.GenerateRandom((uint)(num2 + (num4 - num2 + 1) / 3 * 2), (uint)(num4 - 1));
				num9 = (int)this.GenerateRandom((uint)(num3 + (num5 - num3 + 1) / 3 * 2), (uint)(num5 - 1));
			}
			this.SetValue(floor, address2, (byte)num6, (byte)num7, (byte)num8, (byte)num9);
			for (int i = 4; i < 20; i++)
			{
				checked(TreasureMapDetailData._dungeonInfo[(int)((IntPtr)(unchecked((long)floor))), (int)((IntPtr)(unchecked((ulong)address2 + (ulong)((long)i))))]) = 0;
			}
			fixed (byte* ptr = &checked(TreasureMapDetailData._dungeonInfo[(int)((IntPtr)(unchecked((long)floor))), (int)((IntPtr)(unchecked((ulong)(address1 + 8U))))]))
			{
				*(int*)ptr = (int)address2;
			}
			for (int j = num7; j <= num9; j++)
			{
				for (int k = num6; k <= num8; k++)
				{
					TreasureMapDetailData._dungeonInfo[floor, k + (j << 4) + 792] = 0;
				}
			}
			this.routineD(floor, address2);
			return true;
		}

		private bool routineD(int floor, uint address)
		{
			int num;
			int num2;
			checked
			{
				num = (int)TreasureMapDetailData._dungeonInfo[(int)((IntPtr)(unchecked((long)floor))), (int)((IntPtr)(unchecked((ulong)address)))];
				if (num == 0)
				{
					return false;
				}
				if (TreasureMapDetailData._dungeonInfo[(int)((IntPtr)(unchecked((long)floor))), (int)((IntPtr)(unchecked((ulong)(address + 1U))))] == 0)
				{
					return false;
				}
				num2 = (int)TreasureMapDetailData._dungeonInfo[(int)((IntPtr)(unchecked((long)floor))), (int)((IntPtr)(unchecked((ulong)(address + 2U))))];
				if (num2 == 0)
				{
					return false;
				}
				if (TreasureMapDetailData._dungeonInfo[(int)((IntPtr)(unchecked((long)floor))), (int)((IntPtr)(unchecked((ulong)(address + 3U))))] == 0)
				{
					return false;
				}
			}
			if (num2 - num + 1 < 5)
			{
				checked(TreasureMapDetailData._dungeonInfo[(int)((IntPtr)(unchecked((long)floor))), (int)((IntPtr)(unchecked((ulong)(address + 12U))))]) = (byte)this.GenerateRandom((uint)num, (uint)num2);
				checked
				{
					TreasureMapDetailData._dungeonInfo[(int)((IntPtr)(unchecked((long)floor))), (int)((IntPtr)(unchecked((ulong)(address + 13U))))] = TreasureMapDetailData._dungeonInfo[(int)((IntPtr)(unchecked((long)floor))), (int)((IntPtr)(unchecked((ulong)(address + 1U))))];
					TreasureMapDetailData._dungeonInfo[(int)((IntPtr)(unchecked((long)floor))), (int)((IntPtr)(unchecked((ulong)(address + 16U))))] = unchecked((byte)checked(this.GenerateRandom((uint)TreasureMapDetailData._dungeonInfo[(int)((IntPtr)(unchecked((long)floor))), (int)((IntPtr)(unchecked((ulong)address)))], (uint)TreasureMapDetailData._dungeonInfo[(int)((IntPtr)(unchecked((long)floor))), (int)((IntPtr)(unchecked((ulong)(address + 2U))))])));
					TreasureMapDetailData._dungeonInfo[(int)((IntPtr)(unchecked((long)floor))), (int)((IntPtr)(unchecked((ulong)(address + 17U))))] = TreasureMapDetailData._dungeonInfo[(int)((IntPtr)(unchecked((long)floor))), (int)((IntPtr)(unchecked((ulong)(address + 3U))))];
				}
				int num3 = ((int)checked(TreasureMapDetailData._dungeonInfo[(int)((IntPtr)(unchecked((long)floor))), (int)((IntPtr)(unchecked((ulong)(address + 13U))))]) << 4) + 792;
				num3 += (int)checked(TreasureMapDetailData._dungeonInfo[(int)((IntPtr)(unchecked((long)floor))), (int)((IntPtr)(unchecked((ulong)(address + 12U))))]);
				TreasureMapDetailData._dungeonInfo[floor, num3] = 8;
				num3 = ((int)checked(TreasureMapDetailData._dungeonInfo[(int)((IntPtr)(unchecked((long)floor))), (int)((IntPtr)(unchecked((ulong)(address + 17U))))]) << 4) + 792;
				num3 += (int)checked(TreasureMapDetailData._dungeonInfo[(int)((IntPtr)(unchecked((long)floor))), (int)((IntPtr)(unchecked((ulong)(address + 16U))))]);
				TreasureMapDetailData._dungeonInfo[floor, num3] = 8;
			}
			else
			{
				int num4 = num + (num2 - num + 1) / 2 - 1;
				checked(TreasureMapDetailData._dungeonInfo[(int)((IntPtr)(unchecked((long)floor))), (int)((IntPtr)(unchecked((ulong)(address + 12U))))]) = (byte)this.GenerateRandom((uint)num, (uint)num4);
				checked
				{
					TreasureMapDetailData._dungeonInfo[(int)((IntPtr)(unchecked((long)floor))), (int)((IntPtr)(unchecked((ulong)(address + 13U))))] = TreasureMapDetailData._dungeonInfo[(int)((IntPtr)(unchecked((long)floor))), (int)((IntPtr)(unchecked((ulong)(address + 1U))))];
					TreasureMapDetailData._dungeonInfo[(int)((IntPtr)(unchecked((long)floor))), (int)((IntPtr)(unchecked((ulong)(address + 14U))))] = unchecked((byte)this.GenerateRandom((uint)(num4 + 1), (uint)checked(TreasureMapDetailData._dungeonInfo[(int)((IntPtr)(unchecked((long)floor))), (int)((IntPtr)(unchecked((ulong)(address + 2U))))])));
					TreasureMapDetailData._dungeonInfo[(int)((IntPtr)(unchecked((long)floor))), (int)((IntPtr)(unchecked((ulong)(address + 15U))))] = TreasureMapDetailData._dungeonInfo[(int)((IntPtr)(unchecked((long)floor))), (int)((IntPtr)(unchecked((ulong)(address + 1U))))];
					TreasureMapDetailData._dungeonInfo[(int)((IntPtr)(unchecked((long)floor))), (int)((IntPtr)(unchecked((ulong)(address + 16U))))] = unchecked((byte)this.GenerateRandom((uint)checked(TreasureMapDetailData._dungeonInfo[(int)((IntPtr)(unchecked((long)floor))), (int)((IntPtr)(unchecked((ulong)address)))]), (uint)num4));
					TreasureMapDetailData._dungeonInfo[(int)((IntPtr)(unchecked((long)floor))), (int)((IntPtr)(unchecked((ulong)(address + 17U))))] = TreasureMapDetailData._dungeonInfo[(int)((IntPtr)(unchecked((long)floor))), (int)((IntPtr)(unchecked((ulong)(address + 3U))))];
					TreasureMapDetailData._dungeonInfo[(int)((IntPtr)(unchecked((long)floor))), (int)((IntPtr)(unchecked((ulong)(address + 18U))))] = unchecked((byte)this.GenerateRandom((uint)(num4 + 1), (uint)checked(TreasureMapDetailData._dungeonInfo[(int)((IntPtr)(unchecked((long)floor))), (int)((IntPtr)(unchecked((ulong)(address + 2U))))])));
					TreasureMapDetailData._dungeonInfo[(int)((IntPtr)(unchecked((long)floor))), (int)((IntPtr)(unchecked((ulong)(address + 19U))))] = TreasureMapDetailData._dungeonInfo[(int)((IntPtr)(unchecked((long)floor))), (int)((IntPtr)(unchecked((ulong)(address + 3U))))];
				}
				int num5 = ((int)checked(TreasureMapDetailData._dungeonInfo[(int)((IntPtr)(unchecked((long)floor))), (int)((IntPtr)(unchecked((ulong)(address + 13U))))]) << 4) + 792;
				num5 += (int)checked(TreasureMapDetailData._dungeonInfo[(int)((IntPtr)(unchecked((long)floor))), (int)((IntPtr)(unchecked((ulong)(address + 12U))))]);
				TreasureMapDetailData._dungeonInfo[floor, num5] = 8;
				num5 = ((int)checked(TreasureMapDetailData._dungeonInfo[(int)((IntPtr)(unchecked((long)floor))), (int)((IntPtr)(unchecked((ulong)(address + 15U))))]) << 4) + 792;
				num5 += (int)checked(TreasureMapDetailData._dungeonInfo[(int)((IntPtr)(unchecked((long)floor))), (int)((IntPtr)(unchecked((ulong)(address + 14U))))]);
				TreasureMapDetailData._dungeonInfo[floor, num5] = 8;
				num5 = ((int)checked(TreasureMapDetailData._dungeonInfo[(int)((IntPtr)(unchecked((long)floor))), (int)((IntPtr)(unchecked((ulong)(address + 17U))))]) << 4) + 792;
				num5 += (int)checked(TreasureMapDetailData._dungeonInfo[(int)((IntPtr)(unchecked((long)floor))), (int)((IntPtr)(unchecked((ulong)(address + 16U))))]);
				TreasureMapDetailData._dungeonInfo[floor, num5] = 8;
				num5 = ((int)checked(TreasureMapDetailData._dungeonInfo[(int)((IntPtr)(unchecked((long)floor))), (int)((IntPtr)(unchecked((ulong)(address + 19U))))]) << 4) + 792;
				num5 += (int)checked(TreasureMapDetailData._dungeonInfo[(int)((IntPtr)(unchecked((long)floor))), (int)((IntPtr)(unchecked((ulong)(address + 18U))))]);
				TreasureMapDetailData._dungeonInfo[floor, num5] = 8;
			}
			if (checked(TreasureMapDetailData._dungeonInfo[(int)((IntPtr)(unchecked((long)floor))), (int)((IntPtr)(unchecked((ulong)(address + 3U))))]) - checked(TreasureMapDetailData._dungeonInfo[(int)((IntPtr)(unchecked((long)floor))), (int)((IntPtr)(unchecked((ulong)(address + 1U))))]) + 1 < 5)
			{
				checked
				{
					TreasureMapDetailData._dungeonInfo[(int)((IntPtr)(unchecked((long)floor))), (int)((IntPtr)(unchecked((ulong)(address + 4U))))] = TreasureMapDetailData._dungeonInfo[(int)((IntPtr)(unchecked((long)floor))), (int)((IntPtr)(unchecked((ulong)address)))];
					TreasureMapDetailData._dungeonInfo[(int)((IntPtr)(unchecked((long)floor))), (int)((IntPtr)(unchecked((ulong)(address + 5U))))] = unchecked((byte)checked(this.GenerateRandom((uint)TreasureMapDetailData._dungeonInfo[(int)((IntPtr)(unchecked((long)floor))), (int)((IntPtr)(unchecked((ulong)(address + 1U))))], (uint)TreasureMapDetailData._dungeonInfo[(int)((IntPtr)(unchecked((long)floor))), (int)((IntPtr)(unchecked((ulong)(address + 3U))))])));
					TreasureMapDetailData._dungeonInfo[(int)((IntPtr)(unchecked((long)floor))), (int)((IntPtr)(unchecked((ulong)(address + 8U))))] = TreasureMapDetailData._dungeonInfo[(int)((IntPtr)(unchecked((long)floor))), (int)((IntPtr)(unchecked((ulong)(address + 2U))))];
					TreasureMapDetailData._dungeonInfo[(int)((IntPtr)(unchecked((long)floor))), (int)((IntPtr)(unchecked((ulong)(address + 9U))))] = unchecked((byte)checked(this.GenerateRandom((uint)TreasureMapDetailData._dungeonInfo[(int)((IntPtr)(unchecked((long)floor))), (int)((IntPtr)(unchecked((ulong)(address + 1U))))], (uint)TreasureMapDetailData._dungeonInfo[(int)((IntPtr)(unchecked((long)floor))), (int)((IntPtr)(unchecked((ulong)(address + 3U))))])));
				}
				int num6 = ((int)checked(TreasureMapDetailData._dungeonInfo[(int)((IntPtr)(unchecked((long)floor))), (int)((IntPtr)(unchecked((ulong)(address + 5U))))]) << 4) + 792;
				num6 += (int)checked(TreasureMapDetailData._dungeonInfo[(int)((IntPtr)(unchecked((long)floor))), (int)((IntPtr)(unchecked((ulong)(address + 4U))))]);
				TreasureMapDetailData._dungeonInfo[floor, num6] = 8;
				num6 = ((int)checked(TreasureMapDetailData._dungeonInfo[(int)((IntPtr)(unchecked((long)floor))), (int)((IntPtr)(unchecked((ulong)(address + 9U))))]) << 4) + 792;
				num6 += (int)checked(TreasureMapDetailData._dungeonInfo[(int)((IntPtr)(unchecked((long)floor))), (int)((IntPtr)(unchecked((ulong)(address + 8U))))]);
				TreasureMapDetailData._dungeonInfo[floor, num6] = 8;
			}
			else
			{
				int num7 = (int)checked(TreasureMapDetailData._dungeonInfo[(int)((IntPtr)(unchecked((long)floor))), (int)((IntPtr)(unchecked((ulong)(address + 1U))))]);
				num7 = num7 + ((int)(checked(TreasureMapDetailData._dungeonInfo[(int)((IntPtr)(unchecked((long)floor))), (int)((IntPtr)(unchecked((ulong)(address + 3U))))]) + 1) - num7) / 2 - 1;
				checked
				{
					TreasureMapDetailData._dungeonInfo[(int)((IntPtr)(unchecked((long)floor))), (int)((IntPtr)(unchecked((ulong)(address + 4U))))] = TreasureMapDetailData._dungeonInfo[(int)((IntPtr)(unchecked((long)floor))), (int)((IntPtr)(unchecked((ulong)address)))];
					TreasureMapDetailData._dungeonInfo[(int)((IntPtr)(unchecked((long)floor))), (int)((IntPtr)(unchecked((ulong)(address + 5U))))] = unchecked((byte)this.GenerateRandom((uint)checked(TreasureMapDetailData._dungeonInfo[(int)((IntPtr)(unchecked((long)floor))), (int)((IntPtr)(unchecked((ulong)(address + 1U))))]), (uint)num7));
					TreasureMapDetailData._dungeonInfo[(int)((IntPtr)(unchecked((long)floor))), (int)((IntPtr)(unchecked((ulong)(address + 6U))))] = TreasureMapDetailData._dungeonInfo[(int)((IntPtr)(unchecked((long)floor))), (int)((IntPtr)(unchecked((ulong)address)))];
					TreasureMapDetailData._dungeonInfo[(int)((IntPtr)(unchecked((long)floor))), (int)((IntPtr)(unchecked((ulong)(address + 7U))))] = unchecked((byte)this.GenerateRandom((uint)(num7 + 1), (uint)checked(TreasureMapDetailData._dungeonInfo[(int)((IntPtr)(unchecked((long)floor))), (int)((IntPtr)(unchecked((ulong)(address + 3U))))])));
					TreasureMapDetailData._dungeonInfo[(int)((IntPtr)(unchecked((long)floor))), (int)((IntPtr)(unchecked((ulong)(address + 8U))))] = TreasureMapDetailData._dungeonInfo[(int)((IntPtr)(unchecked((long)floor))), (int)((IntPtr)(unchecked((ulong)(address + 2U))))];
					TreasureMapDetailData._dungeonInfo[(int)((IntPtr)(unchecked((long)floor))), (int)((IntPtr)(unchecked((ulong)(address + 9U))))] = unchecked((byte)this.GenerateRandom((uint)checked(TreasureMapDetailData._dungeonInfo[(int)((IntPtr)(unchecked((long)floor))), (int)((IntPtr)(unchecked((ulong)(address + 1U))))]), (uint)num7));
					TreasureMapDetailData._dungeonInfo[(int)((IntPtr)(unchecked((long)floor))), (int)((IntPtr)(unchecked((ulong)(address + 10U))))] = TreasureMapDetailData._dungeonInfo[(int)((IntPtr)(unchecked((long)floor))), (int)((IntPtr)(unchecked((ulong)(address + 2U))))];
					TreasureMapDetailData._dungeonInfo[(int)((IntPtr)(unchecked((long)floor))), (int)((IntPtr)(unchecked((ulong)(address + 11U))))] = unchecked((byte)this.GenerateRandom((uint)(num7 + 1), (uint)checked(TreasureMapDetailData._dungeonInfo[(int)((IntPtr)(unchecked((long)floor))), (int)((IntPtr)(unchecked((ulong)(address + 3U))))])));
				}
				int num8 = ((int)checked(TreasureMapDetailData._dungeonInfo[(int)((IntPtr)(unchecked((long)floor))), (int)((IntPtr)(unchecked((ulong)(address + 5U))))]) << 4) + 792;
				num8 += (int)checked(TreasureMapDetailData._dungeonInfo[(int)((IntPtr)(unchecked((long)floor))), (int)((IntPtr)(unchecked((ulong)(address + 4U))))]);
				TreasureMapDetailData._dungeonInfo[floor, num8] = 8;
				num8 = ((int)checked(TreasureMapDetailData._dungeonInfo[(int)((IntPtr)(unchecked((long)floor))), (int)((IntPtr)(unchecked((ulong)(address + 7U))))]) << 4) + 792;
				num8 += (int)checked(TreasureMapDetailData._dungeonInfo[(int)((IntPtr)(unchecked((long)floor))), (int)((IntPtr)(unchecked((ulong)(address + 6U))))]);
				TreasureMapDetailData._dungeonInfo[floor, num8] = 8;
				num8 = ((int)checked(TreasureMapDetailData._dungeonInfo[(int)((IntPtr)(unchecked((long)floor))), (int)((IntPtr)(unchecked((ulong)(address + 9U))))]) << 4) + 792;
				num8 += (int)checked(TreasureMapDetailData._dungeonInfo[(int)((IntPtr)(unchecked((long)floor))), (int)((IntPtr)(unchecked((ulong)(address + 8U))))]);
				TreasureMapDetailData._dungeonInfo[floor, num8] = 8;
				num8 = ((int)checked(TreasureMapDetailData._dungeonInfo[(int)((IntPtr)(unchecked((long)floor))), (int)((IntPtr)(unchecked((ulong)(address + 11U))))]) << 4) + 792;
				num8 += (int)checked(TreasureMapDetailData._dungeonInfo[(int)((IntPtr)(unchecked((long)floor))), (int)((IntPtr)(unchecked((ulong)(address + 10U))))]);
				TreasureMapDetailData._dungeonInfo[floor, num8] = 8;
			}
			return true;
		}

		private unsafe bool routineE(int floor, uint address, uint value1, uint value2)
		{
			int num = (int)(checked(TreasureMapDetailData._dungeonInfo[(int)((IntPtr)(unchecked((long)floor))), (int)((IntPtr)(unchecked((ulong)(address + 2U))))]) + 1 - checked(TreasureMapDetailData._dungeonInfo[(int)((IntPtr)(unchecked((long)floor))), (int)((IntPtr)(unchecked((ulong)address)))]));
			if (num < 7)
			{
				return false;
			}
			if (checked(TreasureMapDetailData._dungeonInfo[(int)((IntPtr)(unchecked((long)floor))), (int)((IntPtr)(unchecked((ulong)(address + 5U))))]) != 0)
			{
				return false;
			}
			num = (int)this.GenerateRandom(0U, (uint)(num - 7));
			int num2 = (int)checked(TreasureMapDetailData._dungeonInfo[(int)((IntPtr)(unchecked((long)floor))), (int)((IntPtr)(unchecked((ulong)address)))]) + num + 3;
			for (int i = (int)checked(TreasureMapDetailData._dungeonInfo[(int)((IntPtr)(unchecked((long)floor))), (int)((IntPtr)(unchecked((ulong)(address + 1U))))]); i < (int)(checked(TreasureMapDetailData._dungeonInfo[(int)((IntPtr)(unchecked((long)floor))), (int)((IntPtr)(unchecked((ulong)(address + 3U))))]) + 1); i++)
			{
				TreasureMapDetailData._dungeonInfo[floor, (i << 4) + num2 + 792] = 3;
			}
			this.SetValue(floor, value2, (byte)num2, checked(TreasureMapDetailData._dungeonInfo[(int)((IntPtr)(unchecked((long)floor))), (int)((IntPtr)(unchecked((ulong)(address + 1U))))]), (byte)num2, checked(TreasureMapDetailData._dungeonInfo[(int)((IntPtr)(unchecked((long)floor))), (int)((IntPtr)(unchecked((ulong)(address + 3U))))]));
			checked
			{
				this.SetValue(floor, value1, unchecked((byte)(num2 + 1)), TreasureMapDetailData._dungeonInfo[(int)((IntPtr)(unchecked((long)floor))), (int)((IntPtr)(unchecked((ulong)(address + 1U))))], TreasureMapDetailData._dungeonInfo[(int)((IntPtr)(unchecked((long)floor))), (int)((IntPtr)(unchecked((ulong)(address + 2U))))], TreasureMapDetailData._dungeonInfo[(int)((IntPtr)(unchecked((long)floor))), (int)((IntPtr)(unchecked((ulong)(address + 3U))))]);
				TreasureMapDetailData._dungeonInfo[(int)((IntPtr)(unchecked((long)floor))), (int)((IntPtr)(unchecked((ulong)(value1 + 4U))))] = 0;
				TreasureMapDetailData._dungeonInfo[(int)((IntPtr)(unchecked((long)floor))), (int)((IntPtr)(unchecked((ulong)(value1 + 5U))))] = 0;
				TreasureMapDetailData._dungeonInfo[(int)((IntPtr)(unchecked((long)floor))), (int)((IntPtr)(unchecked((ulong)(address + 2U))))] = unchecked((byte)(num2 - 1));
				TreasureMapDetailData._dungeonInfo[(int)((IntPtr)(unchecked((long)floor))), (int)((IntPtr)(unchecked((ulong)(address + 5U))))] = 1;
				fixed (byte* ptr = &TreasureMapDetailData._dungeonInfo[(int)((IntPtr)(unchecked((long)floor))), (int)((IntPtr)(unchecked((ulong)value2)))])
				{
					unchecked
					{
						((int*)ptr)[1] = (int)address;
						((int*)ptr)[2] = (int)value1;
					}
				}
				TreasureMapDetailData._dungeonInfo[(int)((IntPtr)(unchecked((long)floor))), (int)((IntPtr)(unchecked((ulong)(value2 + 12U))))] = 2;
				return true;
			}
		}

		private void routineF(int floor, uint address)
		{
			if (TreasureMapDetailData._dungeonInfo[floor, 21] >= 15)
			{
				return;
			}
			if (checked(TreasureMapDetailData._dungeonInfo[(int)((IntPtr)(unchecked((long)floor))), (int)((IntPtr)(unchecked((ulong)(address + 5U))))]) != 0)
			{
				if (!this.routineA(floor, address, (uint)(TreasureMapDetailData._dungeonInfo[floor, 21] * 12 + 24), (uint)(((int)TreasureMapDetailData._dungeonInfo[floor, 22] << 4) + 216)))
				{
					return;
				}
				this.routineB(floor, address);
				return;
			}
			else if (checked(TreasureMapDetailData._dungeonInfo[(int)((IntPtr)(unchecked((long)floor))), (int)((IntPtr)(unchecked((ulong)(address + 4U))))]) != 0)
			{
				if (!this.routineE(floor, address, (uint)(TreasureMapDetailData._dungeonInfo[floor, 21] * 12 + 24), (uint)(((int)TreasureMapDetailData._dungeonInfo[floor, 22] << 4) + 216)))
				{
					return;
				}
				this.routineB(floor, address);
				return;
			}
			else if ((this.GenerateRandom() & 1U) != 0U)
			{
				if (!this.routineE(floor, address, (uint)(TreasureMapDetailData._dungeonInfo[floor, 21] * 12 + 24), (uint)(((int)TreasureMapDetailData._dungeonInfo[floor, 22] << 4) + 216)))
				{
					return;
				}
				this.routineB(floor, address);
				return;
			}
			else
			{
				if (!this.routineA(floor, address, (uint)(TreasureMapDetailData._dungeonInfo[floor, 21] * 12 + 24), (uint)(((int)TreasureMapDetailData._dungeonInfo[floor, 22] << 4) + 216)))
				{
					return;
				}
				this.routineB(floor, address);
				return;
			}
		}

		private unsafe int routineG(int floor, uint address)
		{
			if (TreasureMapDetailData._dungeonInfo[floor, (int)(address + 12U)] == 1)
			{
				int address2;
				int address3;
				fixed (byte* ptr = &TreasureMapDetailData._dungeonInfo[floor, 0])
				{
					address2 = (int)((uint*)ptr + ((uint*)ptr + address / 4U)[1] / 4U)[2];
					address3 = (int)((uint*)ptr + ((uint*)ptr + address / 4U)[2] / 4U)[2];
				}
				int value;
				if (this.GenerateRandom(0U, 7U) != 0U)
				{
					value = 0;
				}
				else
				{
					value = 1;
				}
				this.routineH(floor, (uint)address2, 16U, (uint)address3, 12U, address, (uint)value);
				if (this.GenerateRandom(0U, 7U) != 0U)
				{
					value = 0;
				}
				else
				{
					value = 1;
				}
				this.routineH(floor, (uint)address2, 18U, (uint)address3, 12U, address, (uint)value);
				if (this.GenerateRandom(0U, 7U) != 0U)
				{
					value = 0;
				}
				else
				{
					value = 1;
				}
				this.routineH(floor, (uint)address2, 16U, (uint)address3, 14U, address, (uint)value);
				if (this.GenerateRandom(0U, 7U) != 0U)
				{
					value = 0;
				}
				else
				{
					value = 1;
				}
				this.routineH(floor, (uint)address2, 18U, (uint)address3, 14U, address, (uint)value);
			}
			else if (TreasureMapDetailData._dungeonInfo[floor, (int)(address + 12U)] == 2)
			{
				int address2;
				int address3;
				fixed (byte* ptr2 = &TreasureMapDetailData._dungeonInfo[floor, 0])
				{
					address2 = (int)((uint*)ptr2 + ((uint*)ptr2 + address / 4U)[1] / 4U)[2];
					address3 = (int)((uint*)ptr2 + ((uint*)ptr2 + address / 4U)[2] / 4U)[2];
				}
				int value;
				if (this.GenerateRandom(0U, 7U) != 0U)
				{
					value = 0;
				}
				else
				{
					value = 1;
				}
				this.routineH(floor, (uint)address2, 8U, (uint)address3, 4U, address, (uint)value);
				if (this.GenerateRandom(0U, 7U) != 0U)
				{
					value = 0;
				}
				else
				{
					value = 1;
				}
				this.routineH(floor, (uint)address2, 10U, (uint)address3, 4U, address, (uint)value);
				if (this.GenerateRandom(0U, 7U) != 0U)
				{
					value = 0;
				}
				else
				{
					value = 1;
				}
				this.routineH(floor, (uint)address2, 8U, (uint)address3, 6U, address, (uint)value);
				if (this.GenerateRandom(0U, 7U) != 0U)
				{
					value = 0;
				}
				else
				{
					value = 1;
				}
				this.routineH(floor, (uint)address2, 10U, (uint)address3, 6U, address, (uint)value);
			}
			return 1;
		}

		private bool routineH(int floor, uint address1, uint value1, uint address2, uint value2, uint address3, uint value3)
		{
			int num;
			int num2;
			int num3;
			int num4;
			checked
			{
				num = (int)TreasureMapDetailData._dungeonInfo[(int)((IntPtr)(unchecked((long)floor))), (int)((IntPtr)(unchecked((ulong)(address1 + value1))))];
				num2 = (int)TreasureMapDetailData._dungeonInfo[(int)((IntPtr)(unchecked((long)floor))), (int)((IntPtr)(unchecked((ulong)(address1 + value1 + 1U))))];
				num3 = (int)TreasureMapDetailData._dungeonInfo[(int)((IntPtr)(unchecked((long)floor))), (int)((IntPtr)(unchecked((ulong)(address2 + value2))))];
				num4 = (int)TreasureMapDetailData._dungeonInfo[(int)((IntPtr)(unchecked((long)floor))), (int)((IntPtr)(unchecked((ulong)(address2 + value2 + 1U))))];
				if (num == 0 || num2 == 0 || num3 == 0 || num4 == 0)
				{
					return false;
				}
			}
			if (checked(TreasureMapDetailData._dungeonInfo[(int)((IntPtr)(unchecked((long)floor))), (int)((IntPtr)(unchecked((ulong)(address3 + 12U))))]) == 1)
			{
				for (int i = num2 + 1; i < (int)(TreasureMapDetailData._dungeonInfo[floor, (int)(address3 + 1U)] + 1); i++)
				{
					TreasureMapDetailData._dungeonInfo[floor, (i << 4) + 792 + num] = 2;
				}
				for (int j = num4 - 1; j > (int)TreasureMapDetailData._dungeonInfo[floor, (int)(address3 + 1U)]; j--)
				{
					TreasureMapDetailData._dungeonInfo[floor, (j << 4) + 792 + num3] = 2;
				}
				if (num < num3)
				{
					for (int k = num; k < num3 + 1; k++)
					{
						TreasureMapDetailData._dungeonInfo[floor, k + ((int)TreasureMapDetailData._dungeonInfo[floor, (int)(address3 + 1U)] << 4) + 792] = 2;
					}
				}
				else if (num > num3)
				{
					for (int l = num3; l < num + 1; l++)
					{
						TreasureMapDetailData._dungeonInfo[floor, l + ((int)TreasureMapDetailData._dungeonInfo[floor, (int)(address3 + 1U)] << 4) + 792] = 2;
					}
				}
				if (value3 == 0U)
				{
					return true;
				}
				if (num < num3)
				{
					for (int m = num3 + 1; m < 16; m++)
					{
						int num5 = (int)TreasureMapDetailData._dungeonInfo[floor, m + ((int)TreasureMapDetailData._dungeonInfo[floor, (int)(address3 + 1U)] << 4) + 792];
						if (num5 != 1 && num5 != 3)
						{
							for (int n = num3 + 1; n < m; n++)
							{
								TreasureMapDetailData._dungeonInfo[floor, n + ((int)TreasureMapDetailData._dungeonInfo[floor, (int)(address3 + 1U)] << 4) + 792] = 2;
							}
							break;
						}
					}
					for (int num6 = num - 1; num6 >= 0; num6--)
					{
						int num7 = (int)TreasureMapDetailData._dungeonInfo[floor, num6 + ((int)TreasureMapDetailData._dungeonInfo[floor, (int)(address3 + 1U)] << 4) + 792];
						if (num7 != 1 && num7 != 3)
						{
							for (int num8 = num - 1; num8 > num6; num8--)
							{
								TreasureMapDetailData._dungeonInfo[floor, num8 + ((int)TreasureMapDetailData._dungeonInfo[floor, (int)(address3 + 1U)] << 4) + 792] = 2;
							}
							break;
						}
					}
				}
				else if (num >= num3)
				{
					for (int num9 = num + 1; num9 < 16; num9++)
					{
						int num10 = (int)TreasureMapDetailData._dungeonInfo[floor, num9 + ((int)TreasureMapDetailData._dungeonInfo[floor, (int)(address3 + 1U)] << 4) + 792];
						if (num10 != 1 && num10 != 3)
						{
							for (int num11 = num + 1; num11 < num9; num11++)
							{
								TreasureMapDetailData._dungeonInfo[floor, num11 + ((int)TreasureMapDetailData._dungeonInfo[floor, (int)(address3 + 1U)] << 4) + 792] = 2;
							}
							break;
						}
					}
					for (int num12 = num3 - 1; num12 >= 0; num12--)
					{
						int num13 = (int)TreasureMapDetailData._dungeonInfo[floor, num12 + ((int)TreasureMapDetailData._dungeonInfo[floor, (int)(address3 + 1U)] << 4) + 792];
						if (num13 != 1 && num13 != 3)
						{
							for (int num14 = num3 - 1; num14 > num12; num14--)
							{
								TreasureMapDetailData._dungeonInfo[floor, num14 + ((int)TreasureMapDetailData._dungeonInfo[floor, (int)(address3 + 1U)] << 4) + 792] = 2;
							}
							break;
						}
					}
				}
			}
			else if (checked(TreasureMapDetailData._dungeonInfo[(int)((IntPtr)(unchecked((long)floor))), (int)((IntPtr)(unchecked((ulong)(address3 + 12U))))]) == 2)
			{
				for (int num15 = num + 1; num15 < (int)(TreasureMapDetailData._dungeonInfo[floor, (int)address3] + 1); num15++)
				{
					TreasureMapDetailData._dungeonInfo[floor, num15 + (num2 << 4) + 792] = 2;
				}
				for (int num16 = num3 - 1; num16 > (int)TreasureMapDetailData._dungeonInfo[floor, (int)address3]; num16--)
				{
					TreasureMapDetailData._dungeonInfo[floor, num16 + (num4 << 4) + 792] = 2;
				}
				if (num2 < num4)
				{
					for (int num17 = num2; num17 < num4 + 1; num17++)
					{
						TreasureMapDetailData._dungeonInfo[floor, (num17 << 4) + 792 + (int)TreasureMapDetailData._dungeonInfo[floor, (int)address3]] = 2;
					}
				}
				else if (num2 > num4)
				{
					for (int num18 = num4; num18 < num2 + 1; num18++)
					{
						TreasureMapDetailData._dungeonInfo[floor, (num18 << 4) + 792 + (int)TreasureMapDetailData._dungeonInfo[floor, (int)address3]] = 2;
					}
				}
				if (value3 == 0U)
				{
					return true;
				}
				if (num2 < num4)
				{
					for (int num19 = num4 + 1; num19 < 16; num19++)
					{
						int num20 = (int)TreasureMapDetailData._dungeonInfo[floor, (num19 << 4) + 792 + (int)TreasureMapDetailData._dungeonInfo[floor, (int)address3]];
						if (num20 != 1 && num20 != 3)
						{
							for (int num21 = num4 + 1; num21 < num19; num21++)
							{
								TreasureMapDetailData._dungeonInfo[floor, (num21 << 4) + 792 + (int)TreasureMapDetailData._dungeonInfo[floor, (int)address3]] = 2;
							}
							break;
						}
					}
					for (int num22 = num2 - 1; num22 >= 0; num22--)
					{
						int num23 = (int)TreasureMapDetailData._dungeonInfo[floor, (num22 << 4) + 792 + (int)TreasureMapDetailData._dungeonInfo[floor, (int)address3]];
						if (num23 != 1 && num23 != 3)
						{
							for (int num24 = num2 - 1; num24 > num22; num24--)
							{
								TreasureMapDetailData._dungeonInfo[floor, (num24 << 4) + 792 + (int)TreasureMapDetailData._dungeonInfo[floor, (int)address3]] = 2;
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
						int num26 = (int)TreasureMapDetailData._dungeonInfo[floor, (num25 << 4) + 792 + (int)TreasureMapDetailData._dungeonInfo[floor, (int)address3]];
						if (num26 != 1 && num26 != 3)
						{
							for (int num27 = num2 + 1; num27 < num25; num27++)
							{
								TreasureMapDetailData._dungeonInfo[floor, (num27 << 4) + 792 + (int)TreasureMapDetailData._dungeonInfo[floor, (int)address3]] = 2;
							}
							break;
						}
						num25++;
					}
					for (num25 = num4 - 1; num25 >= 0; num25--)
					{
						int num28 = (int)TreasureMapDetailData._dungeonInfo[floor, (num25 << 4) + 792 + (int)TreasureMapDetailData._dungeonInfo[floor, (int)address3]];
						if (num28 != 1 && num28 != 3)
						{
							for (int num29 = num4 - 1; num29 > num25; num29--)
							{
								TreasureMapDetailData._dungeonInfo[floor, (num29 << 4) + 792 + (int)TreasureMapDetailData._dungeonInfo[floor, (int)address3]] = 2;
							}
							break;
						}
					}
				}
			}
			return true;
		}

		private uint routineI(int floor, uint value1, uint value2, uint value3)
		{
			int num;
			int num2;
			int num3;
			int num4;
			int num5;
			int num6;
			int num7;
			int num8;
			int num9;
			checked
			{
				num = (int)TreasureMapDetailData._dungeonInfo[(int)((IntPtr)(unchecked((long)floor))), (int)((IntPtr)(unchecked((ulong)(value1 + (value2 << 4) + 792U))))];
				if (num == 1 || num == 3)
				{
					return 255U;
				}
				num2 = (int)TreasureMapDetailData._dungeonInfo[(int)((IntPtr)(unchecked((long)floor))), (int)((IntPtr)(unchecked((ulong)(value1 + (value2 - 1U << 4) + 792U))))];
				num3 = (int)TreasureMapDetailData._dungeonInfo[(int)((IntPtr)(unchecked((long)floor))), (int)((IntPtr)(unchecked((ulong)(value1 + (value2 - 1U << 4) + 792U - 1U))))];
				num4 = (int)TreasureMapDetailData._dungeonInfo[(int)((IntPtr)(unchecked((long)floor))), (int)((IntPtr)(unchecked((ulong)(value1 + (value2 - 1U << 4) + 792U + 1U))))];
				num5 = (int)TreasureMapDetailData._dungeonInfo[(int)((IntPtr)(unchecked((long)floor))), (int)((IntPtr)(unchecked((ulong)(value1 + (value2 << 4) + 792U + 1U))))];
				num6 = (int)TreasureMapDetailData._dungeonInfo[(int)((IntPtr)(unchecked((long)floor))), (int)((IntPtr)(unchecked((ulong)(value1 + (value2 + 1U << 4) + 792U + 1U))))];
				num7 = (int)TreasureMapDetailData._dungeonInfo[(int)((IntPtr)(unchecked((long)floor))), (int)((IntPtr)(unchecked((ulong)(value1 + (value2 + 1U << 4) + 792U))))];
				num8 = (int)TreasureMapDetailData._dungeonInfo[(int)((IntPtr)(unchecked((long)floor))), (int)((IntPtr)(unchecked((ulong)(value1 + (value2 + 1U << 4) + 792U - 1U))))];
				num9 = (int)TreasureMapDetailData._dungeonInfo[(int)((IntPtr)(unchecked((long)floor))), (int)((IntPtr)(unchecked((ulong)(value1 + (value2 << 4) + 792U - 1U))))];
			}
			if (num != 0 && num != 2)
			{
				num = (num + 252 & 255);
				if (num > 4)
				{
					return value3;
				}
			}
			if (num3 == 1 || num3 == 3)
			{
				value3 |= 128U;
			}
			else
			{
				value3 &= 127U;
			}
			if (num4 == 1 || num4 == 3)
			{
				value3 |= 32U;
			}
			else
			{
				value3 &= 223U;
			}
			if (num6 == 1 || num6 == 3)
			{
				value3 |= 8U;
			}
			else
			{
				value3 &= 247U;
			}
			if (num8 == 1 || num8 == 3)
			{
				value3 |= 2U;
			}
			else
			{
				value3 &= 253U;
			}
			if (num2 == 1 || num2 == 3)
			{
				value3 |= 224U;
			}
			else
			{
				value3 &= 191U;
			}
			if (num5 == 1 || num5 == 3)
			{
				value3 |= 56U;
			}
			else
			{
				value3 &= 239U;
			}
			if (num7 == 1 || num7 == 3)
			{
				value3 |= 14U;
			}
			else
			{
				value3 &= 251U;
			}
			if (num9 == 1 || num9 == 3)
			{
				value3 |= 131U;
			}
			else
			{
				value3 &= 254U;
			}
			return value3;
		}

		private void routineJ(int floor)
		{
			int num = 0;
			int num2 = 0;
			int num5;
			int num6;
			for (;;)
			{
				int num3 = (int)this.GenerateRandom(0U, (uint)(TreasureMapDetailData._dungeonInfo[floor, 23] - 1));
				int num4 = num3 * 20 + 472;
				num5 = (int)this.GenerateRandom((uint)TreasureMapDetailData._dungeonInfo[floor, num4], (uint)TreasureMapDetailData._dungeonInfo[floor, num4 + 2]);
				num6 = (int)this.GenerateRandom((uint)TreasureMapDetailData._dungeonInfo[floor, num4 + 1], (uint)TreasureMapDetailData._dungeonInfo[floor, num4 + 3]);
				int num7;
				int num8;
				int num9;
				int num10;
				if (num2 < 100)
				{
					num7 = (int)TreasureMapDetailData._dungeonInfo[floor, (num6 - 1 << 4) + num5 + 792];
					num8 = (int)TreasureMapDetailData._dungeonInfo[floor, (num6 + 1 << 4) + num5 + 792];
					num9 = (int)TreasureMapDetailData._dungeonInfo[floor, (num6 << 4) + 792 + num5 - 1];
					num10 = (int)TreasureMapDetailData._dungeonInfo[floor, (num6 << 4) + 792 + num5 + 1];
					if (num7 == 1 || num7 == 3)
					{
						num7 = 1;
					}
					else
					{
						num7 = 0;
					}
					if (num8 == 1 || num8 == 3)
					{
						num8 = 1;
					}
					else
					{
						num8 = 0;
					}
					if (num9 == 1 || num9 == 3)
					{
						num9 = 1;
					}
					else
					{
						num9 = 0;
					}
					if (num10 == 1 || num10 == 3)
					{
						num10 = 1;
					}
					else
					{
						num10 = 0;
					}
					if (num7 != 0 && num8 != 0 && num9 != 0 && num10 != 0)
					{
						num2++;
						continue;
					}
					if (num7 != 0 && num10 != 0)
					{
						num2++;
						continue;
					}
					if (num7 != 0 && num9 != 0)
					{
						num2++;
						continue;
					}
					if (num8 != 0 && num10 != 0)
					{
						num2++;
						continue;
					}
					if (num8 != 0 && num9 != 0)
					{
						num2++;
						continue;
					}
					int num11 = (int)this.routineI(floor, (uint)num5, (uint)num6, 0U);
					int num12 = num11 & 255;
					if (num12 == 234 || num12 == 171 || num12 == 174 || num12 == 186 || num12 == 163 || num12 == 142 || num12 == 58 || num12 == 232 || num12 == 184 || num12 == 226 || num12 == 139 || num12 == 46)
					{
						num2++;
						continue;
					}
				}
				TreasureMapDetailData._dungeonInfo[floor, num5 + (num6 << 4) + 792] = 4;
				TreasureMapDetailData._dungeonInfo[floor, 4] = (byte)num5;
				TreasureMapDetailData._dungeonInfo[floor, 5] = (byte)num6;
				num8 = num5;
				num9 = num6;
				for (;;)
				{
					num6 = (int)this.GenerateRandom(0U, (uint)(TreasureMapDetailData._dungeonInfo[floor, 23] - 1));
					if (num6 == (num3 & 255) && (num & 255) < 25)
					{
						num++;
					}
					else
					{
						num4 = num6 * 20 + 472;
						num5 = (int)this.GenerateRandom((uint)TreasureMapDetailData._dungeonInfo[floor, num4], (uint)TreasureMapDetailData._dungeonInfo[floor, num4 + 2]);
						num7 = num6;
						num6 = (int)this.GenerateRandom((uint)TreasureMapDetailData._dungeonInfo[floor, num4 + 1], (uint)TreasureMapDetailData._dungeonInfo[floor, num4 + 3]);
						if ((num3 & 255) != num7 || num5 != num8 || num6 != num9)
						{
							break;
						}
					}
				}
				num7 = (int)TreasureMapDetailData._dungeonInfo[floor, num5 + (num6 - 1 << 4) + 792];
				num8 = (int)TreasureMapDetailData._dungeonInfo[floor, num5 + (num6 + 1 << 4) + 792];
				num9 = (int)TreasureMapDetailData._dungeonInfo[floor, num5 + (num6 << 4) + 792 - 1];
				num10 = (int)TreasureMapDetailData._dungeonInfo[floor, num5 + (num6 << 4) + 792 + 1];
				if (num7 == 1 || num7 == 3)
				{
					num7 = 1;
				}
				else
				{
					num7 = 0;
				}
				if (num8 == 1 || num8 == 3)
				{
					num8 = 1;
				}
				else
				{
					num8 = 0;
				}
				if (num9 == 1 || num9 == 3)
				{
					num9 = 1;
				}
				else
				{
					num9 = 0;
				}
				if (num10 == 1 || num10 == 3)
				{
					num10 = 1;
				}
				else
				{
					num10 = 0;
				}
				if (num7 != 0 && num8 != 0 && num9 != 0 && num10 != 0)
				{
					num2++;
				}
				else if (num7 != 0 && num10 != 0)
				{
					num2++;
				}
				else if (num7 != 0 && num9 != 0)
				{
					num2++;
				}
				else if (num8 != 0 && num10 != 0)
				{
					num2++;
				}
				else
				{
					if (num8 == 0 || num9 == 0)
					{
						break;
					}
					num2++;
				}
			}
			TreasureMapDetailData._dungeonInfo[floor, num5 + (num6 << 4) + 792] = 5;
			TreasureMapDetailData._dungeonInfo[floor, 6] = (byte)num5;
			TreasureMapDetailData._dungeonInfo[floor, 7] = (byte)num6;
		}

		private int routineK(int floor)
		{
			int num = (int)this.GenerateRandom(1U, 3U);
			TreasureMapDetailData._dungeonInfo[floor, 8] = (byte)num;
			int num2 = 0;
			int num3 = 0;
			for (;;)
			{
				int num4 = (int)(this.GenerateRandom(0U, (uint)(TreasureMapDetailData._dungeonInfo[floor, 23] - 1)) * 20U + 472U);
				int num5 = (int)this.GenerateRandom((uint)TreasureMapDetailData._dungeonInfo[floor, num4], (uint)TreasureMapDetailData._dungeonInfo[floor, num4 + 2]);
				int num6 = (int)this.GenerateRandom((uint)TreasureMapDetailData._dungeonInfo[floor, num4 + 1], (uint)TreasureMapDetailData._dungeonInfo[floor, num4 + 3]);
				int num7 = (int)TreasureMapDetailData._dungeonInfo[floor, num5 + ((num6 & 255) << 4) + 792];
				if (num2 < 100 && (TreasureMapDetailData._dungeonInfo[floor, 0] == 3 || TreasureMapDetailData._dungeonInfo[floor, 0] == 1))
				{
					num2++;
				}
				else if (num7 == 6 || num7 == 4 || num7 == 5)
				{
					num2++;
				}
				else
				{
					TreasureMapDetailData._dungeonInfo[floor, num5 + ((num6 & 255) << 4) + 792] = 6;
					TreasureMapDetailData._dungeonInfo[floor, num3 * 2 + 13] = (byte)num5;
					TreasureMapDetailData._dungeonInfo[floor, num3 * 2 + 14] = (byte)num6;
					num3++;
					if (num3 >= (num & 255))
					{
						break;
					}
				}
			}
			return 1;
		}

		private unsafe void CreateDungeonDetail(bool createSearchData)
		{
			int num = 0;
			for (int i = 1; i < (int)(this._details[1] + 1); i++)
			{
				int fIdx = i - 1;
				TreasureMapDetailData._dungeonInfo[fIdx, 0] = (byte)i;
				TreasureMapDetailData._dungeonInfo[fIdx, 8] = 0;

				this._seed = (uint)((int)this.MapSeed + i);
				// 宝箱があるのは3階以降
				if (!createSearchData || i > 2)
				{
					// dungeonInfo[fIdx, 792] ~ [fIdx, 792 + 255] を 0x01 で埋める
					this.routine1(fIdx, 792U, 1U, 256U);

					TreasureMapDetailData._dungeonInfo[fIdx, 1] = 0;
					TreasureMapDetailData._dungeonInfo[fIdx, 21] = 1;
					TreasureMapDetailData._dungeonInfo[fIdx, 22] = 0;
					TreasureMapDetailData._dungeonInfo[fIdx, 23] = 0;
					TreasureMapDetailData._dungeonInfo[fIdx, 24] = 1;
					TreasureMapDetailData._dungeonInfo[fIdx, 25] = 1;
					TreasureMapDetailData._dungeonInfo[fIdx, 26] = TreasureMapDetailData._dungeonInfo[fIdx, 2] - 2;
					TreasureMapDetailData._dungeonInfo[fIdx, 27] = TreasureMapDetailData._dungeonInfo[fIdx, 3] - 2;
					TreasureMapDetailData._dungeonInfo[fIdx, 28] = 0;
					TreasureMapDetailData._dungeonInfo[fIdx, 29] = 0;

					// この中でごにょごにょする
					this.routineF(fIdx, 24U);

					for (int j = 0; j < (int)_dungeonInfo[fIdx, 21]; j++)
					{
						if (this.routineC(fIdx, (uint)(j * 12 + 24), (uint)(_dungeonInfo[fIdx, 23] * 20) + 472U))
						{
							TreasureMapDetailData._dungeonInfo[fIdx, 23] += 1;
						}
					}

					for (int k = 0; k < (int)TreasureMapDetailData._dungeonInfo[fIdx, 21]; k++)
					{
						this.GenerateFloorMap(fIdx, (uint)(k * 12 + 24));
					}

					for (int l = 0; l < (int)TreasureMapDetailData._dungeonInfo[fIdx, 22]; l++)
					{
						this.routineG(fIdx, (uint)((l << 4) + 216));
					}

					for (int m = 0; m < (int)TreasureMapDetailData._dungeonInfo[fIdx, 2]; m++)
					{
						TreasureMapDetailData._dungeonInfo[fIdx, m + 792] = 1;
						TreasureMapDetailData._dungeonInfo[fIdx, ((int)(TreasureMapDetailData._dungeonInfo[fIdx, 3] - 1) << 4) + m + 792] = 1;
					}

					for (int n = 0; n < (int)TreasureMapDetailData._dungeonInfo[fIdx, 3]; n++)
					{
						TreasureMapDetailData._dungeonInfo[fIdx, (n << 4) + 792] = 1;
						TreasureMapDetailData._dungeonInfo[fIdx, (n << 4) + 792 + (int)TreasureMapDetailData._dungeonInfo[fIdx, 2] - 1] = 1;
					}

					this.routineJ(fIdx);

					if (TreasureMapDetailData._dungeonInfo[fIdx, 0] <= 2)
					{
						TreasureMapDetailData._dungeonInfo[fIdx, 8] = 0;
					}
					else
					{
						this.routineK(fIdx);
					}



					if (!createSearchData)
					{
						TreasureMapDetailData._dungeonInfo[fIdx, 1306] = 0;
						TreasureMapDetailData._dungeonInfo[fIdx, 1307] = 0;
						for (int num3 = 1; num3 < 15; num3++)
						{
							for (int num4 = 1; num4 < 15; num4++)
							{
								int num5 = (int)TreasureMapDetailData._dungeonInfo[fIdx, num4 + (num3 << 4) + 792];
								if (num5 != 1 && num5 != 3)
								{
									ref byte ptr2 = ref TreasureMapDetailData._dungeonInfo[fIdx, 1306];
									ptr2 += 1;
									num = (int)TreasureMapDetailData._dungeonInfo[fIdx, num4 + (num3 - 1 << 4) + 792];
									if (num != 1 && num != 3)
									{
										ref byte ptr3 = ref TreasureMapDetailData._dungeonInfo[fIdx, 1307];
										ptr3 += 1;
									}
									num = (int)TreasureMapDetailData._dungeonInfo[fIdx, (num3 << 4) + 792 + num4 - 1];
									if (num != 1 && num != 3)
									{
										ref byte ptr4 = ref TreasureMapDetailData._dungeonInfo[fIdx, 1307];
										ptr4 += 1;
									}
								}
							}
						}
						TreasureMapDetailData._dungeonInfo[fIdx, 1304] = (byte)((int)this._details[2] + (i - 1) / 4);
						int num6 = (int)((TreasureMapDataTable.TableJ[(int)(this._details[3] - 1)] * 12 + TreasureMapDetailData._dungeonInfo[fIdx, 1304] - 1) * 18);
						TreasureMapDetailData._dungeonInfo[fIdx, 1305] = TreasureMapDataTable.TableK[num6];
						TreasureMapDetailData._dungeonInfo[fIdx, 1312] = 0;
						fixed (byte* ptr5 = &TreasureMapDetailData._dungeonInfo[fIdx, 0])
						{
							((int*)ptr5)[327] = ((int)ptr5[1306] << 4) + 4896;
							int num7 = 4128 - (((int)ptr5[1306] << 4) + (int)(ptr5[1307] * 8));
							if (num7 < 0)
							{
								((uint*)ptr5)[1308] += (uint)(((int)ptr5[1307] + (num7 + 7) / 8 - 1) * 8);
							}
							else
							{
								((uint*)ptr5)[1308] += (uint)(ptr5[1307] * 8);
							}
							((uint*)ptr5)[1308] += 4U;
							((uint*)ptr5)[1308] += (uint)(ptr5[1307] * 8);
							((uint*)ptr5)[1308] += (uint)(ptr5[1305] * 20);
							for (int num8 = 0; num8 < (int)ptr5[1305]; num8++)
							{
								fixed (byte* ptr6 = &TreasureMapDataTable.TableK[0])
								{
									int num9 = (int)((ushort*)ptr6)[(num6 + num8 * 2 + 2) / 2];
									fixed (byte* ptr7 = &TreasureMapDataTable.TableM[num9 * 2])
									{
										((uint*)ptr5)[1308] += (uint)(TreasureMapDataTable.TableL[(int)(*(ushort*)ptr7)] + 8);
									}
								}
							}
							num = 11216;
							if ((long)num >= (long)((ulong)((uint*)ptr5)[327]))
							{
								num = (int)((long)num - (long)((ulong)((uint*)ptr5)[327])) / 20;
							}
							else
							{
								num = 0;
							}
							ptr5[1313] = 0;
							int num10 = 0;
							for (int num11 = 0; num11 < (int)ptr5[1305]; num11++)
							{
								try
								{
									fixed (byte* ptr8 = &TreasureMapDataTable.TableK[0])
									{
										int num12 = (int)((ushort*)ptr8)[(num6 + num11 * 2 + 2) / 2];
										if (num12 < 38 || num12 > 40)
										{
											if (num11 >= num)
											{
												num10++;
											}
											else
											{
												((short*)ptr5)[((int)(ptr5[1313] * 2) + 1314) / 2] = (short)((ushort)num12);
												byte* ptr9 = ptr5 + 1313;
												*ptr9 += 1;
											}
										}
									}
								}
								finally
								{
									byte* ptr8 = null;
								}
							}
							if (ptr5[1313] == 0)
							{
								ptr5[1312] = (ptr5[1312] | 1);
							}
							else if (ptr5[1313] == 1)
							{
								ptr5[1312] = (ptr5[1312] | 2);
							}
							else if (num10 > 0)
							{
								ptr5[1312] = (ptr5[1312] | 4);
							}
							if (ptr5[1313] == 1)
							{
								this._details2[14] = ptr5[1314];
								this._details2[15] = ptr5[1315];
							}
							else
							{
								this._details2[12] = (this._details2[12] | ptr5[1312]);
							}
						}
					}
				}
			}
			
			for (int f = 2; f < (int)this._details[1]; f++)
			{
				this._seed = (uint)((int)this.MapSeed + f + 1);

				// 宝箱の数*2回 乱数を進める
				for (int i = 0; i < (int)TreasureMapDetailData._dungeonInfo[f, 8] << 1; i++)
				{
					this.GenerateRandom();
				}

				// ランク 4階ごとに+1されていくらしい
				// 1-indexedなので-1する
				var rank = (int)this._details[2] + f / 4 - 1;
				for (int i = 0; i < (int)TreasureMapDetailData._dungeonInfo[f, 8]; i++)
				{
					TreasureMapDetailData._dungeonInfo[f, i + 9] 
						= (byte)this.GetItemRank((uint)TreasureMapDataTable.TableN[rank * 4 + 1], (uint)TreasureMapDataTable.TableN[rank * 4 + 2]);
					
					byte[] details = this._details2;
					byte b = TreasureMapDetailData._dungeonInfo[f, i + 9] - 1;
					details[(int)b] = details[(int)b] + 1;
				}
				
				if (TreasureMapDetailData._dungeonInfo[f, 8] > 0)
				{
					for (int num16 = 0; num16 < (int)TreasureMapDetailData._dungeonInfo[f, 8]; num16++)
					{
						TreasureBoxInfo treasureBoxInfo = new TreasureBoxInfo(num16, (int)TreasureMapDetailData._dungeonInfo[f, 9 + num16], (int)TreasureMapDetailData._dungeonInfo[f, num16 * 2 + 13], (int)TreasureMapDetailData._dungeonInfo[f, num16 * 2 + 14]);
						int num17 = 0;
						while (num17 < this._treasureBoxInfoList[f].Count && treasureBoxInfo.Rank <= this._treasureBoxInfoList[f][num17].Rank)
						{
							num17++;
						}
						this._treasureBoxInfoList[f].Insert(num17, treasureBoxInfo);
					}
				}
			}
		}

		public string GetTreasureBoxItem(int floor, int boxIndex, int second)
		{
			this._seed = (uint)((int)((ushort)TreasureMapDetailData._dungeonInfo[floor, 0] + this.MapSeed) + second);
			for (int i = 0; i < (int)TreasureMapDetailData._dungeonInfo[floor, 8]; i++)
			{
				int num = (int)this.routineRandom(100U);
				if (i == boxIndex)
				{
					int num2 = (int)TreasureMapDetailData._dungeonInfo[floor, i + 9];
					int num3 = (int)TreasureMapDataTable.TableO[num2 - 1];
					int num4 = (int)TreasureMapDataTable.TableO[num2];
					int num5 = 0;
					for (int j = num3; j < num4; j++)
					{
						num5 += (int)TreasureMapDataTable.TableP[j];
						if (num < num5)
						{
							return TreasureMapDataTable.TableR[(int)TreasureMapDataTable.TableQ[j]];
						}
					}
				}
			}
			return null;
		}

		private uint routineRandom(uint value)
		{
			int num = (int)this.GenerateRandom();
			float num2 = (float)num - 1f;
			return (uint)(num2 * value / 32767f);
		}

		public void CalculateDetail()
		{
			this.CalculateDetail(false, false);
		}

		public void CalculateDetail(bool floorDetail)
		{
			this.CalculateDetail(floorDetail, false);
		}

		public void CalculateDetail(bool floorDetail, bool createSearchData)
		{
			this._validSeed = false;
			this._validPlaceList.Clear();
			this._lowRankValidPlaceList.Clear();
			this._middleRankValidPlaceList.Clear();
			this._highRankValidPlaceList.Clear();
			this._validRankList.Clear();
			for (int i = 0; i < 16; i++)
			{
				this._treasureBoxInfoList[i].Clear();
			}
			Array.Clear(this._details, 0, 20);
			Array.Clear(this._details2, 0, 20);
			if (this.MapRank < 2 || this.MapRank > 248)
			{
				return;
			}
			
			this._seed = (uint)this.MapSeed;
			for (int j = 0; j < 12; j++)
			{
				this.GenerateRandom(100U);
			}
			this._details[3] = (byte)this.Seek1(TreasureMapDataTable.TableA, 5);
			this._details[1] = (byte)this.Seek2(TreasureMapDataTable.TableB, this.MapRank, 9);
			this._details[2] = (byte)this.Seek2(TreasureMapDataTable.TableC, this.MapRank, 8);
			this._details[0] = (byte)this.Seek4(TreasureMapDataTable.TableD, TreasureMapDataTable.TableE, 9);
			for (int k = 0; k < 12; k++)
			{
				this._details[k + 1 + 7] = (byte)this.Seek3(TreasureMapDataTable.TableF[k * 4 + 1], TreasureMapDataTable.TableF[k * 4 + 2]);
			}
			this._details[5] = (byte)this.Seek2(TreasureMapDataTable.TableH, this._details[2], 5);
			this._details[6] = (byte)this.Seek2(TreasureMapDataTable.TableI, this._details[0], 4);
			this._details[7] = (byte)this.Seek2(TreasureMapDataTable.TableG, this._details[1], 8);

			int num = (int)((this._details[0] + this._details[1] + this._details[2] - 4) * 3);
			num += (int)(this.GenerateRandom(11U) - 5U);
			if (num < 1)
			{
				num = 1;
			}
			if (num > 99)
			{
				num = 99;
			}
			this._details[4] = (byte)num;

			this._name3Index = TreasureMapDataTable.TreasureMapName3_IndexTable[(int)((this._details[7] - 1) * 5 + this._details[3] - 1)];
			if (!createSearchData)
			{
				ReadOnlyCollection<ushort> reverseSeedTable = TreasureMapDataTable.GetReverseSeedTable(this.MapSeed);
				if (reverseSeedTable != null)
				{
					this._validSeed = true;
					foreach (ushort seed in reverseSeedTable)
					{
						this._seed = (uint)seed;
						uint num2 = this.GenerateRandom();
						foreach (byte b in TreasureMapDetailData._candidateRank)
						{
							int num3 = (int)((ulong)b + (ulong)num2 % (ulong)((long)(b / 10 * 2 + 1)) - (ulong)((long)(b / 10)));
							if (num3 > 248)
							{
								num3 = 248;
							}
							if (!this._validRankList.Contains((byte)num3))
							{
								this._validRankList.Add((byte)num3);
							}
						}
						num2 = this.GenerateRandom();
						num2 = this.GenerateRandom();
						uint num4;
						if (this.MapRank <= 50)
						{
							num4 = num2 % 47U + 1U;
						}
						else if (this.MapRank <= 80)
						{
							num4 = num2 % 131U + 1U;
						}
						else
						{
							num4 = num2 % 150U + 1U;
						}
						if (!this._validPlaceList.Contains((byte)num4))
						{
							this._validPlaceList.Add((byte)num4);
						}
						num4 = num2 % 47U + 1U;
						if (!this._lowRankValidPlaceList.Contains((byte)num4))
						{
							this._lowRankValidPlaceList.Add((byte)num4);
						}
						num4 = num2 % 131U + 1U;
						if (!this._middleRankValidPlaceList.Contains((byte)num4))
						{
							this._middleRankValidPlaceList.Add((byte)num4);
						}
						num4 = num2 % 150U + 1U;
						if (!this._highRankValidPlaceList.Contains((byte)num4))
						{
							this._highRankValidPlaceList.Add((byte)num4);
						}
					}
					if (this.MapRank == 2 && this.MapSeed == 50)
					{
						this._validPlaceList.Add(5);
						this._lowRankValidPlaceList.Add(5);
					}
				}
				else
				{
					this._validSeed = false;
					this._validPlaceList.Clear();
					this._validRankList.Clear();
				}
			}
			if (floorDetail)
			{
				for (int l = 1; l < (int)(this._details[1] + 1); l++)
				{
					int num5;
					if (l <= 4)
					{
						num5 = ((int)this.MapSeed + l) % 5 + 10;
					}
					else if (l <= 8)
					{
						num5 = ((int)this.MapSeed + l) % 4 + 12;
					}
					else if (l <= 12)
					{
						num5 = ((int)this.MapSeed + l) % 3 + 14;
					}
					else
					{
						num5 = 16;
					}
					TreasureMapDetailData._dungeonInfo[l - 1, 2] = (byte)num5;
					TreasureMapDetailData._dungeonInfo[l - 1, 3] = (byte)num5;
				}
				this.CreateDungeonDetail(createSearchData);
			}
		}

		public string BossName
		{
			get
			{
				return MonsterDataList.List[282 + (int)this._details[0] - 1];
			}
		}

		public byte BossIndex
		{
			get
			{
				return this._details[0] - 1;
			}
		}

		public int FloorCount
		{
			get
			{
				return (int)this._details[1];
			}
		}

		public int MonsterRank
		{
			get
			{
				return (int)this._details[2];
			}
		}

		public string MapTypeName
		{
			get
			{
				return TreasureMapDataTable.TreasureMapMapTypeName_Table[(int)(this._details[3] - 1)];
			}
		}

		public byte MapTypeIndex
		{
			get
			{
				return this._details[3] - 1;
			}
		}

		public List<TreasureBoxInfo>[] TreasureBoxInfoList
		{
			get
			{
				return this._treasureBoxInfoList;
			}
		}

		public int GetTreasureBoxCount(int rank)
		{
			if (rank == 0)
			{
				int num = 0;
				for (int i = 0; i < 10; i++)
				{
					num += (int)this._details2[i];
				}
				return num;
			}
			if (rank > 0 && rank <= 10)
			{
				return (int)this._details2[rank - 1];
			}
			return 0;
		}

		public int GetTreasureBoxRankPerFloor(int floor, int index)
		{
			if (floor < this.FloorCount && index < this.GetTreasureBoxCountPerFloor(floor))
			{
				return (int)TreasureMapDetailData._dungeonInfo[floor, 9 + index];
			}
			return 0;
		}

		public void GetTreasureBoxPosPerFloor(int floor, int index, out int x, out int y)
		{
			x = -1;
			y = -1;
			if (floor < this.FloorCount && index < this.GetTreasureBoxCountPerFloor(floor))
			{
				x = (int)TreasureMapDetailData._dungeonInfo[floor, index * 2 + 13];
				y = (int)TreasureMapDetailData._dungeonInfo[floor, index * 2 + 14];
			}
		}

		public int GetTreasureBoxCountPerFloor(int floor)
		{
			if (floor < this.FloorCount)
			{
				return (int)TreasureMapDetailData._dungeonInfo[floor, 8];
			}
			return 0;
		}

		public int GetFloorWidth(int floor)
		{
			if (floor < this.FloorCount)
			{
				return (int)TreasureMapDetailData._dungeonInfo[floor, 2];
			}
			return 0;
		}

		public int GetFloorHeight(int floor)
		{
			if (floor < this.FloorCount)
			{
				return (int)TreasureMapDetailData._dungeonInfo[floor, 3];
			}
			return 0;
		}

		public byte[,] GetFloorMap(int floor)
		{
			if (floor < this.FloorCount)
			{
				int floorHeight = this.GetFloorHeight(floor);
				int floorWidth = this.GetFloorWidth(floor);
				byte[,] array = new byte[floorWidth, floorHeight];
				int num = floor * 1336 + 792;
				int num2 = 0;
				for (int i = 0; i < floorHeight; i++)
				{
					Array.Copy(TreasureMapDetailData._dungeonInfo, num, array, num2, floorWidth);
					num += 16;
					num2 += floorWidth;
				}
				return array;
			}
			return null;
		}

		public bool IsUpStep(int floor, int x, int y)
		{
			return floor < this.FloorCount && x < this.GetFloorWidth(floor) && y < this.GetFloorHeight(floor) && (int)TreasureMapDetailData._dungeonInfo[floor, 4] == x && (int)TreasureMapDetailData._dungeonInfo[floor, 5] == y;
		}

		public int IsTreasureBoxRank(int floor, int x, int y)
		{
			if (floor < this.FloorCount && x < this.GetFloorWidth(floor) && y < this.GetFloorHeight(floor))
			{
				for (int i = 0; i < this.GetTreasureBoxCountPerFloor(floor); i++)
				{
					if ((int)TreasureMapDetailData._dungeonInfo[floor, i * 2 + 13] == x && (int)TreasureMapDetailData._dungeonInfo[floor, i * 2 + 14] == y)
					{
						return (int)TreasureMapDetailData._dungeonInfo[floor, i + 9];
					}
				}
			}
			return 0;
		}

		public int GetTreasureBoxIndex(int floor, int x, int y)
		{
			if (floor < this.FloorCount && x < this.GetFloorWidth(floor) && y < this.GetFloorHeight(floor))
			{
				for (int i = 0; i < this.GetTreasureBoxCountPerFloor(floor); i++)
				{
					if ((int)TreasureMapDetailData._dungeonInfo[floor, i * 2 + 13] == x && (int)TreasureMapDetailData._dungeonInfo[floor, i * 2 + 14] == y)
					{
						return i;
					}
				}
			}
			return -1;
		}

		private const int FloorMapDataOffset = 792;

		private static List<byte> _candidateRank;

		private ushort _mapSeed;

		private byte _mapRank;

		private bool _validSeed;

		private List<byte> _validRankList = new List<byte>();

		private List<byte> _validPlaceList = new List<byte>();

		private List<byte> _lowRankValidPlaceList = new List<byte>();

		private List<byte> _middleRankValidPlaceList = new List<byte>();

		private List<byte> _highRankValidPlaceList = new List<byte>();

		private uint _seed;

		private byte[] _details = new byte[20];

		private byte[] _details2 = new byte[20];

		private byte _name3Index;

		[ThreadStatic]
		private static byte[,] _dungeonInfo;

		private List<TreasureBoxInfo>[] _treasureBoxInfoList = new List<TreasureBoxInfo>[16];
	}
}
