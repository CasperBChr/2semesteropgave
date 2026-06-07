using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace _2SemesterOpgave
{
	/// <summary>
	/// Interaction logic for PromptDialogWindow.xaml
	/// </summary>
	public partial class PromptDialogWindow : Window
	{
		public PromptDialogWindow(string text)
		{
			InitializeComponent();
			PromptTextBlock.Text = text;
		}


		private void YesButton_Click(object sender, RoutedEventArgs e)
		{
			this.DialogResult = true;
			this.Close();
		}

		private void NoButton_Click(object sender, RoutedEventArgs e)
		{
			this.DialogResult = false;
			this.Close();
		}
	}
}
