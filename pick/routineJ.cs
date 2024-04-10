// routineJ

private void routineJ()
{
	int num = 0;
	int num2 = 0;
	int num5;
	int num6;
	for (;;)
	{
		int num3 = (int)this.GenerateRandom(0U, (uint)(_buffer[23] - 1));
		int num4 = num3 * 20 + 472;
		num5 = (int)this.GenerateRandom((uint)_buffer[num4], (uint)_buffer[num4 + 2]);
		num6 = (int)this.GenerateRandom((uint)_buffer[num4 + 1], (uint)_buffer[num4 + 3]);
		int num7;
		int num8;
		int num9;
		int num10;
		if (num2 < 100)
		{
			num7 = (int)_buffer[(num6 - 1 << 4) + num5 + 792];
			num8 = (int)_buffer[(num6 + 1 << 4) + num5 + 792];
			num9 = (int)_buffer[(num6 << 4) + 792 + num5 - 1];
			num10 = (int)_buffer[(num6 << 4) + 792 + num5 + 1];
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
			int num11 = (int)this.routineI((uint)num5, (uint)num6, 0U);
			int num12 = num11 & 255;
			if (num12 == 234 || num12 == 171 || num12 == 174 || num12 == 186 || num12 == 163 || num12 == 142 || num12 == 58 || num12 == 232 || num12 == 184 || num12 == 226 || num12 == 139 || num12 == 46)
			{
				num2++;
				continue;
			}
		}
		// 昇り階段
		_buffer[num5 + (num6 << 4) + 792] = 4;
		_buffer[4] = (byte)num5;
		_buffer[5] = (byte)num6;
		num8 = num5;
		num9 = num6;
		for (;;)
		{
			num6 = (int)this.GenerateRandom(0U, (uint)(_buffer[23] - 1));
			if (num6 == (num3 & 255) && (num & 255) < 25)
			{
				num++;
			}
			else
			{
				num4 = num6 * 20 + 472;
				num5 = (int)this.GenerateRandom((uint)_buffer[num4], (uint)_buffer[num4 + 2]);
				num7 = num6;
				num6 = (int)this.GenerateRandom((uint)_buffer[num4 + 1], (uint)_buffer[num4 + 3]);
				if ((num3 & 255) != num7 || num5 != num8 || num6 != num9)
				{
					break;
				}
			}
		}
		num7 = (int)_buffer[num5 + (num6 - 1 << 4) + 792];
		num8 = (int)_buffer[num5 + (num6 + 1 << 4) + 792];
		num9 = (int)_buffer[num5 + (num6 << 4) + 792 - 1];
		num10 = (int)_buffer[num5 + (num6 << 4) + 792 + 1];
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
	_buffer[num5 + (num6 << 4) + 792] = 5;
	_buffer[6] = (byte)num5;
	_buffer[7] = (byte)num6;
}

private uint routineI(uint value1, uint value2, uint value3)
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
		num = (int)_buffer[(value1 + (value2 << 4) + 792U)];
		if (num == 1 || num == 3)
		{
			return 255U;
		}
		num2 = (int)_buffer[(value1 + (value2 - 1U << 4) + 792U)];
		num3 = (int)_buffer[(value1 + (value2 - 1U << 4) + 792U - 1U)];
		num4 = (int)_buffer[(value1 + (value2 - 1U << 4) + 792U + 1U)];
		num5 = (int)_buffer[(value1 + (value2 << 4) + 792U + 1U)];
		num6 = (int)_buffer[(value1 + (value2 + 1U << 4) + 792U + 1U)];
		num7 = (int)_buffer[(value1 + (value2 + 1U << 4) + 792U)];
		num8 = (int)_buffer[(value1 + (value2 + 1U << 4) + 792U - 1U)];
		num9 = (int)_buffer[(value1 + (value2 << 4) + 792U - 1U)];
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
