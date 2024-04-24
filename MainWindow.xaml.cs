using Microsoft.VisualBasic;
using Microsoft.Win32;
using System;
using System.Diagnostics;
using System.IO;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Markup;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using Path = System.IO.Path;

namespace EllipDraw
{
	/// <summary>
	///
	/// </summary>
	public partial class MainWindow : Window
	{
		public Canvas selCanvas;
		private Random _rnd = new Random();
		public Ellipse selEllipse;
		private int _step = 10;
		private Ellipse clipboard;

		public MainWindow()
		{
			InitializeComponent();
			selCanvas = canvas;

			// Menubar File
			MI_Exit.Click += (sender, e) => ExitApplication(null);
			MI_New.Click += (sender, e) => NewTab("New");
			MI_Open.Click += (sender, e) => OpenFile();
			MI_Save_Image.Click += (sender, e) => SaveAsImage();
			MI_Save_XAML.Click += (sender, e) => SaveToFile();

			// Menubar Edit
			MI_Clone.Click += (sender, e) => Clone(selEllipse);
			MI_Delete.Click += (sender, e) => Remove();
			MI_Add_Custom.Click += (sender, e) => AddCustom();
			MI_Add_Random.Click += (sender, e) => AddRandom();
			MI_Change.Click += (sender, e) => ChangeSelected();
			MI_Background_Color.Click += (sender, e) => ChangeBackground();
			MI_Precision.Click += (sender, e) => ChangePrecision();

			// Menubar Help
			MI_Controls.Click += (sender, e) => new Controls().ShowDialog();
			MI_About.Click += (sender, e) => new About().ShowDialog();

			// Context Menu Tab
			CM_Add_Tab.Click += (sender, e) => NewTab("New");
			CM_Delete_Tab.Click += (sender, e) => DeleteTab();
			CM_Rename_Tab.Click += (sender, e) => RenameTab();

			// Context Menu Canvas
			CM_Clone.Click += (sender, e) => Clone(selEllipse);
			CM_Delete.Click += (sender, e) => Remove();
			CM_Add_Custom.Click += (sender, e) => AddCustom();
			CM_Add_Random.Click += (sender, e) => AddRandom();
			CM_Change.Click += (sender, e) => ChangeSelected();
			CM_Background_Color.Click += (sender, e) => ChangeBackground();
			CM_Precision.Click += (sender, e) => ChangePrecision();

			// Other Events
			TC_Tabs.SelectionChanged += (sender, e) => ChangeTab();
			Closing += (sender, e) => ExitApplication(e);
		}

		private void Window_KeyDown(object sender, KeyEventArgs e)
		{
			switch (e.Key)
			{
				// Exit and Help
				case Key.Escape: ExitApplication(null); break;
				case Key.F1: new Controls().ShowDialog(); break;

				// Ellipse Actions
				case Key.G: AddCustom(); break;
				case Key.T: AddRandom(); break;

				// Clipboard
				case Key.C: clipboard = selEllipse; break;
				case Key.V: Clone(clipboard); break;
				case Key.X: clipboard = selEllipse; Remove(); break;
			}

			if (selEllipse == null) return;

			// Position and size of selected Ellipse
			double x = Canvas.GetLeft(selEllipse);
			double y = Canvas.GetTop(selEllipse);
			double w = selEllipse.Width;
			double h = selEllipse.Height;

			switch (e.Key)
			{
				// Movement
				case Key.W: y -= _step; break;
				case Key.S: y += _step; break;
				case Key.A: x -= _step; break;
				case Key.D: x += _step; break;

				// Scale
				case Key.O: w += _step; h += _step; break;
				case Key.U: w -= _step; h -= _step; break;

				// Size
				case Key.I: h += _step; break;
				case Key.K: h -= _step; break;
				case Key.J: w -= _step; break;
				case Key.L: w += _step; break;

				// Rotation
				case Key.Q: Spin(true); break;
				case Key.E: Spin(false); break;
				
				// Opacity
				case Key.Z: selEllipse.Opacity -= 0.1; break;
				case Key.H: selEllipse.Opacity += 0.1; break;

				// Actions
				case Key.R: ChangeSelected(); break;
				case Key.F: ChangePrecision(); break;
				case Key.Enter: Clone(selEllipse); break;
				case Key.Delete: Remove(); break;
				case Key.Back: Remove(); break;
			}

			// Set new position and size
			Canvas.SetLeft(selEllipse, x);
			Canvas.SetTop(selEllipse, y);
			if (w < _step) w = _step;
			if (h < _step) h = _step;
			selEllipse.Width = w;
			selEllipse.Height = h;
		}

		private SolidColorBrush GenerateColor()
		{
			return new SolidColorBrush(Color.FromRgb((byte)_rnd.Next(0, 255), (byte)_rnd.Next(0, 255), (byte)_rnd.Next(0, 255)));
		}

		
		// Selection of Ellipses
		private void Ellipse_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
		{ selEllipse = (Ellipse)sender; }


		private void ExitApplication(System.ComponentModel.CancelEventArgs? e)
		{
			MessageBoxResult result = MessageBox.Show("Are you sure you want to exit?", "Exit", MessageBoxButton.YesNo);
			if (result == MessageBoxResult.Yes) Application.Current.Shutdown();
			else if (e != null) e.Cancel = true;
		}

		// Change Background Color of Canvas
		private void ChangeBackground()
		{
			if (selCanvas == null) return;
			ColorInput input = new((selCanvas.Background as SolidColorBrush).Color);
			if (input.ShowDialog() == true && input.DialogResult == true) selCanvas.Background = new SolidColorBrush(input.SelectedColor);
		}

		// Change Precision of Movement
		private void ChangePrecision()
		{
			try {
				TextBox input = new("Enter new precision", _step.ToString());
				if (input.ShowDialog() == true && input.DialogResult == true) _step = int.Parse(input.Input);
			}
			catch (Exception) { MessageBox.Show("Invalid input"); }
		}


		// TABS

		private void NewTab(string name)
		{
			TabItem tabItem = new() { Header = name, Content = new Canvas() { Background = Brushes.LightGray , ClipToBounds=true } };
			TC_Tabs.Items.Insert(TC_Tabs.Items.Count - 1, tabItem);
		}
		private void ChangeTab()
		{
			if (TC_Tabs.SelectedIndex == TC_Tabs.Items.Count - 1)
			{
				NewTab("New");
				TC_Tabs.SelectedIndex = TC_Tabs.Items.Count - 2;
			}
			else
			{
				selCanvas = (Canvas)TC_Tabs.SelectedContent;
				selEllipse = null;
			}
		}
		private void RenameTab()
		{
			TabItem tabItem = (TabItem)TC_Tabs.SelectedItem;
			string name = tabItem.Header.ToString();
			TextBox ibox = new("Enter new name", name);
			if (ibox.ShowDialog() == true && ibox.DialogResult == true) tabItem.Header = ibox.Input;

		}
		private void DeleteTab()
		{
			MessageBoxResult result = MessageBox.Show("Are you sure you want to delete this tab?\nUnsaved Changes will be lost", "Delete Tab", MessageBoxButton.YesNo);
			if (result == MessageBoxResult.Yes)
			{
				int index = TC_Tabs.SelectedIndex;
				TC_Tabs.SelectedIndex = index - 1;
				TC_Tabs.Items.RemoveAt(index);
			}
		}



		// ELLIPSE ACTIONS
		private void ChangeSelected()
		{
			Custom custom = new(selEllipse);
			if (custom.ShowDialog() == true && custom.DialogResult == true)
			{
				selEllipse.Stroke = custom.Ell.Stroke;
				selEllipse.Fill = custom.Ell.Fill;
				selEllipse.StrokeThickness = custom.Ell.StrokeThickness;
				selEllipse.Width = custom.Ell.Width;
				selEllipse.Height = custom.Ell.Height;
			}
		}

		private void Remove()
		{
			selCanvas.Children.Remove(selEllipse);
		}
		private void AddCustom()
		{
			Custom custom = new();
			if (custom.ShowDialog() == true && custom.DialogResult == true)
			{
				selCanvas.Children.Add(custom.Ell);
				selEllipse = custom.Ell;
				selEllipse.MouseLeftButtonDown += Ellipse_MouseLeftButtonDown;
				Canvas.SetLeft(selEllipse, _step);
				Canvas.SetTop(selEllipse, _step);
			}
		}

		private void AddRandom()
		{
			Ellipse ellipse = new()
			{
				Width = _rnd.Next(10, 100),
				Height = _rnd.Next(10, 100),
				Fill = GenerateColor(),
				Stroke = GenerateColor(),
				StrokeThickness = _rnd.Next(1, 10)
			};
			Canvas.SetLeft(ellipse, _rnd.Next(0, (int)selCanvas.ActualWidth - (int)ellipse.Width));
			Canvas.SetTop(ellipse, _rnd.Next(0, (int)selCanvas.ActualHeight - (int)ellipse.Height));
			ellipse.MouseLeftButtonDown += Ellipse_MouseLeftButtonDown;
			selCanvas.Children.Add(ellipse);
		}

		private void Spin(bool left)
		{
			double curAngle = selEllipse.RenderTransform is RotateTransform ? ((RotateTransform)selEllipse.RenderTransform).Angle : 0;
			curAngle = left ?  curAngle -= _step : curAngle += _step;
			selEllipse.RenderTransform = new RotateTransform(curAngle, selEllipse.Width / 2, selEllipse.Height / 2);
		}

		private void Clone(Ellipse ell)
		{
			if (ell == null) return;
			Ellipse ellipse = new()
				{
					Width = ell.Width,
					Height = ell.Height,
					Fill = ell.Fill,
					Stroke = ell.Stroke,
					StrokeThickness = ell.StrokeThickness,
					Opacity = ell.Opacity,
					RenderTransform = ell.RenderTransform
				};
			Canvas.SetLeft(ellipse, Canvas.GetLeft(ell));
			Canvas.SetTop(ellipse, Canvas.GetTop(ell));
			ellipse.MouseLeftButtonDown += Ellipse_MouseLeftButtonDown;
			selCanvas.Children.Add(ellipse);
		}

		// LOAD AND SAVE

		// Open the SaveFileDialog to save the Canvas as XAML
		private void SaveToFile()
		{
			TabItem ti = TC_Tabs.SelectedItem as TabItem;
			string name = ti.Header.ToString();
			SaveFileDialog saveFileDialog = new();
			saveFileDialog.Filter = "XAML Files (*.xaml)|*.xaml";
			saveFileDialog.FileName = name;
			if (saveFileDialog.ShowDialog() == true)
			{
				string filePath = saveFileDialog.FileName;
				string xamlString = XamlWriter.Save(selCanvas);
				File.WriteAllText(filePath, xamlString);
			}
		}

		// Open the OpenFileDialog to load a XAML file and add it to a new Tab
		private void OpenFile()
		{
			OpenFileDialog openFileDialog = new() {  Filter = "XAML Files (*.xaml)|*.xaml" };
			if (openFileDialog.ShowDialog() == true)
			{
				string filePath = openFileDialog.FileName;
				if (File.Exists(filePath))
				{
					Canvas? loadedCanvas = XamlReader.Parse(File.ReadAllText(filePath)) as Canvas;
					string fileName = Path.GetFileNameWithoutExtension(filePath);
					NewTab(fileName);
					TC_Tabs.Items.RemoveAt(TC_Tabs.Items.Count - 2);
					TC_Tabs.Items.Insert(TC_Tabs.Items.Count - 1, new TabItem() { Header = fileName, Content = loadedCanvas });
					TC_Tabs.SelectedIndex = TC_Tabs.Items.Count - 2;
					selCanvas = (Canvas)TC_Tabs.SelectedContent;
					foreach (var item in selCanvas.Children)
					{
						if (item is Ellipse)
						{
							Ellipse ellipse = (Ellipse)item;
							ellipse.MouseLeftButtonDown += Ellipse_MouseLeftButtonDown;
						}
					}
				}
				else MessageBox.Show("File not found!");
			}
		}

		// Save the Canvas as PNG
		private void SaveAsImage()
		{
			RenderTargetBitmap renderTargetBitmap = new((int)selCanvas.ActualWidth, (int)selCanvas.ActualHeight, 96, 96, PixelFormats.Pbgra32);
			renderTargetBitmap.Render(selCanvas);
			PngBitmapEncoder encoder = new();
			encoder.Frames.Add(BitmapFrame.Create(renderTargetBitmap));
			TabItem ti = TC_Tabs.SelectedItem as TabItem;
			string name = ti.Header.ToString();

			SaveFileDialog saveFileDialog = new();
			saveFileDialog.FileName = name;
			saveFileDialog.Filter = "PNG Files (*.png)|*.png";
			if (saveFileDialog.ShowDialog() == true)
			{
				using (var fileStream = new FileStream(saveFileDialog.FileName, FileMode.Create))
				{
					encoder.Save(fileStream);
				}
			}
		}
	}
}