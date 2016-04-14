using System;

namespace cUrlWPFGUI.Utils
{
	public class Header
	{
		public string Name { get; set; }
		public string Value { get; set; }

		public override string ToString()
		{
			return $"{Name}:{Value}";
		}

		public override int GetHashCode()
		{
			return Name.GetHashCode();
		}

		public override bool Equals(object obj)
		{
			if (obj == null)
				return false;

			if (this.GetType() != obj.GetType())
				return false;

			// safe because of the GetType check
			Header header = (Header)obj;

			// use this pattern to compare reference members
			if (!Object.Equals(Name, header.Name))
				return false;

			return true;
		}
	}
}
