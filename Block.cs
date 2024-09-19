using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tetris
{
    public abstract class Block
    {
        public abstract int Id { get; }
        public int[,] CurrentRotation
        {
            get
            {
                if (_rotationState == 0)
                    return Rotation0;
                else if (_rotationState == 1)
                    return Rotation1;
                else if (_rotationState == 2)
                    return Rotation2;
                else if (_rotationState == 3)
                    return Rotation3;
                else
                    throw new Exception();
            }
        }

        public abstract int[,] Rotation0 { get; }
        public abstract int[,] Rotation1 { get; }
        public abstract int[,] Rotation2 { get; }
        public abstract int[,] Rotation3 { get; }

        public int[] Position { get; set; }

        public abstract int Rows { get; }
        public abstract int Columns { get; }

        private int _rotationState;

        public abstract CellInGridState Cell { get; }

        public Block(int[] position)
        {
            _rotationState = 0;
            Position = position;  
        }

        public void Rotate()
        {
            if(_rotationState < 3)
            {
                _rotationState++;
            }
            else
            {
                _rotationState = 0;
            }
        }


    }
}
