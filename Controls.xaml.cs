using System.Windows;

namespace EllipDraw
{
	/// <summary>
	/// A Window to display the controls of the application.
	/// </summary>
	public partial class Controls : Window
	{
		public Controls()
		{
			InitializeComponent();
		}

		private void Button_Click(object sender, RoutedEventArgs e)
		{
			this.Close();
		}
	}
}
