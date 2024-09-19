﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tetris
{
    internal class SBlock : Block
    {
        public override int Id => 5;

        public override int[,] Rotation0 => _rotation0;
        public override int[,] Rotation1 => _rotation1;
        public override int[,] Rotation2 => _rotation2;
        public override int[,] Rotation3 => _rotation3;

        public override int Rows => 3;
        public override int Columns => 3;

        public override CellInGridState Cell => CellInGridState.SBlock;


        private int[,] _rotation0 = new int[,]
        {
            { 0, 1 },
            { 0, 2 },
            { 1, 0 },
            { 1, 1 }
        };

        private int[,] _rotation1 = new int[,]
        {
            { 0, 1 },
            { 1, 1 },
            { 1, 2 },
            { 2, 2 }
        };

        private int[,] _rotation2 = new int[,]
        {
            { 1, 1 },
            { 1, 2 },
            { 2, 0 },
            { 2, 1 }
        };

        private int[,] _rotation3 = new int[,]
        {
            { 0, 0 },
            { 1, 0 },
            { 1, 1 },
            { 2, 1 }
        };

        public SBlock(int[] position) : base(position)
        {
        }
    }
}