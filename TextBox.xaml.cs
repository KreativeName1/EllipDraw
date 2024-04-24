using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace EllipDraw
{
	/// <summary>
	/// A Window to input text. It has a message and a text box.
	/// The user can input text and confirm or cancel the input.
	/// Optionally, the text box can be pre-filled with content.
	/// </summary>
	public partial class TextBox : Window
	{
		public string Input { get; set; }
		public TextBox(string message, string? content)
		{
			InitializeComponent();
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
			LBL_Message.Content = message;
			TB_Input.Text = content;
			TB_Input.Focus();
			TB_Input.SelectAll();
			BTN_Cancel.Click += (sender, e) =>
			{
				this.DialogResult = false;
			};
			BTN_OK.Click += (sender, e) =>
			{
				Input = TB_Input.Text;
				this.DialogResult = true;
			};
		}
	}
}
