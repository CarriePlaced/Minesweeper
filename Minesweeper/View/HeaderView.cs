using System;
using MonoTouch.UIKit;
using MonoTouch.Foundation;

namespace Minesweeper
{
	public class HeaderView : UICollectionReusableView
	{

		public TileGrid TileGrid { set; get; }

		[Export ("initWithFrame:")]
		public HeaderView (System.Drawing.RectangleF frame) : base (frame)
		{
			_imageView = new UIImageView (UIImage.FromBundle ("Icons/BlueArrow.png")){Frame = new System.Drawing.RectangleF(0,0,70,50) };

			_label = new UILabel (){Frame = new System.Drawing.RectangleF (75,5,220,40), BackgroundColor = UIColor.Yellow};
			_label.Text = String.Format (" Red : 0  Mine: 15  Blue: 0");
			Add (_imageView);
			Add (_label);
		}

		public void Update()
		{
			_label.Text = String.Format (" Red : {0}   Mine: {1}   Blue: {2}", TileGrid.PlayerRed, TileGrid.MineCount, TileGrid.PlayerBlue);
			_imageView.Image = TileGrid.IsPlayerRed ? UIImage.FromBundle ("Icons/RedArrow.png"): UIImage.FromBundle ("Icons/BlueArrow.png");
			SetNeedsDisplay ();
		}

		UILabel _label;
		UIImageView _imageView;
	}
}

