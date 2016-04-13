using System.Collections.ObjectModel;
using System.Windows;
using cUrlWPFGUI.Utils;

namespace cUrlWPFGUI
{
	/// <summary>
	/// Interaction logic for MainWindow.xaml
	/// </summary>
	public partial class MainWindow : Window
	{
		public ObservableCollection<Header> Headers = new ObservableCollection<Header>();

		public MainWindow()
		{
			InitializeComponent();

			GridHeaders.DataContext = Headers;
			Method.SelectedIndex = 0;
		}

		private void Go_Click(object sender, RoutedEventArgs e)
		{
			Output.Document.Blocks.Clear();

			Curl curl = new Curl();
			curl.Method = Method.Text;
			curl.Url = Url.Text;
			curl.Body = Body.Text;
			curl.Headers = Headers;
			curl.JsonContent = ChkJson.IsChecked.HasValue ? ChkJson.IsChecked.Value : false;
			curl.AcceptSelfSignedCerts = ChkSelfSigne.IsChecked.HasValue ? ChkSelfSigne.IsChecked.Value : false;
			curl.Verbose = ChkVerbose.IsChecked.HasValue ? ChkVerbose.IsChecked.Value : false;

			curl.Run();

			Output.AppendText(curl.Output);
		}

		private void AddHeader_Click(object sender, RoutedEventArgs e)
		{
			if (string.IsNullOrWhiteSpace(HeaderName.Text) || string.IsNullOrWhiteSpace(HeaderValue.Text))
				return;
			Header header = new Header { Name = HeaderName.Text.Trim(), Value = HeaderValue.Text.Trim() };

			if (Headers.Contains(header))
				return;

			Headers.Add(header);
			HeaderName.Text = "";
			HeaderValue.Text = "";
		}

		private void RemoveHeader_Click(object sender, RoutedEventArgs e)
		{
			Header header = ((FrameworkElement)sender).DataContext as Header;
			Headers.Remove(header);
		}
	}
}
