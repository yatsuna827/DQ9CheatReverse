// GenerateFloorMap

// addr: Struct_A型のポインタ
private unsafe void GenerateFloorMap(uint addr)
{
	int num;
	fixed (byte* ptr = &_buffer[addr + 8])
	{
		num = *(int*)ptr;
	}

	// 何かの中央
	int midX = (int)((_buffer[num + 2] - _buffer[num] + 1) / 2);
	int midY = (int)((_buffer[num + 3] - _buffer[num + 1] + 1) / 2);

	// top == 0
	var flag = (_buffer[1] == 0 && this.GetRand(16) == 0U);

	int value3;
	int value4;
	int value5;
	int num3;
	if (!flag)
	{
		value3 = (int)(_buffer[addr + 3] - _buffer[num + 3]);
		value4 = (int)(_buffer[num] - _buffer[addr]);
		value5 = (int)(_buffer[addr + 2] - _buffer[num + 2]);
		num3 = (int)(_buffer[num + 1] - _buffer[addr + 1]);
	}
	else
	{
		value3 = (int)(_buffer[3] - _buffer[num + 3] - 1);
		value4 = (int)(_buffer[num] - 1);
		value5 = (int)(_buffer[2] - _buffer[num + 2] - 1);
		num3 = (int)(_buffer[num + 1] - 1);
		_buffer[1] = 1;
	}

	// なんか…左右のマスを見て壁を作ったりしていく？

	for (int i = (int)_buffer[num]; i <= (int)_buffer[num + 2]; i++)
	{
		int tile = (int)_buffer[792 + i + (_buffer[num + 1] * 16)];
		if (tile == 1 || tile == 3) continue;

		if (this.GetRand(2) == 0U)
		{
			if (tile == 8) continue;

			int tileU = _buffer[792 + i + ((_buffer[num + 1] - 1) * 16)];
			if (tileU == 1 || tileU == 8) continue;
			
			var num4 = (int)this.GenerateRandom(0U, (uint)midY);
			int num6 = i - 1;
			for (int j = 0; j < num4; j++)
			{
				if (
						_buffer[792 + i + _buffer[num + 1] + (j * 16)] == 8
				|| _buffer[792 + i + _buffer[num + 1] + j + 1 * 16] == 1
				||  (_buffer[792 + i + 1 + _buffer[num + 1] + j * 16] != 1
					&& _buffer[792 + i + 1 + _buffer[num + 1] + j + 1 * 16] == 1) // ここ、(j+1) << 4の間違い？
				||  (_buffer[792 + num6 + _buffer[num + 1] + j * 16] != 1 
					&& _buffer[792 + num6 + _buffer[num + 1] + j + 1 * 16] == 1))
				{
					break;
				}

				_buffer[792 + i + _buffer[num + 1] + j * 16] = 1;
			}
		}
		else
		{
			var num4 = (int)this.GenerateRandom(0U, (uint)num3);
			for (int k = 0; k < num4; k++)
			{
				int p = 792 + i + _buffer[num + 1] - 1 - k * 16;
				if (_buffer[p] != 8)
				{
					_buffer[p] = 0;
				}
			}
		}
	}
	
	for (int l = (int)_buffer[num]; l <= (int)_buffer[num + 2]; l++)
	{
		var tile = (int)_buffer[l + ((int)_buffer[num + 3] * 16) + 792];
		if (tile == 1 || tile == 3) continue;

		if (this.GenerateRandom(0U, 1U) != 0U)
		{
			var num3 = (int)this.GenerateRandom(0U, (uint)value3);
			for (int m = 0; m < num3; m++)
			{
				if (_buffer[l + ((int)_buffer[num + 3] + m + 1 * 16) + 792] != 8)
				{
					_buffer[l + ((int)_buffer[num + 3] + m + 1 * 16) + 792] = 0;
				}
			}
		}
		else if (tile != 8)
		{
			var tileBottom = (int)_buffer[792 + l + ((_buffer[num + 3] + 1) * 16)];
			if (tileBottom == 1 || tileBottom == 8) continue;

			var num9 = (int)this.GenerateRandom(0U, (uint)midY);
			var num3 = l - 1;

			var num10 = 0;
			while (num10 < num9 && _buffer[l + ((int)_buffer[num + 3] - num10 * 16) + 792] != 8 && _buffer[l + ((int)_buffer[num + 3] - num10 - 1 * 16) + 792] != 1 && (_buffer[((int)_buffer[num + 3] - num10 * 16) + l + 1 + 792] == 1 || _buffer[((int)_buffer[num + 3] - num10 - 1 * 16) + l + 1 + 792] != 1) && (_buffer[num3 + ((int)_buffer[num + 3] - num10 * 16) + 792] == 1 || _buffer[num3 + ((int)_buffer[num + 3] - num10 - 1 * 16) + 792] != 1))
			{
				_buffer[792 + l + ((int)_buffer[num + 3] - num10 * 16)] = 1;
				num10++;
			}
		}
	}
	
	for (int n = (int)_buffer[num + 1]; n <= (int)_buffer[num + 3]; n++)
	{
		num3 = (int)_buffer[(n * 16) + 792 + (int)_buffer[num]];
		if (num3 != 1 && num3 != 3)
		{
			if (this.GenerateRandom(0U, 1U) != 0U)
			{
				num3 = (int)this.GenerateRandom(0U, (uint)value4);
				for (int num11 = 0; num11 < num3; num11++)
				{
					if (_buffer[(n * 16) + 792 + (int)_buffer[num] - 1 - num11] != 8)
					{
						_buffer[(n * 16) + 792 + (int)_buffer[num] - 1 - num11] = 0;
					}
				}
			}
			else if (num3 != 8)
			{
				int num12 = (int)_buffer[(n * 16) + 792 + (int)_buffer[num] - 1];
				if (num12 != 1 && num12 != 8)
				{
					int num13 = (int)this.GenerateRandom(0U, (uint)midX);
					num3 = n - 1;
					int num14 = 0;
					while (num14 < num13 && _buffer[num14 + (n * 16) + (int)_buffer[num] + 792] != 8 && _buffer[num14 + (n * 16) + (int)_buffer[num] + 792 + 1] != 1 && (_buffer[num14 + (n + 1 * 16) + (int)_buffer[num] + 792] == 1 || _buffer[num14 + (n + 1 * 16) + (int)_buffer[num] + 792 + 1] != 1) && (_buffer[num14 + (num3 * 16) + (int)_buffer[num] + 792] == 1 || _buffer[num14 + (num3 * 16) + (int)_buffer[num] + 792 + 1] != 1))
					{
						_buffer[num14 + (n * 16) + (int)_buffer[num] + 792] = 1;
						num14++;
					}
				}
			}
		}
	}
	
	for (int num15 = (int)_buffer[num + 1]; num15 <= (int)_buffer[num + 3]; num15++)
	{
		num3 = (int)_buffer[(num15 * 16) + 792 + (int)_buffer[num + 2]];
		if (num3 != 1 && num3 != 3)
		{
			if (this.GenerateRandom(0U, 1U) != 0U)
			{
				num3 = (int)this.GenerateRandom(0U, (uint)value5);
				for (int num16 = 0; num16 < num3; num16++)
				{
					if (_buffer[num16 + (num15 * 16) + (int)_buffer[num + 2] + 792 + 1] != 8)
					{
						_buffer[num16 + (num15 * 16) + (int)_buffer[num + 2] + 792 + 1] = 0;
					}
				}
			}
			else if (num3 != 8)
			{
				int num17 = (int)_buffer[(int)_buffer[num + 1] + (num15 * 16) + 792 + 1];
				if (num17 != 1 && num17 != 8)
				{
					int num18 = (int)this.GenerateRandom(0U, (uint)midX);
					num3 = num15 - 1;
					int num19 = 0;
					while (num19 < num18 && _buffer[(num15 * 16) + 792 + (int)_buffer[num + 2] - num19] != 8 && _buffer[(num15 * 16) + 792 + (int)_buffer[num + 2] - num19 - 1] != 1 && (_buffer[(num15 + 1 * 16) + 792 + (int)_buffer[num + 2] - num19] == 1 || _buffer[(num15 + 1 * 16) + 792 + (int)_buffer[num + 2] - num19 - 1] != 1) && (_buffer[(num3 * 16) + 792 + (int)_buffer[num + 2] - num19] == 1 || _buffer[(num3 * 16) + 792 + (int)_buffer[num + 2] - num19 - 1] != 1))
					{
						_buffer[(num15 * 16) + 792 + (int)_buffer[num + 2] - num19] = 1;
						num19++;
					}
				}
			}
		}
	}
}
