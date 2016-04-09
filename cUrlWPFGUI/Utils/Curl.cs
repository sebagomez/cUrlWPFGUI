using System;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Text;

namespace cUrlWPFGUI.Utils
{
	internal class Curl
	{
		public string Method { get; set; }
		public string Url { get; set; }
		public string Body { get; set; }
		public ObservableCollection<Header> Headers { get; set; }
		public bool JsonContent { get; set; }
		public bool AcceptSelfSignedCerts { get; set; }
		public bool Verbose { get; set; }
		public string Output { get; set; }
		public string Status { get; set; }

		public bool Run()
		{
			bool ret;
			string value;
			using (Process proc = new Process())
			{
				proc.StartInfo.CreateNoWindow = true;
				proc.StartInfo.ErrorDialog = false;
				proc.StartInfo.RedirectStandardError = true;
				proc.StartInfo.RedirectStandardOutput = true;
				proc.StartInfo.UseShellExecute = false;
				proc.StartInfo.FileName = @"C:\ProgramData\chocolatey\bin\curl.exe";
				proc.StartInfo.Arguments = GetArguments();

				proc.Start();
				string standardError = proc.StandardError.ReadToEnd();
				value = proc.StandardOutput.ReadToEnd();
				proc.WaitForExit();

				ret = proc.ExitCode == 0;

				Output = value;
				Status = standardError;
			}

			return ret;
		}

		string GetArguments()
		{
			Validate();

			StringBuilder arguments = new StringBuilder();
			if (Verbose)
				arguments.Append("-v ");

			if (AcceptSelfSignedCerts)
				arguments.Append("--insecure ");

			arguments.Append($"-X{Method} ");
			foreach (Header header in Headers)
				arguments.Append($"-H '{header.Name}:{header.Value}' ");

			if (JsonContent)
				arguments.Append($"-H 'Content-type:application/json' ");

			if (!string.IsNullOrWhiteSpace(Body))
				arguments.Append($"-d '{Body}' ");

			arguments.Append($"{Url}");

			return arguments.ToString();
		}

		void Validate()
		{
			if (string.IsNullOrWhiteSpace(Url))
				throw new ApplicationException("A url must be set");
		}
	}
}
