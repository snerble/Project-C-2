using System.ComponentModel.DataAnnotations;

namespace QARS.Data.Models
{
	/// <summary>
	/// Represents a physical store owned by a <see cref="Franchisee"/>.
	/// </summary>
	public class Store
	{
		/// <summary>
		/// Gets or sets the primary key of this <see cref="Store"/>.
		/// </summary>
		public int Id { get; set; }

		public string Name { get; set; }

		/// <summary>
		/// Gets or sets the <see cref="Franchisee"/> that owns this <see cref="Store"/>.
		/// </summary>
		public Franchisee Franchisee { get; set; }

		/// <summary>
		/// Gets or sets the id of the <see cref="Location"/> of this <see cref="Store"/>.
		/// </summary>
		public int LocationId { get; set; }
		/// <summary>
		/// Gets or sets the location of this <see cref="Store"/>.
		/// </summary>
		public Location Location { get; set; }

		/// <summary>
		/// Gets or sets the description of this <see cref="Store"/>.
		/// </summary>
		[StringLength(2500)]
		public string Description { get; set; }
	}
}
