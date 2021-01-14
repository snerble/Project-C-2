using System;
using System.ComponentModel.DataAnnotations;

namespace QARS.Data.Models
{
	public class Contact
	{

		
		public int Id { get; set; }

		// makes it so attribute has to exist and cant be null, but also makes it so a valid phone number is given.
		[Required(ErrorMessage = "Mobile Number is required.")]
		[RegularExpression(@"^([0-9]{10})$", ErrorMessage = "Invalid Mobile Number.")]
		public string Tell { get; set; }

		[Required]
		public string City { get; set; }

		[Required]
		public string Contactaddress { get; set; }

		[Required]
		public string Countrycode { get; set; }

		[Required]
		public string Discript { get; set; }

		public byte[] Image { get; set; }


	}
}
