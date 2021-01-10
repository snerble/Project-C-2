using System;
using System.ComponentModel.DataAnnotations;

namespace QARS.Data.Models
{
	public class Contact
	{
		public int Id { get; set; }


		[Required]
		public string Tell{ get; set; }

		[Required]
		public string City { get; set; }

		[Required]
		public string Contactaddress { get; set; }

		[Required]
		public string Discript { get; set; }

		[Required]
		public byte[] Image { get; set; }


	}
}
