using System;
using MonoTouch.AudioToolbox;
using System.Collections.Generic;

namespace Minesweeper
{
    public class TileGrid
    {
        public TileGrid(int row, int col, int mineCount)
        {
            _row = row;
            _col = col;
			MineCount = mineCount;
        }

		public bool IsPlayerRed { get; set;}
		public int PlayerRed{ get; set; }
		public int PlayerBlue{ get; set; }
		public int MineCount { get; set; }

        public Tile[] Grid
        {
            get
            {
                if (_tileArray == null)
                {
                    // initialize
                    _tileArray = new Tile[_row * _col];
                    for (int i = 0; i < _row; i++)
                    {
                        for (int j = 0; j < _col; j++)
                        {
							_tileArray[j + i * _col] = new Tile(i, j);
                        }
                    }

                    // generate mines
                    Random random = new Random();
					int counter = MineCount;
                    while (counter > 0)
                    {
						int index = random.Next(_row * _col);
                        if (!_tileArray[index].IsMine)
                        {
                            _tileArray[index].Mines = -1;
                            _tileArray[index].IsMine = true;
                            counter--;
                        }
                    }

                    // count mines
                    for (int i = 0; i < _row; i++)
                    {
                        for (int j = 0; j < _col; j++)
                        {
							if (!_tileArray[j + i * _col].IsMine)
                            {
                                for (int x = i - 1; x <= i + 1; x++)
                                {
                                    for (int y = j - 1; y <= j + 1; y++)
                                    {
										if (x >= 0 && y >= 0 && x < _row && y < _col && _tileArray[y + x * _col].IsMine)
                                        {
											_tileArray[j + i * _col].Mines++;
                                        }
                                    }
                                }
                            }
                        }
                    }
                }

                return _tileArray;
            }
        }

		public void  UpdateSurrounding(Tile tile, List<int> updateTiles)
        {
            if (tile.Mines == 0)
            {
                for (int x = tile.X - 1; x <= tile.X + 1 &&  x <_row ; x++)
                {
                    for (int y = tile.Y - 1; y <= tile.Y + 1 && y < _col; y++)
                    {
						if (x >= 0 && y >= 0 && _tileArray[y + x * _col] != tile)
                        {
							if (_tileArray[y + x * _col].Mines == 0 && !_tileArray[y + x * _col].State)
                            {
								_tileArray[y + x * _col].State = true;
								UpdateSurrounding(_tileArray[y + x * _col], updateTiles);
                            }
							_tileArray[y + x * _col].State = true;
							updateTiles.Add (y + x * _col);
                        }
                    }
                }
            }
        }

		public List<int> Click(Tile tile)
		{
			List <int> updateTile = new List<int>();
			if (tile.State != true)
			{
				tile.State = true;
				tile.IsPlayerRed = IsPlayerRed;
				UpdateSurrounding(tile, updateTile);

				if (tile.IsMine)
				{
					if (IsPlayerRed)
					{
						PlayerRed++;
					}
					else
					{
						PlayerBlue++;
					}
				
					SystemSound.FromFile ("Mine.wav").PlaySystemSound ();
					MineCount--;
				}
				else
				{
					SystemSound.FromFile ("Number.wav").PlaySystemSound ();
					IsPlayerRed = !IsPlayerRed;
				}
			}
			return updateTile;
		}

        private readonly int _row;
        private readonly int _col;
        private Tile[] _tileArray;
    }
}
