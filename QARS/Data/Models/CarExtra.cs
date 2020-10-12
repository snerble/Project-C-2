using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace QARS.Data.Models
{
	public class CarExtra
	{
		public int CarId { get; set; }
		public Car Car { get; set; }
		
		public int ExtraId { get; set; }
		public Extra Extra { get; set; }
	}
}
