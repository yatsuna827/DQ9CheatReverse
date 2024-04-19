// routineF

var buffer = TreasureMapDetailData._dungeonInfo[floor];

// 21, 「今見ている行の番号」か？
// 0行目は全て壁、15行目も全て壁だから処理がスキップされる？
// 26,27は『床にできる幅』の最大値？もしくは

// おそらく、フロアを縦横半分ずつに分割していく処理
// 分割する線は道になる？

// 最初はaddr = 24で呼ばれる
private void routineF(StructA current, int index)
{
	if (_buffer[21] >= 15) return;

	StructA next = structAs[index + 1];
	StructB strB = structBs[index];

	// addrに対してroutineEが実行済みならroutineAを見る
	if (structA.FlagRoutineE)
	{
		var hasSeen = routineA(structA, next, strB) == false;
		if (hasSeen) return;
	}
	// addrに対してroutineAが実行済みならroutineEを見る
	else if (structA.FlagRoutineA)
	{
		var hasSeen = routineE(structA, next, strB) == false;
		if (hasSeen) return;
	}
	// 両方未実行ならランダムに選ぶ
	else 
	{
		var result = (this.GetRand() & 1U) == 0U
			? routineA(structA, next, strB)
			: routineE(structA, next, strB);
		
		if (!result) return;
	}
	
	// 次へ
	routineB(structA);
}

// routineFのサブルーチン
private unsafe bool routineA(StructA current, StructA next, StructB strB)
{
	// 実行済みフラグ
	if (current.FlagRoutineA) return false;
	
	var height = current.Bottom - current.Top + 1;
	if (height < 7) return false;

	// ([addr], y) ~ ([addr+2], y)を0x03(=壁)で埋める
	var y = current.Top + this.GetRand(height - 6) + 3;
	for (int x = current.Left; x <= current.Right; x++) {
		floorMap[x, y] = 3;
	}

	next.Left = current.Left;
	next.Top = y + 1;
	next.Right = current.Right;
	next.Bottom = current.Bottom;
	next.FlagRoutineA = false;
	next.FlagRoutineE = false;

	current.Bottom = y - 1;
	current.FlagRoutineA = true;

	strB.Left = current.Left;
	strB.Top = y;
	strB.Right = current.Right;
	strB.Bottom = y;

	strB.structA1 = current;
	strB.structA2 = next;
	strB.flag = 1;

	return true;
}

// routineFのサブルーチン
// X軸方向の処理
private unsafe bool routineE(StructA current, StructA next, StructB strB)
{
	// 実行済みフラグ
	if (current.FlagRoutineE) return false;

	var width = current.Right - current.Left + 1;
	if (width < 7)
		return false;

	var x = current.Left + this.GetRand(width - 7 + 1) + 3;
	// (x, [addr+1]) ~ (x, [addr+3])を0x03(=壁)で埋める
	for (int y = current.Top; y <= current.Bottom; y++) {
		floorMap[x, y] = 3;
	}
	
	next.Left = x + 1;
	next.Top = current.Top;
	next.Right = current.Right;
	next.Bottom = current.Bottom;
	next.FlagRoutineA = false;
	next.FlagRoutineE = false;

	current.Right = x - 1;
	current.FlagRoutineE = true;
	
	strB.Left = x;
	strB.Top = current.Top;
	strB.Right = x;
	strB.Bottom = current.Bottom;
	strB.structA1 = current;
	strB.structA2 = next;

	strB.flag = 2;

	return true;
}


// routineFのサブルーチン
// 中でroutineFが呼び出されている
private void routineB(StructA current, int index)
{
	if ((this.GetRand() & 1U) != 0U) {
		this.routineF(addr, index + 1);
		this.routineF(structAs[index], index + 1);
	} else {
		this.routineF(structAs[index], index + 1);
		this.routineF(addr, index + 1);
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
