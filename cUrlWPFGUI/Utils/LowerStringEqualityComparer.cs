using System.Collections.Generic;

namespace cUrlWPFGUI.Utils
{
	class LowerStringEqualityComparer : IEqualityComparer<string>
	{
		public bool Equals(string x, string y)
		{
			if (string.IsNullOrEmpty(x) && string.IsNullOrEmpty(y))
				return true;
			else if (string.IsNullOrEmpty(x) || string.IsNullOrEmpty(y))
				return false;

			return x.Trim().ToLower() == y.Trim().ToLower();
		}

		public int GetHashCode(string obj)
		{
			return obj.GetHashCode();
		}
	}
}
