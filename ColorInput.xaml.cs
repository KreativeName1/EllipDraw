using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;

namespace EllipDraw
{
	/// <summary>
	/// A Window to select a color using the ColorPicker.
	/// The user can confirm or cancel the selection.
	/// </summary>
	public partial class ColorInput : Window
	{
		public Color SelectedColor { get; set; }
		public ColorInput(Color? color)
		{
			InitializeComponent();
			if (color != null) CC_Color.SelectedColor = new Color?(color.Value);
			this.KeyDown += (sender, e) =>
			{
				if (e.Key == Key.Enter)
				{
					BTN_OK.RaiseEvent(new RoutedEventArgs(Button.ClickEvent));
				}
				else if (e.Key == Key.Escape)
				{
					BTN_Cancel.RaiseEvent(new RoutedEventArgs(Button.ClickEvent));
				}
			};
			BTN_OK.Click += (sender, e) =>
			{
				SelectedColor = CC_Color.SelectedColor.Value;
				DialogResult = true;
			};
			BTN_Cancel.Click += (sender, e) =>
			{
				DialogResult = false;
			};
		}
	}
}
