using System.ComponentModel.DataAnnotations;


namespace QARS.Data.Models
{
	public class Return
	{
		[Required]
		public int Id { get; set; }
		
		public Reservation Reservations { get; set; }


	}
}
