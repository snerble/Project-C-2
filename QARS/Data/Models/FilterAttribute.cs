using Microsoft.CodeAnalysis.CSharp;

using System;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;

namespace QARS.Data.Models
{
	/// <summary>
	/// Defines an attribute that validates a property against either a whitelist or blacklist.
	/// </summary>
	[AttributeUsage(AttributeTargets.Property, AllowMultiple = false, Inherited = true)]
	public class FilterAttribute : ValidationAttribute
	{
		private readonly FilterMode mode;
		private readonly object[] list;

		/// <summary>
		/// Initializes a new instance of <see cref="FilterAttribute"/> with the
		/// specified <paramref name="args"/>.
		/// </summary>
		/// <param name="mode">The type of filtering that this attribute should perform.</param>
		/// <param name="args">A collection of values that are either
		/// whitelisted or blacklisted by this attribute.</param>
		public FilterAttribute(FilterMode mode, params object[] args)
		{
			this.mode = mode;
			list = args;
		}

		public override bool IsValid(object value)
		{
			return list.Contains(value)
				? mode == FilterMode.Whitelist  // If it contains the value and is set to whitelist, return true
				: mode == FilterMode.Blacklist; // If it does not contain the value and is set to blacklist, return true
		}

		public override string FormatErrorMessage(string name)
		{
			var sb = new StringBuilder(name);
			sb.Append(mode == FilterMode.Whitelist ? " must" : " may not");
			sb.Append(" be one of the following values: ");
			sb.AppendJoin(',', list.Select(o => o switch
			{
				string s => SymbolDisplay.FormatLiteral(s, true),
				_ when o is null => "null",
				_ => o.ToString()
			}));
			return sb.ToString();
		}
	}

	/// <summary>
	/// Specifies how a <see cref="FilterAttribute"/> should behave.
	/// </summary>
	public enum FilterMode : byte
	{
		Blacklist = 0,
		Whitelist = 1
	}
}
