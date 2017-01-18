using System;
using System.Collections.Generic;
using System.Linq;
using Xamarin.Forms.Internals;

namespace Xamarin.Forms.Controls
{
	[Preserve(AllMembers = true)]
	public class CellForceUpdateSizeGalleryPage : TabbedPage
	{
		public CellForceUpdateSizeGalleryPage()
		{
			Children.Add(new ViewCellPage());
			Children.Add(new ImageCellPage());
			Children.Add(new TextCellPage());
			Children.Add(new EntryCellPage());
			Children.Add(new SwitchCellPage());
		}

		public class ViewCellPage : ContentPage
		{
			public ViewCellPage()
			{
				var listview = new ListView
				{
					HasUnevenRows = true,
				};
				IEnumerable<int> items = Enumerable.Range(0, 10);
				listview.ItemsSource = items;
				listview.ItemTemplate = new DataTemplate(typeof(MyViewCell));
				Content = listview;
				Title = "View Cell";
			}

			[Preserve(AllMembers = true)]
			public class MyViewCell : ViewCell
			{
				public MyViewCell()
				{
					var image = new Image
					{
						Source = ImageSource.FromFile("crimson.jpg"),
						BackgroundColor = Color.Gray,
						HeightRequest = 50,
						VerticalOptions = LayoutOptions.Fill,
						HorizontalOptions = LayoutOptions.Fill
					};

					var button = new Button { Text = "+" };
					button.Clicked += (object sender, EventArgs e) =>
					{
						image.HeightRequest = image.Height + 100;
						ForceUpdateSize();
					};

					Tapped += (object sender, EventArgs e) =>
					{
						image.HeightRequest = image.Height - 100;
						ForceUpdateSize();
					};

					View = new StackLayout { Orientation = StackOrientation.Horizontal, Children = { image, button } };
				}
			}
		}

		public class ImageCellPage : ContentPage
		{
			public ImageCellPage()
			{
				var listview = new ListView
				{
					HasUnevenRows = true,
				};
				IEnumerable<int> items = Enumerable.Range(0, 10);
				listview.ItemsSource = items;
				listview.ItemTemplate = new DataTemplate(typeof(MyImageCell));
				Content = listview;
				Title = "Image Cell";
			}

			[Preserve(AllMembers = true)]
			public class MyImageCell : ImageCell
			{
				public MyImageCell()
				{
					ImageSource = ImageSource.FromFile("crimson.jpg");
					Height = 20;
					Command = new Command(() =>
					{
						Height += 20;
						ForceUpdateSize();
					});
				}
			}
		}

		public class TextCellPage : ContentPage
		{
			public TextCellPage()
			{
				var listview = new ListView
				{
					HasUnevenRows = true,
				};
				IEnumerable<int> items = Enumerable.Range(0, 10);
				listview.ItemsSource = items;
				listview.ItemTemplate = new DataTemplate(typeof(MyTextCell));
				Content = listview;
				Title = "Text Cell";
			}

			[Preserve(AllMembers = true)]
			public class MyTextCell : TextCell
			{
				public MyTextCell()
				{
					Text = "I am a TextCell, short and stout.";
					Height = 20;
					Command = new Command(() =>
					{
						Height += 20;
						ForceUpdateSize();
					});
				}
			}
		}

		public class EntryCellPage : ContentPage
		{
			public EntryCellPage()
			{
				var listview = new ListView
				{
					HasUnevenRows = true,
				};
				IEnumerable<int> items = Enumerable.Range(0, 10);
				listview.ItemsSource = items;
				listview.ItemTemplate = new DataTemplate(typeof(MyEntryCell));
				Content = listview;
				Title = "Entry Cell";
			}

			[Preserve(AllMembers = true)]
			public class MyEntryCell : EntryCell
			{
				public MyEntryCell()
				{
					Text = "I am an EntryCell, short and stout.";
					Height = 20;
					Tapped += (object sender, EventArgs e) =>
					{
						Height += 20;
						ForceUpdateSize();
					};
					Completed += (object sender, EventArgs e) =>
					{
						Height -= 20;
						ForceUpdateSize();
					};
				}
			}
		}

		public class SwitchCellPage : ContentPage
		{
			public SwitchCellPage()
			{
				var listview = new ListView
				{
					HasUnevenRows = true,
				};
				IEnumerable<int> items = Enumerable.Range(0, 10);
				listview.ItemsSource = items;
				listview.ItemTemplate = new DataTemplate(typeof(MySwitchCell));
				Content = listview;
				Title = "Switch Cell";
			}

			[Preserve(AllMembers = true)]
			public class MySwitchCell : SwitchCell
			{
				public MySwitchCell()
				{
					Text = "I am a SwitchCell, short and stout.";
					Height = 20;
					Tapped += (object sender, EventArgs e) =>
					{
						Height += 20;
						ForceUpdateSize();
					};
					OnChanged += (object sender, ToggledEventArgs e) =>
					{
						Height -= 20;
						ForceUpdateSize();
					};
				}
			}
		}
	}
}