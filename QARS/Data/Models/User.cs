using Microsoft.CodeAnalysis.CSharp;

using System;
using System.Collections.Generic;
using System.Reflection;

namespace QARS.Data.Models
{
	public class User
	{
		public int? Id { get; set; }
		public string FullName { get; set; }
		public string UserName { get; set; }
		public string Email { get; set; }
		public int Age { get; set; }

		public override string ToString() => ObjectToString(this);

		public static string ObjectToString(object o)
		{
			var values = new List<object>();

			foreach (PropertyInfo prop in o.GetType().GetProperties())
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
