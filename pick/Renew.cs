

class TreasureMapDetailData {
  private static byte[,] _dungeonInfo = new byte[16, 1336];
  // 16階層分のフロアデータ
  // 0: 階層番号
  // 1: なにかのフラグ？
  // 2: フロアのWidth (MapSeed + Floor) % n + mの形で決まっている
  // 3: フロアのHeight
  // 4: 階段のx座標
  // 5: 階段のy座標
  // 6:
  // 7: 

  // 8: 宝箱の個数
  //  9, 10, 11, 12: 宝箱のランク(つまり1フロア当たり最大で4個？)
  // 13, 15, 17, 19: 宝箱のx座標
  // 14, 16, 18, 20: 宝箱のy座標

  // 21: 1で初期化され、routineFの中でインクリメントされる, 最大15
  // 22: 0で初期化され、routineFの中でインクリメントされる, 最大14?
  // 23: 謎。

  // 謎だけど12byteで1セット
  // 24:
  // 25:
  // 26: Width-2が格納される
  // 27: Height-2が格納される
  // 28: routineAを実行してあったら立つフラグ
  // 29: routineEを実行してあったら立つフラグ
  // 31:
  // 32:
  // 33,34,35,36: なんかのぽいんた
  // 
  // 792 ~ 1047: フロア構造データ, 左上からx軸方向に16マスずつ
  // - 792から256(=16*16)byteぶん確保されている
  // - デフォルトは01で埋められる

  private byte[] _details = new byte[20];
  // 0: ボスID（1-oritented注意）
  // 1: フロア数(0-oriented?)
  // 2: 敵ランク
  // 3: マップタイプID(1-o(ry)
  // 4: 地図レベル
  // 5: 地図名1 のIndex
  // 6: 地図名2 のIndex
  // 7: 地図名3 のIndex
  // 8 ~ 19:用途不明、min,max = TableF[k*4 + 1], TableF[k*4 + 2]としてGetRand(min,max)したもの

  private byte[] _details2 = new byte[20];
  // よぐわがんにゃい
  // 0~9は各ランクの宝箱の数をメモってある
  // 10~19は用途不明

  public TreasureMapDetailData() {

  }

  // テーブルを前から走査していって、和がrを超えない最大の項を返す
  //   要するにエンカウントテーブルみたいな感じの構造体
  // 引数にはTreasureMapDataTable.TableAが渡されてきていて、size=5らしい
  private uint Seek1(ref uint seed, byte[] table, int tableSize) {
    var r = seed.GetRand(100u);
    
    var n = 0u;
    for (int i=0; i<tableSize; i++) {
      n += (uint)table[i*4 + 1];
      if (r < n) return (uint)table[i*4];
    }

    return 0u;
  }

  // tableは4個ずつ1まとまりのデータが並んでいる
  // valueがt[0]~t[1]の範囲ならt[2]~t[3]の乱数を発生させる、ということらしい
  // そういうことするなよ…という気持ち
  private uint Seek2(ref uint seed, byte[] table, byte value, int tableSize) {
    for (int i=0; i<tableSize; i+=4) {
      if (value >= table[i] && value <= table[i + 1]) {
        var (min, max) = (table[i + 2], table[i + 3]);
        return seed.GetRand(min, max);
      }
    }

    return 0u;
  }

  // なにこれ…。
  // たぶんデータ構造がカスなせいでゴミみたいなコードを書いている
  private uint Seek4(ref uint seed, byte[] table1, byte[] table2, int table1Size) {
    for (int i=0; i<table1Size; i+=4) {
      if (this.MapRank >= table1[i] && this.MapRank <= table1[i+1]) {
        var (min, max) = (table1[i+2], table1[i+3]);

        var n = 0u;
        for (int k=min; k<=max; k++)
          n += table2[(k-1) * 2 + 1];
        
        var r = seed.GetRand(n);

        var m = 0u;
        for (int k=min; k<=max; k++) {
          m += table2[(k-1) * 2 + 1];
          if (r < m) return k;
        }
        break;
      }
    }

    return 0u;
  }

  // floor, 792, 1, 256
  // 指定した範囲のアドレスをvalueで埋める処理
  // dungeonInfo[floor, 792 ~ 792 + 255]
  private unsafe void paddingDungeonInfo(int floor, uint offset, byte value, uint range)
  {
    if (range == 0) return;

    var addr = offset;

    // 4byteずつ一気に埋める
    if (range >= 4)
    {
      var temp = ((int)value << 8) | value;
      var vector = (temp << 16) | temp;
      
      for (var i = range >> 2; i > 0U; i--)
      {
        fixed (byte* ptr = &checked(TreasureMapDetailData._dungeonInfo[floor, addr]))
        {
          *(int*)ptr = (int)vector;
        }

        addr += 4u
      }
    }

    // 端数を埋める
    if ((range & 3U) != 0U)
    {
      for (int i = range & 3; i > 0U; i--)
      {
        TreasureMapDetailData._dungeonInfo[floor, addr] = value;
        addr++;
      }
    }
  }


  public void Detail() {
    var seed = MapSeed;

    seed.Advance(12); // GetRand(100)を12回繰り返しているが用途不明

    var mapTypeId = Seek1(TableA, 5);
    var floorCount = Seek2(TableB, MapRank, 9);
    var enemyRank = Seek2(TableC, MapRank, 8);
    var bossId = Seek4(TableD, TableE, 9);
    for (int i=0; i<12; i++) {
      var min = TableF[i*4 + 1];
      var max = TableF[i*4 + 2];
      _details[8 + i] = seed.GetRand(min, max); // min >= maxならseedは進まない
    }
    var mapName1 = Seek2(TableH, enemyRank, 5);
    var mapName2 = Seek2(TableI, bossId, 4);
    var mapName3 = Seek2(TableG, floorCount, 8); // 実際の名前はマップタイプに応じて決まる

    var treasureMapLevel = ((bossId + floorCount + enemyRank -4) *3 + (int)seed.GetRand(11) - 5).Clamp(1, 99);

    // ここからフロア決定に入るよ！

  }

  public int GetFloorSize(int floor) {
    if (floor <= 4) return (MapSeed + floor) % 5 + 10;
    if (floor <= 8) return (MapSeed + floor) % 4 + 12;
    if (floor <= 12) return (MapSeed + floor) % 3 + 14;
    return 16;
  }

  public void GenerateTreasureMap(int rankBase, uint seed) {
    var rank = rankBase + seed.GetRand(rankBase / 10, rankBase / 10 * 2).Clamp(1, 248);
    var mapSeed = seed.Advance();
    var worldMapPos = 
      rank <= 50 ? seed.GetRand(1, 47) :
      rank <= 80 ? seed.GetRand(1, 131) :
      seed.GetRand(1, 150);
  }


}

static class TreasureMapData {
  public static string[] TreasureMapName1 = new string[16] {
    "はかなき",
    "ちいさな",
    "うす暗き",
    "ゆらめく",
    "ざわめく",
    "ねむれる",
    "怒れる",
    "呪われし",
    "放たれし",
    "けだかき",
    "わななく",
    "残された",
    "見えざる",
    "あらぶる",
    "とどろく",
    "大いなる"
  };

  public static string[] TreasureMapName2 = new string[16] {
    "花の",
    "岩の",
    "風の",
    "空の",
    "獣の",
    "夢の",
    "影の",
    "大地の",
    "運命の",
    "魂の",
    "闇の",
    "光の",
    "魔神の",
    "星々の",
    "悪霊の",
    "神々の"
  }

  public static string[] TreasureMapName3 = new string[][]
  {
    // 0,1,2,3,4,5,6,7
    // 自然タイプ
    new string[7] { "洞くつ", "坑道", "アジト", "道", "墓場", "巣", "世界", "奈落" },
    // 8,1,2,3,4,9,6,10
    // 遺跡タイプ
    new string[7] { "地下道", "坑道", "アジト", "道", "墓場", "遺跡", "世界", "迷宮" },
    // 0,11,12,13,4,14,6,15
    // 氷タイプ
    new string[7] { "洞くつ", "雪道", "氷穴", "雪原", "墓場", "凍土", "世界", "氷河" },
    // 0,16,17,18,19,6,20,
    // 水タイプ
    new string[7] { "洞くつ", "沼地", "地底湖", "湿原", "墓場", "水脈", "世界", "眠る地" },
    // 0,1,21,22,4,5,6,23
    // 火山タイプ
    new string[7] { "洞くつ", "坑道", "火口", "牢ごく", "墓場", "巣", "世界", "じごく" },
  };

}
