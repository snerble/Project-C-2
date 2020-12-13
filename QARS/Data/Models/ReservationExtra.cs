namespace QARS.Data.Models
{
	/// <summary>
	/// Represents the join table for the many-to-many relationship between
	/// <see cref="Models.Reservation"/> and <see cref="Models.Extra"/>.
	/// </summary>
	public class ReservationExtra
	{
		/// <summary>
		/// Gets or sets the <see cref="Models.Reservation"/> associated with <see cref="Extra"/>.
		/// </summary>
		public Reservation Reservation { get; set; }
		/// <summary>
		/// Gets or sets the <see cref="Models.Extra"/> associated with <see cref="Reservation"/>.
		/// </summary>
		public Extra Extra { get; set; }

		public override string ToString() => Utils.GetDetailedString(this);
	}
}
