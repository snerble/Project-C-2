using Microsoft.CodeAnalysis.CSharp;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace QARS
{
	/// <summary>
	/// Static class containing miscellaneous utility functions.
	/// </summary>
	internal static class Utils
	{
		/// <summary>
		/// Returns a string representation of a given object which displays it's public properties.
		/// </summary>
		public static string GetDetailedString(object o)
		{
			var values = new List<object>();

			// Loop through all public instance properties
			foreach (PropertyInfo prop in from prop in o.GetType().GetProperties(BindingFlags.Public | BindingFlags.Instance)
										  where prop.CanRead
										  select prop)
			{
				object value = prop.GetValue(o);

				// If value is a string, escape it's characters
				if ((Nullable.GetUnderlyingType(prop.PropertyType) ?? prop.PropertyType) == typeof(string))
					value = SymbolDisplay.FormatLiteral(value as string, true);

				// If value is DBNull or just null, replace it with the string 'null'
				if (value == DBNull.Value || value is null)
					value = "null";

				values.Add(value ?? "null");
			}

			return $"{o.GetType().Name}({string.Join(", ", values)})";
		}
	}
}
