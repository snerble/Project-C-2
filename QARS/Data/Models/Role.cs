using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics;

namespace QARS.Data.Models
{
	/// <summary>
	/// Represents a role for a <see cref="User"/> that dictates what they are
	/// authorized for.
	/// </summary>
	[DebuggerDisplay("Id = {Id}, Name = {Name}")]
	public class Role
	{
		#region Builtin Roles
		/// <summary>
		/// The name of the builtin <see cref="Role"/> for <see cref="Models.Customer"/>.
		/// </summary>
		public const string Customer = "Customer";
		/// <summary>
		/// The name of the builtin <see cref="Role"/> for <see cref="Models.Franchisee"/>.
		/// </summary>
		public const string Franchisee = "Franchisee";
		/// <summary>
		/// The name of the builtin <see cref="Role"/> for <see cref="Models.Employee"/>.
		/// </summary>
		public const string Employee = "Employee";
		/// <summary>
		/// The name of the builtin <see cref="Role"/> for <see cref="Administrator"/>.
		/// </summary>
		public const string Admin = "Admin";
		#endregion

		/// <summary>
		/// Gets or sets the primary key of this <see cref="Role"/>.
		/// </summary>
		[DatabaseGenerated(DatabaseGeneratedOption.Identity)]
		public int? Id { get; set; }

		/// <summary>
		/// Gets or sets the name of this <see cref="Role"/>.
		/// </summary>
		[Required, MinLength(1)]
		public string Name { get; set; }

		/// <summary>
		/// Gets or sets the normalized name of this <see cref="Role"/>.
		/// <para/>
		/// This property is used to optimize queries.
		/// </summary>
		[Required, MinLength(1)]
		public string NormalizedName { get; set; }
	}
}
