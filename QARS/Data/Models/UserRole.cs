using System.Diagnostics;

namespace QARS.Data.Models
{
	/// <summary>
	/// Represents the entries in the join-table that links <see cref="User"/>
	/// with <see cref="Role"/> in a many-to-many relationship.
	/// </summary>
	[DebuggerDisplay("User = {User}, Role = {Role}")]
	public class UserRole
	{
		/// <summary>
		/// Gets or sets the <see cref="Models.User"/> of this user-role relation.
		/// </summary>
		public User User { get; set; }
		/// <summary>
		/// Gets or sets the <see cref="Models.Role"/> of this user-role relation.
		/// </summary>
		public Role Role { get; set; }

		public override string ToString() => Utils.GetDetailedString(this);
	}
}
