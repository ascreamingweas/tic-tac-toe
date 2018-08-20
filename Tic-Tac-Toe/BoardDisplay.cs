namespace Tic_Tac_Toe
{
    public class BoardDisplay
    {
        public class Row
        {
            private string preRow = "", row = "", postRow = "", lineRow = "";
            
            public Row() {}

            public Row(string preRow, string row, string postRow, string lineRow)
            {
                this.preRow = preRow;
                this.row = row;
                this.postRow = postRow;
                this.lineRow = lineRow;
            }
            
            public string GetPreRow()
            {
                return preRow;
            }

            public void SetPreRow(string preRow)
            {
                this.preRow = preRow;
            }
        
            public string GetRow()
            {
                return row;
            }

            public void SetRow(string row)
            {
                this.row = row;
            }
        
            public string GetPostRow()
            {
                return postRow;
            }

            public void SetPostRow(string postRow)
            {
                this.postRow = postRow;
            }
        
            public string GetLineRow()
            {
                return lineRow;
            }

            public void SetLineRow(string lineRow)
            {
                this.lineRow = lineRow;
            }
        }
        
        // constants for board display spacing
        public readonly string SHORT = "  ";
        public readonly string LONG = "     ";
        public readonly string SPLIT = "-----";
        
        private Row[] rows;
        
        public BoardDisplay() {}

        public BoardDisplay(int numRows)
        {
            this.rows = new Row[numRows];
        }

        public int GetNumRows()
        {
            return rows.Length;
        }

        public Row GetRowByIndex(int index)
        {
            return rows[index];
        }

        public void SetRowByIndex(int index, Row row)
        {
            rows[index] = row;
        }
    }
}