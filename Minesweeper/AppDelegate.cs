using System;
using System.Collections.Generic;
using System.Linq;
using MonoTouch.Foundation;
using MonoTouch.UIKit;

namespace Minesweeper
{
	[Register ("AppDelegate")]
	public partial class AppDelegate : UIApplicationDelegate
	{
		public override bool FinishedLaunching (UIApplication app, NSDictionary options)
		{
			_window = new UIWindow (UIScreen.MainScreen.Bounds);

			_flowLayout = new UICollectionViewFlowLayout (){
				HeaderReferenceSize = new System.Drawing.SizeF (100, 55),
				ScrollDirection = UICollectionViewScrollDirection.Vertical,
				MinimumInteritemSpacing = -4, 
				MinimumLineSpacing = 0 
			};
					

			_gameGridView = new GameGridView (_flowLayout);
			_gameGridView.CollectionView.ContentInset = new UIEdgeInsets (30, 5, 5, 5);

			_window.RootViewController = _gameGridView;
			_window.MakeKeyAndVisible ();

			return true;
		}

		UIWindow _window;
		UICollectionViewController _gameGridView;
		UICollectionViewFlowLayout _flowLayout;
	}
}

