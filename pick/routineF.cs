// routineF

var buffer = TreasureMapDetailData._dungeonInfo[floor];

// 21, 「今見ている行の番号」か？
// 0行目は全て壁、15行目も全て壁だから処理がスキップされる？
// 26,27は『床にできる幅』の最大値？もしくは

// おそらく、フロアを縦横半分ずつに分割していく処理
// 分割する線は道になる？

// 最初はaddr = 24で呼ばれる
private void routineF(StructA structA)
{
	if (_buffer[21] >= 15) return;

	// どちらも詳細がまるで不明
	// [21] = 1,  [22] = 0で初期化されている
	var addrRight =  24 + (uint)(_buffer[21] * 12); // 『右のマス』のデータのポインタ
	var addrDown  = 216 + (uint)(_buffer[22] * 16); // 『下のマス』のデータのポインタ

	// addrに対してroutineEが実行済みならroutineAを見る
	if (structA.FlagRoutineE)
	{
		var hasSeen = routineA(structA, addrRight, addrDown) == false;
		if (hasSeen) return;
	}
	// addrに対してroutineAが実行済みならroutineEを見る
	else if (structA.FlagRoutineA)
	{
		var hasSeen = routineE(structA, addrRight, addrDown) == false;
		if (hasSeen) return;
	}
	// 両方未実行ならランダムに選ぶ
	else 
	{
		var result = (this.GenerateRandom() & 1U) == 0U
			? routineA(structA, addrRight, addrDown)
			: routineE(structA, addrRight, addrDown);
		
		if (!result) return;
	}
	
	// 次へ
	routineB(structA);
}

// routineFのサブルーチン
// addr: Struct_A型のポインタ
// addrRight: Struct_A型のポインタ
// addrDown: Struct_B型のポインタ
private unsafe bool routineA(StructA structA, uint addrRight, uint addrDown)
{
	// 実行済みフラグ
	if (structA.FlagRoutineA) return false;
	
	var height = structA.Bottom - structA.Top + 1;
	if (height < 7) return false;

	// ([addr], y) ~ ([addr+2], y)を0x03(=壁)で埋める
	var y = structA.Top + this.GetRand(height - 6) + 3;
	for (int x = structA.Left; x <= structA.Right; x++) {
		floorMap[x, y] = 3;
	}

	_buffer[addrRight + 0] = _buffer[addr];     // 左端は据え置き
	_buffer[addrRight + 1] = y + 1;					// 上端が下がる
	_buffer[addrRight + 2] = _buffer[addr + 2]; // 右端は据え置き
	_buffer[addrRight + 3] = _buffer[addr + 3]; // 下端は据え置き
	_buffer[addrRight + 4] = 0;
	_buffer[addrRight + 5] = 0;

	_buffer[addr + 3] = y - 1;  // 下端は上端-2の位置
	_buffer[addr + 4] = 1;         // フラグを立てる

	_buffer[addrDown + 0] = _buffer[addr];     // 左端
	_buffer[addrDown + 1] = y;              // 上端
	_buffer[addrDown + 2] = _buffer[addr + 2]; // 右端
	_buffer[addrDown + 3] = y;              // 下端
	// addrDown + 4 ~ 7にaddr、+8 ~ +11にaddrRightを格納
	fixed (byte* ptr = &_buffer[addrDown])
	{
		((int*)ptr)[1] = (int)addr;
		((int*)ptr)[2] = (int)addrRight;
	}
	_buffer[addrDown + 12] = 1;

	return true;
}

// routineFのサブルーチン
// だいたいAと同じ処理で、ちょこっとだけ違う
private unsafe bool routineE(uint addr, uint addrRight, uint addrDown)
{
	// 実行済みフラグ
	if (_buffer[addr + 5] != 0)
		return false;

	var temp = _buffer[addr + 2] - _buffer[addr] + 1;
	if (temp < 7)
		return false;

	var x = _buffer[addr] + this.GetRand(num - 7 + 1) + 3;
	// (x, [addr+1]) ~ (x, [addr+3])を0x03(=壁)で埋める
	for (int y = _buffer[addr + 1]; y <= _buffer[addr + 3]; y++)
		_buffer[792 + x + (y * 16)] = 3;
	
	_buffer[addrRight + 0] = x + 1;
	_buffer[addrRight + 1] = _buffer[addr + 1];
	_buffer[addrRight + 2] = _buffer[addr + 2];
	_buffer[addrRight + 3] = _buffer[addr + 3];
	_buffer[addrRight + 4] = 0;
	_buffer[addrRight + 5] = 0;

	_buffer[addr + 2] = x - 1;
	_buffer[addr + 5] = 1;
	
	_buffer[addrDown + 0] = x;
	_buffer[addrDown + 1] = _buffer[addr + 1];
	_buffer[addrDown + 2] = x;
	_buffer[addrDown + 3] = _buffer[addr + 3];
	// value2 + 4 ~ 7にaddr、+8 ~ +11にvalue1を格納
	fixed (byte* ptr = &_buffer[addrDown])
	{
		((int*)ptr)[1] = (int)addr;
		((int*)ptr)[2] = (int)addrRight;
	}
	_buffer[addrDown + 12] = 2;

	return true;
}


// routineFのサブルーチン
// 中でroutineFが呼び出されている
private void routineB(uint addr)
{
	// 今見ている「何」のインデックス？
	int cur = (int)_buffer[21];

	_buffer[21] += 1;
	_buffer[22] += 1;

	// 乱数でどちらを先に処理するかが変わる
	if ((this.GenerateRandom() & 1U) != 0U) {
		this.routineF(addr); // A or Eの残り一方を処理する
		this.routineF((uint)(cur * 12 + 24)); // 『次』に移る
	} else {
		this.routineF((uint)(cur * 12 + 24));
		this.routineF(addr);
	}
}

// ---

private uint GetItemRank(uint value1, uint value2)
{
	int num = (int)this.GenerateRandom();
	float num2 = (float)num;
	num2 -= 1f;
	int num3 = (int)(value2 - value1 + 1U);
	num2 = (float)num3 * num2 / 32767f;
	return (uint)num2 + value1;
}



class StructA
{
  public byte Left, Top, Right, Bottom;

  public byte FlagRoutineA, FlagRoutineE;

  public StructB? structB = null;
}

class StructB
{
  public byte Left, Top, Right, Bottom;

  public StructA? structA1;
  public StructA? structA2;

  public byte flag;
}
