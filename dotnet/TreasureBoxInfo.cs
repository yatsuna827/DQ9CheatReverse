using System;

namespace DQ9TreasureMap
{
    // Token: 0x0200004F RID: 79
    public class TreasureBoxInfo
    {
        // Token: 0x170000AF RID: 175
        // (get) Token: 0x0600034F RID: 847 RVA: 0x00021818 File Offset: 0x0001FA18
        public int Index
        {
            get
            {
                return this._index;
            }
        }

        // Token: 0x170000B0 RID: 176
        // (get) Token: 0x06000350 RID: 848 RVA: 0x00021820 File Offset: 0x0001FA20
        public int Rank
        {
            get
            {
                return this._rank;
            }
        }

        // Token: 0x170000B1 RID: 177
        // (get) Token: 0x06000351 RID: 849 RVA: 0x00021828 File Offset: 0x0001FA28
        public int X
        {
            get
            {
                return this._x;
            }
        }

        // Token: 0x170000B2 RID: 178
        // (get) Token: 0x06000352 RID: 850 RVA: 0x00021830 File Offset: 0x0001FA30
        public int Y
        {
            get
            {
                return this._y;
            }
        }

        // Token: 0x06000353 RID: 851 RVA: 0x00021838 File Offset: 0x0001FA38
        public TreasureBoxInfo(int index, int rank, int x, int y)
        {
            this._index = index;
            this._rank = rank;
            this._x = x;
            this._y = y;
        }

        // Token: 0x0400027D RID: 637
        private int _index;

        // Token: 0x0400027E RID: 638
        private int _rank;

        // Token: 0x0400027F RID: 639
        private int _x;

        // Token: 0x04000280 RID: 640
        private int _y;
    }
}
