// routineK

private int routineK()
{
	// 宝箱の個数
	var boxes = (byte)(this.GetRand(3) + 1);
	_buffer[8] = boxes;

	int cnt = 0;
	int boxIdx = 0;
	for (;;)
	{
		// Struct_Cのポインタをランダムで取得？
		// となると23は実際に生成されたStruct_Cの数か？
		var ptr = 472 + this.GetRand(_buffer[23]) * 20;

		var l = _buffer[ptr];
		var t = _buffer[ptr + 1];
		var r = _buffer[ptr + 2];
		var b = _buffer[ptr + 3];

		var x = (int)this.GetRand(l, r);
		var y = (int)this.GetRand(t, b);
		var tile = (int)_buffer[792 + x + (y * 16)];

		if ((cnt < 100 && (_buffer[0] == 3 || _buffer[0] == 1)) 
			// 既に宝箱が置かれている or 階段のマスは上書きしない
			|| tile == 6 || tile == 4 || tile == 5)
		{
			cnt++;
		}
		else
		{
			// 0x06(宝箱)を置く
			_buffer[792 + x + (y * 16)] = 6;
			_buffer[13 + boxIdx * 2] = (byte)x;
			_buffer[14 + boxIdx * 2] = (byte)y;

			boxIdx++;
			if (boxIdx >= boxes) break;
		}
	}
	return 1;
}
