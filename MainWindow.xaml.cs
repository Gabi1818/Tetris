using System.IO;
using System.Media;
using System.Reflection.Metadata.Ecma335;
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
using System.Windows.Threading;

namespace Tetris
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    /// 

    enum GamePhase { Start, Play, End };
    public partial class MainWindow : Window
    {
        private ImageSource _tileEmpty = new BitmapImage(new Uri("Assets/TileEmpty.png", UriKind.Relative));
        private ImageSource _tileCyan = new BitmapImage(new Uri("Assets/TileCyan.png", UriKind.Relative));
        private ImageSource _tileBlue = new BitmapImage(new Uri("Assets/TileBlue.png", UriKind.Relative));
        private ImageSource _tileOrange = new BitmapImage(new Uri("Assets/TileOrange.png", UriKind.Relative));
        private ImageSource _tileYellow = new BitmapImage(new Uri("Assets/TileYellow.png", UriKind.Relative));
        private ImageSource _tileGreen = new BitmapImage(new Uri("Assets/TileGreen.png", UriKind.Relative));
        private ImageSource _tilePurple = new BitmapImage(new Uri("Assets/TilePurple.png", UriKind.Relative));
        private ImageSource _tileRed = new BitmapImage(new Uri("Assets/TileRed.png", UriKind.Relative));

        private ImageSource _imgBlockEmpty = new BitmapImage(new Uri("Assets/Block-Empty.png", UriKind.Relative));
        private ImageSource _imgBlockI = new BitmapImage(new Uri("Assets/Block-I.png", UriKind.Relative));
        private ImageSource _imgBlockJ = new BitmapImage(new Uri("Assets/Block-J.png", UriKind.Relative));
        private ImageSource _imgBlockL = new BitmapImage(new Uri("Assets/Block-L.png", UriKind.Relative));
        private ImageSource _imgBlockO = new BitmapImage(new Uri("Assets/Block-O.png", UriKind.Relative));
        private ImageSource _imgBlockS = new BitmapImage(new Uri("Assets/Block-S.png", UriKind.Relative));
        private ImageSource _imgBlockT = new BitmapImage(new Uri("Assets/Block-T.png", UriKind.Relative));
        private ImageSource _imgBlockZ = new BitmapImage(new Uri("Assets/Block-Z.png", UriKind.Relative));


        private GamePhase gamePhase;
        Random rnd = new Random();
        GameGrid gameGrid = new GameGrid(20, 10);
        Block currentBlock;
        int nextBlock;
        int holdBlock;
        ImageSource currentBlockImageTile;
        bool canHold = true;
        int[] numberOfBlock = new int[8];
        int numberOfLines;
        int highestScore;

        DispatcherTimer timer = new DispatcherTimer();
        int timerValue;

        

        public MainWindow()
        {
            InitializeComponent();
            gamePhase = GamePhase.Start;

            timerValue = 1200;
            timer.Interval = TimeSpan.FromMilliseconds(timerValue); 
            timer.Tick += Timer_Tick;
        }

        private void Timer_Tick(object sender, EventArgs e)
        {
            MoveBlockDown();
        }

        private void BackgroundMusic_MediaEnded(object sender, RoutedEventArgs e)
        {
            // Rewind the audio and play again
            BackgroundMusic.LoadedBehavior = MediaState.Manual;
            BackgroundMusic.Position = TimeSpan.Zero;
            BackgroundMusic.Play();
        }

        private void Window_KeyDown(object sender, KeyEventArgs e)
        {
            if (gamePhase == GamePhase.Play)
            {
                switch (e.Key)
                {
                    case Key.Left:
                        MoveBlockLeft();    
                        break;

                    case Key.Right:
                        MoveBlockRight();
                        break;

                    case Key.Down:
                        MoveBlockDown();
                        break;

                    case Key.NumPad0:
                        HoldBlock();
                        break;

                    case Key.Space:
                        RotateBlock();
                        break;

                    default:
                        break;
                }
            }
        }

        private void NextGamePhase()
        {
            switch (gamePhase)
            {
                case GamePhase.Start:
                    LoadHighestScore();

                    InitializeCanvas();
                    SetBlockStatisticsImages();
                    EndGrid.Visibility = Visibility.Hidden;
                    StartGrid.Visibility = Visibility.Hidden;
                    GameGrid.Visibility = Visibility.Visible;
                    gamePhase = GamePhase.Play;

                    //BackgroundMusic.LoadedBehavior = MediaState.Manual;
                    //BackgroundMusic.Play();

                    NextGamePhase();
                    break;

                case GamePhase.Play:
                    timer.Start();

                    MakeNewBlock([0,0]);
                    DrawBlock();
                    break;

                case GamePhase.End:
                    timer.Stop();

                    GameGrid.Visibility = Visibility.Hidden;
                    EndGrid.Visibility = Visibility.Visible;
                    EndLinesTxtBlock.Text = "Score: " + numberOfLines.ToString();                  
                    NumberOfBlocksTxtBlock.Text = "Blocks: " + numberOfBlock[0].ToString();
                    EndHighestScoreTxtBlock.Text = "Highest Score: " + highestScore.ToString();
                    CheckHighestScore();

                    break;

                default:
                    break;
            }

        }

        private void InitializeCanvas()
        {
            GameCanvas.Children.Clear();

            timerValue = 1200;
            timer.Interval = TimeSpan.FromMilliseconds(timerValue);

            numberOfLines = 0;
            holdBlock = 0;

            for (int i = 0; i < numberOfBlock.Length; i++)
            {
                numberOfBlock[i] = 0;
            }

            BlockIStatisticsTxtBlock.Text = numberOfBlock[1].ToString();
            BlockJStatisticsTxtBlock.Text = numberOfBlock[2].ToString();
            BlockLStatisticsTxtBlock.Text = numberOfBlock[3].ToString();
            BlockOStatisticsTxtBlock.Text = numberOfBlock[4].ToString();
            BlockSStatisticsTxtBlock.Text = numberOfBlock[5].ToString();
            BlockTStatisticsTxtBlock.Text = numberOfBlock[6].ToString();
            BlockZStatisticsTxtBlock.Text = numberOfBlock[7].ToString();

            LinesTxtBlock.Text = "Score: " + numberOfLines.ToString();
            HighestScoreTxtBlock.Text = "Highest Score: " + highestScore.ToString();


            double cellWidth = GameCanvas.Width / gameGrid.Columns;
            double cellHeight = GameCanvas.Height / gameGrid.Rows;

            for (int row = 0; row < gameGrid.Rows; row++)
            {
                for (int col = 0; col < gameGrid.Columns; col++)
                {
                    Image cellImage = new Image
                    {
                        Source = _tileEmpty,
                        Width = cellWidth,
                        Height = cellHeight
                    };

                    Canvas.SetLeft(cellImage, col * cellWidth);
                    Canvas.SetTop(cellImage, row * cellHeight);

                    GameCanvas.Children.Add(cellImage);
                    gameGrid.GridImages[row, col] = cellImage;
                    gameGrid.Grid[row, col] = CellInGridState.Empty;
                }
            }
        }

        private void SetBlockStatisticsImages()
        {
            BlockIStatisticsIMG.Source = _imgBlockI;
            BlockJStatisticsIMG.Source = _imgBlockJ;
            BlockLStatisticsIMG.Source = _imgBlockL;
            BlockOStatisticsIMG.Source = _imgBlockO;
            BlockSStatisticsIMG.Source = _imgBlockS;
            BlockTStatisticsIMG.Source = _imgBlockT;
            BlockZStatisticsIMG.Source = _imgBlockZ;

            HoldIMG.Source = _imgBlockEmpty;
            NextIMG.Source = _imgBlockEmpty;
        }

        private void StartGame_Click(object sender, RoutedEventArgs e)
        {
            gamePhase = GamePhase.Start;
            NextGamePhase();
        }

        private void DrawBlock()
        {
            int y = currentBlock.Position[0];
            int x = currentBlock.Position[1];

            int[,] rotation = currentBlock.CurrentRotation;

            for (int i = 0; i < rotation.GetLength(0); i++)
            {
                int rowOffset = rotation[i, 0];
                int colOffset = rotation[i, 1];

                if (x == 0 && y == 0 && IsOccupied(y + rowOffset, x + colOffset))
                {
                    //canot draw a block at spawnpoint => game over
                    gamePhase = GamePhase.End;
                    NextGamePhase();
                    return;
                }
            }

            double cellWidth = GameCanvas.Width / gameGrid.Columns;
            double cellHeight = GameCanvas.Height / gameGrid.Rows;

            for (int i = 0; i < rotation.GetLength(0); i++)
            {
                int rowOffset = rotation[i, 0];
                int colOffset = rotation[i, 1];

                double left = (x + colOffset) * cellWidth;
                double top = (y + rowOffset) * cellHeight;

                Image cellImage = gameGrid.GridImages[y + rowOffset, x + colOffset];

                if (!IsOccupied(y + rowOffset, x + colOffset))

                {
                    cellImage.Source = currentBlockImageTile;
                    cellImage.Width = cellWidth;
                    cellImage.Height = cellHeight;

                    Canvas.SetLeft(cellImage, left);
                    Canvas.SetTop(cellImage, top);

                    gameGrid.Grid[y + rowOffset, x + colOffset] = CellInGridState.CurrentBlock;
                }
            }

        }

        private void RemoveOneBack()
        {
            int y = currentBlock.Position[0];
            int x = currentBlock.Position[1];

            int[,] rotation = currentBlock.CurrentRotation;

            double cellWidth = GameCanvas.Width / gameGrid.Columns;
            double cellHeight = GameCanvas.Height / gameGrid.Rows;

            for (int i = 0; i < rotation.GetLength(0); i++)
            {
                int rowOffset = rotation[i, 0];
                int colOffset = rotation[i, 1];

                double left = (x + colOffset) * cellWidth;
                double top = (y + rowOffset) * cellHeight;

                Image cellImage = gameGrid.GridImages[y + rowOffset, x + colOffset];

                if (!IsOccupied(y + rowOffset, x + colOffset))
                {
                    cellImage.Source = _tileEmpty;
                    cellImage.Width = cellWidth;
                    cellImage.Height = cellHeight;

                    Canvas.SetLeft(cellImage, left);
                    Canvas.SetTop(cellImage, top);

                    gameGrid.Grid[y + rowOffset, x + colOffset] = CellInGridState.Empty;
                }
            }
        }

        public void MakeNewBlock(int[] coords, int makeThisBlock = 0)
        {
            int block;
            canHold = true;

            if (makeThisBlock == 0)
            {
                if (nextBlock == 0)
                    nextBlock = rnd.Next(1, 8);

                block = nextBlock;
            }
            else
            {
                block = makeThisBlock;
            }

            //block = 1;

            switch (block)
            {
                case 1:
                    //if (coords[0] == 0 && coords[1] == 0)
                    //    currentBlock = new IBlock([-1,0]);
                    //else
                        currentBlock = new IBlock(coords);

                    currentBlockImageTile = _tileCyan;
                    numberOfBlock[1]++;
                    BlockIStatisticsTxtBlock.Text = numberOfBlock[1].ToString();
                    break;

                case 2:
                    currentBlock = new JBlock(coords);
                    currentBlockImageTile = _tileBlue;
                    numberOfBlock[2]++;
                    BlockJStatisticsTxtBlock.Text = numberOfBlock[2].ToString();
                    break;

                case 3:
                    currentBlock = new LBlock(coords);
                    currentBlockImageTile = _tileOrange;
                    numberOfBlock[3]++;
                    BlockLStatisticsTxtBlock.Text = numberOfBlock[3].ToString();
                    break;

                case 4:
                    currentBlock = new OBlock(coords);
                    currentBlockImageTile = _tileYellow;
                    numberOfBlock[4]++;
                    BlockOStatisticsTxtBlock.Text = numberOfBlock[4].ToString();
                    break;

                case 5:
                    currentBlock = new SBlock(coords);
                    currentBlockImageTile = _tileGreen;
                    numberOfBlock[5]++;
                    BlockSStatisticsTxtBlock.Text = numberOfBlock[5].ToString();
                    break;

                case 6:
                    currentBlock = new TBlock(coords);
                    currentBlockImageTile = _tilePurple;
                    numberOfBlock[6]++;
                    BlockTStatisticsTxtBlock.Text = numberOfBlock[6].ToString();
                    break;

                case 7:
                    currentBlock = new ZBlock(coords);
                    currentBlockImageTile = _tileRed;
                    numberOfBlock[7]++;
                    BlockZStatisticsTxtBlock.Text = numberOfBlock[7].ToString();
                    break;

                default:
                    break;
            }

            if (makeThisBlock == 0)
            {
                numberOfBlock[0]++;
                nextBlock = rnd.Next(1, 8);

                switch (nextBlock)
                {
                    case 1:
                        NextIMG.Source = _imgBlockI;
                        break;

                    case 2:
                        NextIMG.Source = _imgBlockJ;
                        break;

                    case 3:
                        NextIMG.Source = _imgBlockL;
                        break;

                    case 4:
                        NextIMG.Source = _imgBlockO;
                        break;

                    case 5:
                        NextIMG.Source = _imgBlockS;
                        break;

                    case 6:
                        NextIMG.Source = _imgBlockT;
                        break;

                    case 7:
                        NextIMG.Source = _imgBlockZ;
                        break;

                    default:
                        break;
                }
            }

        }

        public void HoldBlock()
        {
            if (canHold)
            {
                if (holdBlock == 0)
                {
                    holdBlock = currentBlock.Id;
                    RemoveOneBack();

                    MakeNewBlock(currentBlock.Position);
                    DrawBlock();
                }
                else
                {
                    RemoveOneBack();
                    int num = currentBlock.Id;
                    MakeNewBlock(currentBlock.Position, holdBlock);
                    holdBlock = num;
                    DrawBlock();
                }

                switch (holdBlock)
                {
                    case 1:
                        HoldIMG.Source = _imgBlockI;
                        break;

                    case 2:
                        HoldIMG.Source = _imgBlockJ;
                        break;

                    case 3:
                        HoldIMG.Source = _imgBlockL;
                        break;

                    case 4:
                        HoldIMG.Source = _imgBlockO;
                        break;

                    case 5:
                        HoldIMG.Source = _imgBlockS;
                        break;

                    case 6:
                        HoldIMG.Source = _imgBlockT;
                        break;

                    case 7:
                        HoldIMG.Source = _imgBlockZ;
                        break;

                    default:
                        break;
                }
            }
            
            canHold = false;
        }

        public void MoveBlockLeft()
        {
            int y = currentBlock.Position[0];
            int x = currentBlock.Position[1];

            x--;

            int[,] rotation = currentBlock.CurrentRotation;


            for (int i = 0; i < rotation.GetLength(0); i++)
            {
                int rowOffset = rotation[i, 0];
                int colOffset = rotation[i, 1];

                if (x + colOffset <= -1 || IsOccupied(y + rowOffset, x + colOffset))
                {
                    return;
                }
            }

            RemoveOneBack();

            currentBlock.Position[1]--;
            DrawBlock();

        }

        public void MoveBlockRight()
        {
            int y = currentBlock.Position[0];
            int x = currentBlock.Position[1];

            x++;

            int[,] rotation = currentBlock.CurrentRotation;


            for (int i = 0; i < rotation.GetLength(0); i++)
            {
                int rowOffset = rotation[i, 0];
                int colOffset = rotation[i, 1];

                if (x + colOffset >= gameGrid.Columns || IsOccupied(y + rowOffset, x + colOffset))
                {
                    return;
                }
            }

            RemoveOneBack();

            currentBlock.Position[1]++;
            DrawBlock();
        }

        public void MoveBlockDown()
        {
            int y = currentBlock.Position[0];
            int x = currentBlock.Position[1];

            y++; 

            int[,] rotation = currentBlock.CurrentRotation;

            for (int i = 0; i < rotation.GetLength(0); i++)
            {
                int rowOffset = rotation[i, 0];
                int colOffset = rotation[i, 1];

                if (y + rowOffset >= gameGrid.Rows || IsOccupied(y + rowOffset, x + colOffset))
                {
                    for (int j = 0; j < rotation.GetLength(0); j++)
                    {
                        int offsetY = currentBlock.Position[0] + rotation[j, 0];
                        int offsetX = currentBlock.Position[1] + rotation[j, 1];
                        gameGrid.Grid[offsetY, offsetX] = currentBlock.Cell;
                    }

                    CheckAndRemoveRows();
                    MakeNewBlock([0,0]);
                    DrawBlock();
                    return;
                }
            }

            RemoveOneBack(); 
            currentBlock.Position[0]++;
            DrawBlock(); 
        }
        public void RotateBlock()
        {
            RemoveOneBack();
            currentBlock.Rotate();

            int y = currentBlock.Position[0];
            int x = currentBlock.Position[1];

            int[,] rotation = currentBlock.CurrentRotation;


            for (int i = 0; i < rotation.GetLength(0); i++)
            {
                int rowOffset = rotation[i, 0];
                int colOffset = rotation[i, 1];

                if (x + colOffset >= gameGrid.Columns || x + colOffset <= -1 || y + rowOffset >= gameGrid.Rows || IsOccupied(y + rowOffset, x + colOffset))
                {
                    currentBlock.Rotate();
                    currentBlock.Rotate();
                    currentBlock.Rotate();
                    DrawBlock();


                    return;
                }
            }

            RemoveOneBack();
            DrawBlock();
        }

        public bool IsOccupied(int x, int y)
        {
            if (gameGrid.Grid[x, y] == CellInGridState.IBlock ||
                gameGrid.Grid[x, y] == CellInGridState.JBlock ||
                gameGrid.Grid[x, y] == CellInGridState.LBlock ||
                gameGrid.Grid[x, y] == CellInGridState.OBlock ||
                gameGrid.Grid[x, y] == CellInGridState.SBlock ||
                gameGrid.Grid[x, y] == CellInGridState.TBlock ||
                gameGrid.Grid[x, y] == CellInGridState.ZBlock)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public void CheckAndRemoveRows()
        {
            bool isFullRow = true;

            for (int x = 0; x < gameGrid.Grid.GetLength(0); x++)
            {
                for (int y = 0; y < gameGrid.Grid.GetLength(1); y++)
                {
                    if (gameGrid.Grid[x,y] == CellInGridState.Empty)
                    {
                        isFullRow = false;
                        break;
                    }
                }

                if (isFullRow)
                {
                    RemoveRow(x);
                    numberOfLines++;
                    LinesTxtBlock.Text = "Score: " + numberOfLines.ToString();

                    if (timerValue >= 250)
                    {
                        timerValue -= 50;
                        timer.Interval = TimeSpan.FromMilliseconds(timerValue);
                    }
                }
                isFullRow = true;
            }
        }

        public void RemoveRow(int rowNumber)
        {
            while (rowNumber > 1)
            {
                for (int y = 0; y < gameGrid.Grid.GetLength(1); y++)
                {
                    Image cellImage = gameGrid.GridImages[rowNumber, y];
                    ImageSource tileImage = GetTileImage(rowNumber - 1, y);
                    gameGrid.Grid[rowNumber, y] = gameGrid.Grid[rowNumber - 1, y];

                    double cellWidth = GameCanvas.Width / gameGrid.Columns;
                    double cellHeight = GameCanvas.Height / gameGrid.Rows;
                    cellImage.Source = tileImage;
                    cellImage.Width = cellWidth;
                    cellImage.Height = cellHeight;
                }

                rowNumber--;
            }
        }

        public ImageSource GetTileImage(int rowNumber, int y)
        {
            CellInGridState cell = gameGrid.Grid[rowNumber, y];

            switch (cell)
            {
                case CellInGridState.Empty:
                    return _tileEmpty;
                case CellInGridState.CurrentBlock:
                    return currentBlockImageTile;
                case CellInGridState.IBlock:
                    return _tileCyan;
                case CellInGridState.JBlock:
                    return _tileBlue;
                case CellInGridState.LBlock:
                    return _tileOrange;
                case CellInGridState.OBlock:
                    return _tileYellow;
                case CellInGridState.SBlock:
                    return _tileGreen;
                case CellInGridState.TBlock:
                    return _tilePurple;
                case CellInGridState.ZBlock:
                    return _tileRed;
                default:
                    throw new Exception();
            }
        }

        public void CheckHighestScore()
        {
            if (numberOfLines > highestScore)
                highestScore = numberOfLines;

            File.WriteAllText("score.txt", highestScore.ToString());

        }

        public void LoadHighestScore()
        {
            try
            {
                string fromDisk = System.IO.File.ReadAllText("score.txt");

                highestScore = int.Parse(fromDisk);
            }
            catch
            {
                highestScore = 0;
            }
        }
    }
}