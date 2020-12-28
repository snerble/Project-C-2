using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace QARS.Data.Models
{
	/// <summary>
	/// Describes an optional item for a <see cref="Car"/> to include in a <see cref="Reservation"/>.
	/// </summary>
	public class Extra
	{
		/// <summary>
		/// Gets or sets the primary key of this <see cref="Extra"/>.
		/// </summary>
		public int Id { get; set; }

		/// <summary>
		/// Gets or sets the name of this <see cref="Extra"/>.
		/// </summary>
		[Required, MinLength(1)]
		public string Name { get; set; }
		/// <summary>
		/// Gets or sets the cost of adding this <see cref="Extra"/> to a <see cref="Reservation"/>.
		/// </summary>
		[Range(0, double.MaxValue)]
		public decimal Cost { get; set; }

		/// <summary>
		/// Gets or sets the description of this <see cref="Extra"/>.
		/// </summary>
		[StringLength(500)]
		public string Description { get; set; }

		public int StoreId { get; set; }

		/// <summary>
		/// Gets or sets a collection of <see cref="ReservationExtra"/>s associated
		/// with this <see cref="Extra"/>.
		/// </summary>
		public IList<ReservationExtra> ReservationExtras { get; set; }

		public override string ToString() => Utils.GetDetailedString(this);
	}
}
