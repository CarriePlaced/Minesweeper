using System;
using System.Collections.Generic;
using MonoTouch.UIKit;
using MonoTouch.Foundation;
using MonoTouch.ObjCRuntime;

namespace Minesweeper
{
	public class GameGridView : UICollectionViewController
    {
		static NSString tileCellId = new NSString ("TileCell");
		static NSString headerId = new NSString ("HeaderView");
		static int row = 9;
		static int col = 6;
		static int mineCount = 15;

		public GameGridView (UICollectionViewLayout layout) : base (layout)
        {
			_tileGrid = new TileGrid (row, col, mineCount); 
        }

        public override void ViewDidLoad ()
        {
            base.ViewDidLoad ();
			CollectionView.BackgroundColor = UIColor.LightGray;
            CollectionView.RegisterClassForCell (typeof(TileCellView), tileCellId);
			CollectionView.RegisterClassForSupplementaryView (typeof(HeaderView), UICollectionElementKindSection.Header, headerId);
		}

        public override int NumberOfSections (UICollectionView collectionView)
        {
            return 1;
        }

        public override int GetItemsCount (UICollectionView collectionView, int section)
        {
			return _tileGrid.Grid.Length;
        }

        public override UICollectionViewCell GetCell (UICollectionView collectionView, MonoTouch.Foundation.NSIndexPath indexPath)
        {
			var tileCell = (TileCellView)collectionView.DequeueReusableCell (tileCellId, indexPath);
			tileCell.Tile = _tileGrid.Grid[indexPath.Row];
			tileCell.Layer.BorderWidth = 0;
			tileCell.Update ();
			return tileCell;
        }

		public override void ItemSelected (UICollectionView collectionView, NSIndexPath indexPath)
		{
			var tile = (TileCellView)collectionView.CellForItem (indexPath);
			var updateTiles = _tileGrid.Click (tile.Tile);
			tile.Update ();
			tile.Layer.BorderColor = tile.Tile.IsPlayerRed? UIColor.Red.CGColor:UIColor.Blue.CGColor;
			tile.Layer.BorderWidth = 4.0f;

			if (updateTiles != null) 
			{
				foreach (var item in updateTiles) {
					var updateTile = (TileCellView)CollectionView.CellForItem (NSIndexPath.FromRowSection(item, 0));
					updateTile.Update ();
				}
			}

			CheckWinning ();
			_headerView.Update ();
		}
			
		public override void ItemDeselected (UICollectionView collectionView, NSIndexPath indexPath)
		{
			var tile = (TileCellView)collectionView.CellForItem (indexPath);
			tile.Layer.BorderWidth = 0;
		}
			
        public override UICollectionReusableView GetViewForSupplementaryElement (UICollectionView collectionView, NSString elementKind, NSIndexPath indexPath)
        {
			_headerView = (HeaderView)collectionView.DequeueReusableSupplementaryView (elementKind, headerId, indexPath);
			_headerView.TileGrid = _tileGrid;
			return _headerView;
		}

		private void CheckWinning()
		{
			var alert = new UIAlertView ();
			alert.AddButton ("Restart");
			alert.Clicked += delegate {
				_tileGrid = new TileGrid (row, col, mineCount);
				CollectionView.ReloadData();
				_headerView.TileGrid = _tileGrid;
				_headerView.Update();
			};

			if (_tileGrid.PlayerRed > mineCount /2 ) 
			{
				alert.Title = "Red Won!!!";
				alert.Show ();;
			}

			if (_tileGrid.PlayerBlue > mineCount /2) 
			{
				alert.Title = "Blue Won!!!";
				alert.Show ();
			}

			if (_tileGrid.PlayerBlue == mineCount /2 && _tileGrid.PlayerRed == mineCount /2 ) 
			{
				alert.Title = "Tie!!!";
				alert.Show ();
			}

		}

		HeaderView	_headerView;
		TileGrid _tileGrid;
    }
}

