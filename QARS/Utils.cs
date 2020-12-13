using Microsoft.AspNetCore.Identity;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.EntityFrameworkCore;

using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
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
		/// Returns a string representation of a given object which includes it's public properties.
		/// </summary>
		public static string GetDetailedString(object o)
		{
			// List to store the values that will later be combined
			var values = new List<object>();

			// Loop through all public instance properties
			foreach (PropertyInfo prop in from prop in o.GetType().GetProperties(BindingFlags.Public | BindingFlags.Instance)
										  where prop.CanRead && prop.GetCustomAttribute<NotMappedAttribute>() == null
										  select prop)
			{
				object value = prop.GetValue(o);

				// If value is DBNull or just null, replace it with the string 'null'
				if (value == DBNull.Value || value is null)
					value = "null";

				// If value is a string, escape it's characters
				else if ((Nullable.GetUnderlyingType(prop.PropertyType) ?? prop.PropertyType) == typeof(string))
					value = SymbolDisplay.FormatLiteral(value as string, true);

				values.Add(value);
			}

			// Return a string that begins with the type name, followed the values from it's public properties
			return $"{o.GetType().Name}({string.Join(", ", values)})";
		}

		/// <summary>
		/// Generates IdentityError objects from the given database exception.
		/// </summary>
		public static IdentityError[] GetIdentityErrors(DbUpdateException e)
		{
			var errors = new List<IdentityError> { new IdentityError { Code = e.GetType().FullName, Description = e.Message } };
			errors.AddRange(e.Data.CastToString().Select(x => new IdentityError { Code = x.Key, Description = x.Value }));
			errors.AddRange(e.Entries.Select(x =>
			{
				string primaryKey = string.Join(", ",
					x.CurrentValues.EntityType.FindPrimaryKey().Properties.Select(
						y => $"{y.Name}: {x.CurrentValues.GetValue<string>(y)}")
				);
				return new IdentityError() { Code = $"Entity ({x.Entity.GetType()})", Description = primaryKey };
			}));

			return errors.ToArray();
		}

		/// <summary>
		/// Converts this dictionary to a generic dictionary with the specified types.
		/// </summary>
		/// <param name="dictionary">The dictionary to cast.</param>
		/// <returns>The converted dictionary.</returns>
		public static IDictionary<TKey, TValue> Cast<TKey, TValue>(this IDictionary dictionary)
			=> dictionary.Keys.Cast<object>().ToDictionary(k => (TKey)k, k => (TValue)dictionary[k]);

		/// <summary>
		/// Converts this dictionary to a generic dictionary containing only strings.
		/// </summary>
		/// <param name="dictionary">The dictionary to cast.</param>
		/// <returns>The converted dictionary.</returns>
		public static IDictionary<string, string> CastToString(this IDictionary dictionary)
			=> dictionary.Keys.Cast<object>().ToDictionary(k => k.ToString(), k => dictionary[k].ToString());
	}
}
