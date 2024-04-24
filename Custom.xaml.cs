using System;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Shapes;

namespace EllipDraw
{
	/// <summary>
	/// A Window to create or change an ellipse.
	/// The user can input the stroke thickness,
	/// width, height, stroke color, fill color, and opacity.
	/// The user can confirm or cancel the creation or change.
	/// </summary>
	public partial class Custom : Window
	{

		public Ellipse Ell = new Ellipse();
		public Custom()
		{
			InitializeComponent();
			BTN_Create.Content = "Create";
		}
		public Custom(Ellipse ellipse)
		{
			InitializeComponent();
			if (ellipse != null)
			{
				BTN_Create.Content = "Change";
				Ell = ellipse;
				TB_StrokeThickness.Text = Ell.StrokeThickness.ToString();
				TB_Width.Text = Ell.Width.ToString();
				TB_Height.Text = Ell.Height.ToString();
				CP_Stroke.SelectedColor = ((SolidColorBrush)Ell.Stroke).Color;
				CP_Fill.SelectedColor = ((SolidColorBrush)Ell.Fill).Color;
				S_Opacity.Value = Ell.Opacity;
			}
		}
		private void BTN_Create_Click(object sender, RoutedEventArgs e)
		{
			Ell.Stroke = new SolidColorBrush((Color)CP_Stroke.SelectedColor);
			Ell.Fill = new SolidColorBrush((Color)CP_Fill.SelectedColor);
			Ell.StrokeThickness = Convert.ToDouble(TB_StrokeThickness.Text);
			Ell.Opacity = S_Opacity.Value;
			Ell.Width = Convert.ToDouble(TB_Width.Text);
			Ell.Height = Convert.ToDouble(TB_Height.Text);
			this.DialogResult = true;
		}

		private void BTN_Cancel_Click(object sender, RoutedEventArgs e)
		{
			this.DialogResult = false;
			this.Close();
		}

		private void TB_StrokeThickness_KeyUp(object sender, KeyEventArgs e)
		{
			try { newEll.StrokeThickness = Convert.ToDouble(TB_StrokeThickness.Text); }
			catch (Exception) { }
		}

		private void CP_Stroke_SelectedColorChanged(object sender, RoutedPropertyChangedEventArgs<Color?> e)
		{
			newEll.Stroke = new SolidColorBrush((Color)CP_Stroke.SelectedColor);
		}

		private void CP_Fill_SelectedColorChanged(object sender, RoutedPropertyChangedEventArgs<Color?> e)
		{
			newEll.Fill = new SolidColorBrush((Color)CP_Fill.SelectedColor);
		}

		private void S_Opacity_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
		{
			newEll.Opacity = S_Opacity.Value;
		}

		private void Window_KeyDown(object sender, KeyEventArgs e)
		{

		}
	}
}