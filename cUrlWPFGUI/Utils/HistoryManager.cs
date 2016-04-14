using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Json;

namespace cUrlWPFGUI.Utils
{
	internal class HistoryManager
	{
		static List<string> s_urls;
		const string FILENAME = "cUrlGui.json";

		public static List<string> GetHistory()
		{
			if (s_urls == null)
				s_urls = Deserialize();

			return s_urls;
		}

		public static void Add(string url)
		{
			lock (s_urls)
			{
				LowerStringEqualityComparer comp = new LowerStringEqualityComparer();

				List<string> aux = GetHistory();
				if (!aux.Contains(url, comp))
				{
					s_urls.Insert(0,url);
					Serialize();
				}
			}
		}

		static void Serialize()
		{
			string fullPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), FILENAME);
			DataContractJsonSerializer serializer = new DataContractJsonSerializer(typeof(List<string>));
			using (FileStream stream = new FileStream(fullPath, FileMode.Create))
				serializer.WriteObject(stream, s_urls);
		}

		public static List<string> Deserialize()
		{
			try
			{
				List<string> retVal;
				string fullPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), FILENAME);
				DataContractJsonSerializer serializer = new DataContractJsonSerializer(typeof(List<string>));
				using (FileStream stream = new FileStream(fullPath, FileMode.OpenOrCreate))
					retVal = (List<string>)serializer.ReadObject(stream);

				return retVal;
			}
			catch (SerializationException)
			{
				return new List<string>();
			}
		}
	}
}
