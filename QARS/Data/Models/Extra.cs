using System.Collections.Generic;

namespace QARS.Data.Models
{
	public class Extra
	{
		public int? Id { get; set; }
		public string Name { get; set; }
		public decimal Price { get; set; }

		public IList<CarExtra> CarExtras { get; set; }

		public override string ToString() => Utils.GetDetailedString(this);
	}
}
