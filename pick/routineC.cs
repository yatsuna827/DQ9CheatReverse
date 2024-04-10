// routineC

// addr1:  24 + 12 * i, Struct_A型のポインタ
// addr2: 472 + 20 * k, Struct_C型のポインタ
private unsafe bool routineC(StructA addr1, StructC addr2)
{
	if ((addr1.Right - addr1.Left + 1) < 3) return false;
	if ((addr2.Bottom - addr2.Top + 1) < 3) return false;
	
	var left = addr1.Left;
	var top = addr1.Top;
	var right = addr1.Right;
	var bottom = addr1.Bottom;

	var (a, b) = this.GetRand(2) != 0 ? (left, top) : (left+1, top+1);
	var nextL = (int)this.GetRand(a, (uint)(left + (right - left + 1) / 3));
	var nextT = (int)this.GetRand(b, (uint)(top + (bottom - top + 1) / 3));
	
	var (c, d) = this.GetRand(2) != 0 ? (right, bottom) : (right-1, bottom-1);
	var nextR = (int)this.GetRand((uint)(left + (right - left + 1) / 3 * 2), c);
	var nextB = (int)this.GetRand((uint)(top + (bottom - top + 1) / 3 * 2), d);

	addr2.Left = nextL;
	addr2.Top = nextT;
	addr2.Right = nextR;
	addr2.Bottom = nextB;
	
	addr1.structB = addr2

	// 0x00(=床)で埋める
	for (int y = nextT; y <= nextB; y++) {
		for (int x = nextL; x <= nextR; x++) {
			floorMap[x, y] = 0;
		}
	}

	routineD(addr2);
	return true;
}

// routineCのサブルーチン
// addr: Struct_C型のポインタ
private bool routineD(StructC addr)
{
	int left   = addr.Left;
	int top    = addr.Top;
	int right  = addr.Right;
	int bottom = addr.Bottom;

	// routineCの中で0ではない値が格納されているはず
	if (left == 0 || top == 0 || right == 0 || bottom == 0) 
		return false;

	// エリアの上辺と底辺をランダムに0x08(=床)にする
	if (right - left + 1 < 5)
	{
		var p1 = (x: this.GetRand(left, right), y: top);
		var p2 = (x: this.GetRand(left, right), y: bottom);

		addr.Points[4] = p1;
		addr.Points[6] = p2;

		floorMap[p1.x, p1.y] = 8;
		floorMap[p2.x, p2.y] = 8;
	}
	else
	{
		var mid = left + (right - left + 1) / 2 - 1;

		var p1 = (x: this.GetRand(left, mid), y: top);
		var p2 = (x: this.GetRand(mid + 1, right), y: top);
		var p3 = (x: this.GetRand(left, mid), y: bottom);
		var p4 = (x: this.GetRand(mid + 1, right), y: bottom);

		addr.Points[4] = p1;
		addr.Points[5] = p2;
		addr.Points[6] = p3;
		addr.Points[7] = p4;

		floorMap[p1.x, p1.y] = 8;
		floorMap[p2.x, p2.y] = 8;
		floorMap[p3.x, p3.y] = 8;
		floorMap[p4.x, p4.y] = 8;
	}
	
	// エリアの左辺と右辺をランダムに0x08(=床)にする
	if (bottom - top + 1 < 5)
	{
		var p1 = (x: left, y: this.GetRand(top, bottom));
		var p2 = (x: right, y: this.GetRand(top, bottom));

		addr.Points[0] = p1;
		addr.Points[2] = p2;

		floorMap[p1.x, p1.y] = 8;
		floorMap[p2.x, p2.y] = 8;
	}
	else
	{
		var mid = top + (bottom + 1 - top) / 2 - 1;
		
		var p1 = (x: left, y: this.GetRand(top, mid));
		var p2 = (x: left, y: this.GetRand(mid + 1, bottom));
		var p3 = (x: right, y: this.GetRand(top, mid));
		var p4 = (x: right, y: this.GetRand(mid + 1, bottom));

		addr.Points[0] = p1;
		addr.Points[1] = p2;
		addr.Points[2] = p3;
		addr.Points[3] = p4;

		floorMap[p1.x, p1.y] = 8;
		floorMap[p2.x, p2.y] = 8;
		floorMap[p3.x, p3.y] = 8;
		floorMap[p4.x, p4.y] = 8;
	}
	
	return true;
}
