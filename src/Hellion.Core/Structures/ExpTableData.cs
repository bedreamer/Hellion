namespace Hellion.Core.Structures
{
    public struct ExpTableData
    {
        public int Level { get; private set; }

        public long Exp { get; private set; }

        public long Pxp { get; private set; }

        public long Gp { get; private set; }

        public long LimExp { get; private set; }

        public ExpTableData(int level, string[] tableData)
        {
            this.Level = level;
            this.Exp = long.Parse(tableData[0]);
            this.Pxp = long.Parse(tableData[1]);
            this.Gp = long.Parse(tableData[2]);
            this.LimExp = long.Parse(tableData[3]);
        }
    }
}
