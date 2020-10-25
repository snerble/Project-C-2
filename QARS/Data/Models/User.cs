using System.ComponentModel.DataAnnotations;

namespace QARS.Data.Models
{
	public partial class User
	{
		public int? Id { get; set; }
		public string FullName { get; set; }
		public string UserName { get; set; }
		[EmailAddress]
		public string Email { get; set; }
		public int Age { get; set; }

		public override string ToString() => Utils.GetDetailedString(this);
	}
}
