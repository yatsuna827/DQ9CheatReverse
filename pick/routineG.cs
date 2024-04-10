// routineG

private unsafe int routineG(uint addr)
{
	if (_buffer[(int)(addr + 12U)] == 1)
	{
		int addr2;
		int addr3;
		fixed (byte* ptr = &_buffer[0])
		{
			addr2 = (int)((uint*)ptr + ((uint*)ptr + addr / 4U)[1] / 4U)[2];
			addr3 = (int)((uint*)ptr + ((uint*)ptr + addr / 4U)[2] / 4U)[2];
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
		this.routineH((uint)addr2, 16U, (uint)addr3, 12U, addr, (uint)value);
		if (this.GenerateRandom(0U, 7U) != 0U)
		{
			value = 0;
		}
		else
		{
			value = 1;
		}
		this.routineH((uint)addr2, 18U, (uint)addr3, 12U, addr, (uint)value);
		if (this.GenerateRandom(0U, 7U) != 0U)
		{
			value = 0;
		}
		else
		{
			value = 1;
		}
		this.routineH((uint)addr2, 16U, (uint)addr3, 14U, addr, (uint)value);
		if (this.GenerateRandom(0U, 7U) != 0U)
		{
			value = 0;
		}
		else
		{
			value = 1;
		}
		this.routineH((uint)addr2, 18U, (uint)addr3, 14U, addr, (uint)value);
	}
	else if (_buffer[(int)(addr + 12U)] == 2)
	{
		int addr2;
		int addr3;
		fixed (byte* ptr2 = &_buffer[0])
		{
			addr2 = (int)((uint*)ptr2 + ((uint*)ptr2 + addr / 4U)[1] / 4U)[2];
			addr3 = (int)((uint*)ptr2 + ((uint*)ptr2 + addr / 4U)[2] / 4U)[2];
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
		this.routineH((uint)addr2, 8U, (uint)addr3, 4U, addr, (uint)value);
		if (this.GenerateRandom(0U, 7U) != 0U)
		{
			value = 0;
		}
		else
		{
			value = 1;
		}
		this.routineH((uint)addr2, 10U, (uint)addr3, 4U, addr, (uint)value);
		if (this.GenerateRandom(0U, 7U) != 0U)
		{
			value = 0;
		}
		else
		{
			value = 1;
		}
		this.routineH((uint)addr2, 8U, (uint)addr3, 6U, addr, (uint)value);
		if (this.GenerateRandom(0U, 7U) != 0U)
		{
			value = 0;
		}
		else
		{
			value = 1;
		}
		this.routineH((uint)addr2, 10U, (uint)addr3, 6U, addr, (uint)value);
	}
	return 1;
}


private bool routineH(uint addr1, uint value1, uint addr2, uint value2, uint addr3, uint value3)
{
	int num;
	int num2;
	int num3;
	int num4;
	checked
	{
		num = (int)_buffer[(addr1 + value1)];
		num2 = (int)_buffer[(addr1 + value1 + 1U)];
		num3 = (int)_buffer[(addr2 + value2)];
		num4 = (int)_buffer[(addr2 + value2 + 1U)];
		if (num == 0 || num2 == 0 || num3 == 0 || num4 == 0)
		{
			return false;
		}
	}
	if (checked(_buffer[(addr3 + 12U)]) == 1)
	{
		for (int i = num2 + 1; i < (int)(_buffer[(int)(addr3 + 1U)] + 1); i++)
		{
			_buffer[(i << 4) + 792 + num] = 2;
		}
		for (int j = num4 - 1; j > (int)_buffer[(int)(addr3 + 1U)]; j--)
		{
			_buffer[(j << 4) + 792 + num3] = 2;
		}
		if (num < num3)
		{
			for (int k = num; k < num3 + 1; k++)
			{
				_buffer[k + ((int)_buffer[(int)(addr3 + 1U)] << 4) + 792] = 2;
			}
		}
		else if (num > num3)
		{
			for (int l = num3; l < num + 1; l++)
			{
				_buffer[l + ((int)_buffer[(int)(addr3 + 1U)] << 4) + 792] = 2;
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
				int num5 = (int)_buffer[m + ((int)_buffer[(int)(addr3 + 1U)] << 4) + 792];
				if (num5 != 1 && num5 != 3)
				{
					for (int n = num3 + 1; n < m; n++)
					{
						_buffer[n + ((int)_buffer[(int)(addr3 + 1U)] << 4) + 792] = 2;
					}
					break;
				}
			}
			for (int num6 = num - 1; num6 >= 0; num6--)
			{
				int num7 = (int)_buffer[num6 + ((int)_buffer[(int)(addr3 + 1U)] << 4) + 792];
				if (num7 != 1 && num7 != 3)
				{
					for (int num8 = num - 1; num8 > num6; num8--)
					{
						_buffer[num8 + ((int)_buffer[(int)(addr3 + 1U)] << 4) + 792] = 2;
					}
					break;
				}
			}
		}
		else if (num >= num3)
		{
			for (int num9 = num + 1; num9 < 16; num9++)
			{
				int num10 = (int)_buffer[num9 + ((int)_buffer[(int)(addr3 + 1U)] << 4) + 792];
				if (num10 != 1 && num10 != 3)
				{
					for (int num11 = num + 1; num11 < num9; num11++)
					{
						_buffer[num11 + ((int)_buffer[(int)(addr3 + 1U)] << 4) + 792] = 2;
					}
					break;
				}
			}
			for (int num12 = num3 - 1; num12 >= 0; num12--)
			{
				int num13 = (int)_buffer[num12 + ((int)_buffer[(int)(addr3 + 1U)] << 4) + 792];
				if (num13 != 1 && num13 != 3)
				{
					for (int num14 = num3 - 1; num14 > num12; num14--)
					{
						_buffer[num14 + ((int)_buffer[(int)(addr3 + 1U)] << 4) + 792] = 2;
					}
					break;
				}
			}
		}
	}
	else if (checked(_buffer[(addr3 + 12U)]) == 2)
	{
		for (int num15 = num + 1; num15 < (int)(_buffer[(int)addr3] + 1); num15++)
		{
			_buffer[num15 + (num2 << 4) + 792] = 2;
		}
		for (int num16 = num3 - 1; num16 > (int)_buffer[(int)addr3]; num16--)
		{
			_buffer[num16 + (num4 << 4) + 792] = 2;
		}
		if (num2 < num4)
		{
			for (int num17 = num2; num17 < num4 + 1; num17++)
			{
				_buffer[(num17 << 4) + 792 + (int)_buffer[(int)addr3]] = 2;
			}
		}
		else if (num2 > num4)
		{
			for (int num18 = num4; num18 < num2 + 1; num18++)
			{
				_buffer[(num18 << 4) + 792 + (int)_buffer[(int)addr3]] = 2;
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
				int num20 = (int)_buffer[(num19 << 4) + 792 + (int)_buffer[(int)addr3]];
				if (num20 != 1 && num20 != 3)
				{
					for (int num21 = num4 + 1; num21 < num19; num21++)
					{
						_buffer[(num21 << 4) + 792 + (int)_buffer[(int)addr3]] = 2;
					}
					break;
				}
			}
			for (int num22 = num2 - 1; num22 >= 0; num22--)
			{
				int num23 = (int)_buffer[(num22 << 4) + 792 + (int)_buffer[(int)addr3]];
				if (num23 != 1 && num23 != 3)
				{
					for (int num24 = num2 - 1; num24 > num22; num24--)
					{
						_buffer[(num24 << 4) + 792 + (int)_buffer[(int)addr3]] = 2;
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
				int num26 = (int)_buffer[(num25 << 4) + 792 + (int)_buffer[(int)addr3]];
				if (num26 != 1 && num26 != 3)
				{
					for (int num27 = num2 + 1; num27 < num25; num27++)
					{
						_buffer[(num27 << 4) + 792 + (int)_buffer[(int)addr3]] = 2;
					}
					break;
				}
				num25++;
			}
			for (num25 = num4 - 1; num25 >= 0; num25--)
			{
				int num28 = (int)_buffer[(num25 << 4) + 792 + (int)_buffer[(int)addr3]];
				if (num28 != 1 && num28 != 3)
				{
					for (int num29 = num4 - 1; num29 > num25; num29--)
					{
						_buffer[(num29 << 4) + 792 + (int)_buffer[(int)addr3]] = 2;
					}
					break;
				}
			}
		}
	}
	return true;
}
