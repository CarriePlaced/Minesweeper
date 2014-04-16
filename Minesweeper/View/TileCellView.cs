using System;
using MonoTouch.UIKit;
using MonoTouch.Foundation;

namespace Minesweeper
{
	public class TileCellView : UICollectionViewCell
	{
		[Export ("initWithFrame:")]
		public TileCellView (System.Drawing.RectangleF frame) : base (frame)
		{
			var imageView = new UIImageView (UIImage.FromBundle ("Icons/notDiscovered.png"));
			imageView.Center = ContentView.Center;
			BackgroundView = imageView;
		}

		public void Update()
		{
			var imageView = new UIImageView (UIImage.FromBundle (Tile.FilePath));
			imageView.Center = ContentView.Center;
			BackgroundView = imageView;
		}
			
		public Tile Tile { set; get; }
	}
}

