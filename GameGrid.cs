using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Tetris
{
    public enum CellInGridState { Empty, CurrentBlock, IBlock, JBlock, LBlock, OBlock, SBlock, TBlock, ZBlock }
    internal class GameGrid
    {
        private int _rows;
        private int _columns;

        public Image[,] GridImages;
        public CellInGridState[,] Grid;

        public int Rows => _rows;
        public int Columns => _columns;

        public GameGrid(int rows, int columns)
        {
            _rows = rows;
            _columns = columns;
            GridImages = new Image[_rows, _columns];
            Grid = new CellInGridState[_rows, _columns];

            for (int x = 0; x < _rows; x++)
            {
                for (int y = 0; y < _columns; y++)
                {
                    Grid[x, y] = CellInGridState.Empty;
                }
            }
        }

  
    }
}
