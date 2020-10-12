using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace QARS.Data.Models
{
	public class Car
	{
		public int? Id { get; set; }
		public string Name { get; set; }
		public string Manufacturer { get; set; }
		public decimal Price { get; set; }
		public string LicensePlate { get; set; }

		public IList<CarExtra> CarExtras { get; set; }

		public override string ToString() => User.ObjectToString(this);
	}
}
