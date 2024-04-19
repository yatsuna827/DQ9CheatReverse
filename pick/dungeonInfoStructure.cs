  
  // 16階層分のフロアデータ
  // 0: 階層番号
  // 1: なにかのフラグ？
  // 2: フロアのWidth (MapSeed + Floor) % n + mの形で決まっている
  // 3: フロアのHeight
  // 4: 階段のx座標
  // 5: 階段のy座標
  // 6:
  // 7: 

  //  8: 宝箱の個数
  //  9, 10, 11, 12: 宝箱のランク(つまり1フロア当たり最大で4個？)
  // 13, 15, 17, 19: 宝箱のx座標
  // 14, 16, 18, 20: 宝箱のy座標
  // 21: StructAが生成された数（生成中はカウンタとしても使用）
  // 22: StructBが生成された数（生成中はカウンタとしても使用）
  // 23: StructCが生成された数（生成中はカウンタとしても使用）

  // Struct_A
  // 24 ~ 215 ... 謎だけど12byte * 16
  // +0: 左端, 初期値:1
  // +1: 上端, 初期値:1
  // +2: 右端, 初期値:W-2
  // +3: 下端, 初期値:H-2
  // +4: routineAを実行してあったら立つフラグ
  // +5: routineEを実行してあったら立つフラグ
  // +6: 未使用？
  // +7: 未使用？
  // +8 ~ +11: Struct_C型のポインタ

  // Struct_B
  // 216 ~ 471 ... 謎だけど16byte * 16
  // +0: 左端
  // +1: 上端
  // +2: 右端
  // +3: 下端
  // +4 ~ +7:  Struct_A型のポインタ
  // +8 ~ +11: Struct_A型のポインタ
  // +12: なんかのフラグ
  // +13 ~ +15: 未使用？

  // Struct_C
  // 472 ~ ... 謎だけど20byte * 16
  // +0: 左端
  // +1: 上端
  // +2: 右端
  // +3: 下端
  // 以降は(x,y)の座標*8らしい
  // +4, +5:
  // +6, +7:
  // +8, +9:
  // +10, +11:
  // +12, +13:
  // +14, +15:
  // +16, +17:
  // +18, +19:

  // 792 ~ 1047: フロア構造データ, 左上からx軸方向に16マスずつ
  // - 792から256(=16*16)byteぶん確保されている
  // - デフォルトは01で埋められる

  // マップタイル
  // 0: 床
  // 1: 壁
  // 2: 床
  // 3: 壁
  // 4: 上階段候補
  // 5: 下階段
  // 6: 宝箱候補
  // 7: 床
  // 8: 床

class FloorData
{

  public int aIndex = 0;
  public int bIndex = 0;

  public int routineFIndex = 0;

  public byte[,] FloorMap = new byte[16,16];

  public StructA[] structAs = new StructA[16];
  public StructB[] structBs = new StructB[16];
  public StructC[] structCs = new StructC[16];
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

class StructC
{
  public byte Left, Top, Right, Bottom;

  // 8個の座標が何を表しているのかわかったら詳しくする
	public (byte x, byte y)[] Points = new (byte x, byte y)[8]; 
}
