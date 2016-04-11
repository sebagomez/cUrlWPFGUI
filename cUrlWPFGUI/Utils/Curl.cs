using System;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Text;
using System.Threading;

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
			using (Process proc = new Process())
			{
				proc.StartInfo.CreateNoWindow = true;
				proc.StartInfo.ErrorDialog = false;
				proc.StartInfo.RedirectStandardError = true;
				proc.StartInfo.RedirectStandardOutput = true;
				proc.StartInfo.UseShellExecute = false;
				proc.StartInfo.FileName = @"C:\ProgramData\chocolatey\bin\curl.exe";
				proc.StartInfo.Arguments = GetArguments();


				StringBuilder output = new StringBuilder();
				StringBuilder error = new StringBuilder();

				using (AutoResetEvent outputWaitHandle = new AutoResetEvent(false))
				using (AutoResetEvent errorWaitHandle = new AutoResetEvent(false))
				{
					proc.OutputDataReceived += (sender, e) => {
						if (e.Data == null)
						{
							outputWaitHandle.Set();
						}
						else
						{
							output.AppendLine(e.Data);
						}
					};
					proc.ErrorDataReceived += (sender, e) =>
					{
						if (e.Data == null)
						{
							errorWaitHandle.Set();
						}
						else
						{
							error.AppendLine(e.Data);
						}
					};

					proc.Start();

					proc.BeginOutputReadLine();
					proc.BeginErrorReadLine();

					int timeout = 5000;

					if (proc.WaitForExit(timeout) &&
						outputWaitHandle.WaitOne(timeout) &&
						errorWaitHandle.WaitOne(timeout))
					{
						Output = output.ToString();
						Status = error.ToString();
					}
					else
					{
						// Timed out.
						proc.Close();
					}

					ret = proc.ExitCode == 0;

				}
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
				arguments.Append($"-H {header.Name}:{header.Value} ");

			if (JsonContent)
				arguments.Append($"-H Content-type:application/json ");

			if (!string.IsNullOrWhiteSpace(Body))
				arguments.Append($"-d {Body.Replace("\"", @"\""")} ");

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
