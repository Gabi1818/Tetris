using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tetris
{
    internal class OBlock : Block
    {
        public override int Id => 4;

        public override int[,] Rotation0 => _rotation0;
        public override int[,] Rotation1 => _rotation0;
        public override int[,] Rotation2 => _rotation0;
        public override int[,] Rotation3 => _rotation0;

        public override int Rows => 2;
        public override int Columns => 2;

        public override CellInGridState Cell => CellInGridState.OBlock;


        private int[,] _rotation0 = new int[,]
        {
            { 0, 0 },
            { 0, 1 },
            { 1, 0 },
            { 1, 1 }
        };

        public OBlock(int[] position) : base(position)
        {
        }
    }
}
