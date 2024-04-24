using System;
using System.Diagnostics;
using System.Windows;

namespace EllipDraw
{
	/// <summary>
	/// A Window to display information about the application.
	/// </summary>
	public partial class About : Window
	{
		public About()
		{
			InitializeComponent();
		}

		private void Hyperlink_RequestNavigate(object sender, System.Windows.Navigation.RequestNavigateEventArgs e)
		{
			try
			{
				Process.Start(new ProcessStartInfo
				{
					FileName = e.Uri.AbsoluteUri,
					UseShellExecute = true
				});
				e.Handled = true;
			}
			catch (Exception ex)
			{
				MessageBox.Show($"Error opening URL: {ex.Message}");
			}
		}
	}
}
