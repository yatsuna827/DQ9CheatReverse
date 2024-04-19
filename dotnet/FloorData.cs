using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DQ9TreasureMap
{
    class FloorData
    {
        public int aIndex = 0;
        public int bIndex = 0;

        public int routineFIndex = 0;

        public byte[,] FloorMap = new byte[16, 16];

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

}
