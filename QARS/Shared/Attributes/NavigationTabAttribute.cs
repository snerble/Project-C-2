using System;

namespace QARS.Shared.Attributes
{
	/// <summary>
	/// Specifies that the given razor page should be displayed in the navigation bar.
	/// </summary>
	public class NavigationTabAttribute : Attribute
	{
		public NavigationTabAttribute(string name)
		{
			Name = name;
		}

		/// <summary>
		/// Gets the display name of this navigation tab.
		/// </summary>
		public string Name { get; }
		/// <summary>
		/// Gets or sets the placement in the nagivation tab.
		/// </summary>
		public int Index { get; set; }
	}
}
