﻿using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Text;
using Xamarin.Forms.CustomAttributes;
using Xamarin.Forms.Internals;

#if UITEST
using Xamarin.Forms.Core.UITests;
using Xamarin.UITest;
using NUnit.Framework;
#endif

namespace Xamarin.Forms.Controls.Issues
{
#if UITEST
	[Category(UITestCategories.ListView)]
#endif

	[Preserve(AllMembers = true)]
	[Issue(IssueTracker.Bugzilla, 40704, "Strange duplication of listview headers when collapsing/expanding sections")]
	public class Bugzilla40704 : TestContentPage // or TestMasterDetailPage, etc ...
	{
		ListView listview;
		int count = 2;

		protected override void Init()
		{
			listview = new ListView(ListViewCachingStrategy.RecycleElement)
			{
				AutomationId = "lstMain",
				IsGroupingEnabled = true,
				HasUnevenRows = true,
				GroupHeaderTemplate = new DataTemplate(typeof(GroupHeaderViewCell)),
				ItemTemplate = new DataTemplate(typeof(ItemTestViewCell))
			};

			FillPatientsList();

			var button = new Button()
			{
				Text = "Collapse",
				AutomationId = "btnCollappse"
			};
			listview.Footer = button;
			button.Clicked += Button_Clicked;
			Content = listview;
		}

		void Button_Clicked(object sender, EventArgs e)
		{
			var source = listview.ItemsSource as List<PatientsGroupViewModel>;
			source[count].Toggle();
			count--;
			if (count < 0)
				count = 2;
		}

		private void FillPatientsList()
		{
			const int groupsNumber = 3;
			const int patientsNumber = 10;

			var patientGroups = new List<PatientsGroupViewModel>();
			var random = new Random();

			for (var i = 0; i < groupsNumber; i++)
			{
				var patients = new List<PatientViewModel>();
				for (var j = 0; j < patientsNumber; j++)
				{
					string code = string.Format("{0}-{1}", i, j);
					int length = random.Next(5, 100);
					var strBuilder = new StringBuilder();
					for (var z = 0; z < length; z++)
					{
						strBuilder.Append(code);
						if (z % 7 == 0)
						{
							strBuilder.Append(' ');
						}
					}

					patients.Add(new PatientViewModel(code) { Description = strBuilder.ToString() });
				}

				patientGroups.Add(new PatientsGroupViewModel(patients)
				{
					Title = "Menu - " + i.ToString()
				});
			}

			listview.ItemsSource = patientGroups;
		}

		[Preserve(AllMembers = true)]
		public class GroupHeaderViewCell : ViewCell
		{
			TapGestureRecognizer tapGesture;

			public GroupHeaderViewCell()
			{
				Height = 40;
				var grd = new Grid { BackgroundColor = Color.Aqua, Padding = new Thickness(5, 10) };
				tapGesture = new TapGestureRecognizer();
				tapGesture.Tapped += HeaderCell_OnTapped;
				grd.GestureRecognizers.Add(tapGesture);
				var lbl = new Label
				{
					VerticalOptions = LayoutOptions.Center,
					HorizontalOptions = LayoutOptions.FillAndExpand,
					TextColor = Color.Black,
					FontSize = 16
				};
				lbl.SetBinding(Label.TextProperty, new Binding("Title"));

				grd.Children.Add(lbl);
				View = grd;
			}

			void HeaderCell_OnTapped(object sender, EventArgs e)
			{
				var cell = (Layout)sender;
				var vm = cell.BindingContext as PatientsGroupViewModel;

				if (vm != null)
				{
					vm.Toggle();
				}
			}
		}

		[Preserve(AllMembers = true)]
		public class ItemTestViewCell : ViewCell
		{
			public ItemTestViewCell()
			{
				var grd = new Grid { BackgroundColor = Color.Yellow };
				var lbl = new Label
				{
					HorizontalOptions = LayoutOptions.FillAndExpand,
					TextColor = Color.Black,
					FontSize = 16,
					LineBreakMode = LineBreakMode.WordWrap
				};
				lbl.SetBinding(Label.TextProperty, new Binding("Description"));
				grd.Children.Add(lbl);
				View = grd;
			}
		}

		[Preserve(AllMembers = true)]
		public class RangeObservableCollection<T> : ObservableCollection<T>
		{
			private bool _suppressNotification = false;

			public void AddRange(IEnumerable<T> list)
			{
				if (list == null)
					throw new ArgumentNullException("list");

				_suppressNotification = true;

				foreach (T item in list)
				{
					Add(item);
				}
				_suppressNotification = false;
				OnCollectionChanged(new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Reset));
			}

			protected override void OnCollectionChanged(NotifyCollectionChangedEventArgs e)
			{
				if (!_suppressNotification)
					base.OnCollectionChanged(e);
			}
		}

		[Preserve(AllMembers = true)]
		public class PatientsGroupViewModel : RangeObservableCollection<PatientViewModel>
		{
			private readonly List<PatientViewModel> _patients;

			public PatientsGroupViewModel(List<PatientViewModel> patients)
			{
				_patients = patients;

				UpdateCollection();
			}

			public bool IsCollapsed { get; private set; }

			public string Title { get; set; }

			public void Toggle()
			{
				IsCollapsed = !IsCollapsed;

				UpdateCollection();
			}

			private void UpdateCollection()
			{
				if (!IsCollapsed)
				{
					AddRange(_patients);
				}
				else
				{
					Clear();
				}
			}
		}

		[Preserve(AllMembers = true)]
		public class PatientViewModel
		{
			public PatientViewModel(string code)
			{
				Code = code;
			}

			public string Code { get; set; }

			public string Description { get; set; }
		}

#if UITEST
		[Test]
		public void Bugzilla40704HeaderPresentTest()
		{
			RunningApp.WaitForElement("Menu - 0");
		}
		[Test]
		public void Bugzilla40704Test()
		{
			RunningApp.ScrollDownTo("btnCollappse", "lstMain", ScrollStrategy.Gesture, 0.8, timeout: TimeSpan.FromMinutes(2));
			RunningApp.Tap("btnCollappse");
			RunningApp.ScrollDownTo("btnCollappse", "lstMain", ScrollStrategy.Gesture, 0.8, timeout: TimeSpan.FromMinutes(2));
			RunningApp.Tap("btnCollappse");
			RunningApp.ScrollDownTo("btnCollappse", "lstMain", ScrollStrategy.Gesture, 0.8, timeout: TimeSpan.FromMinutes(2));
			RunningApp.Tap("btnCollappse");
			RunningApp.WaitForElement("Menu - 2");
			RunningApp.WaitForElement("Menu - 1");
			RunningApp.WaitForElement("Menu - 0");
		}
#endif
	}
}