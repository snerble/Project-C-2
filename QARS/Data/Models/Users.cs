using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Security.Cryptography;
using System.Text;

namespace QARS.Data.Models
{
	/// <summary>
	/// Serves as the base class for all user types in <see cref="QARS"/>.
	/// </summary>
	public abstract class User
	{
		/// <summary>
		/// Gets or sets the primary key of this <see cref="User"/>.
		/// </summary>
		[DatabaseGenerated(DatabaseGeneratedOption.Identity)]
		public int Id { get; set; }
		/// <summary>
		/// Gets or sets the e-mail address of this <see cref="User"/>.
		/// </summary>
		[Required, EmailAddress]
		public string Email { get; set; }

		/// <summary>
		/// Gets or sets the hashed password for this <see cref="User"/>.
		/// </summary>
		[Required, MinLength(64), MaxLength(64)]
		public byte[] Password { get; set; }
		/// <summary>
		/// Gets or sets the raw password.
		/// <para/>
		/// This property is not mapped and is only intended for temporary storage.
		/// </summary>
		[NotMapped]
		internal string RawPassword { get; set; }

		/// <summary>
		/// Gets or sets the first name of this <see cref="User"/>.
		/// </summary>
		[Required, MinLength(1)]
		public string FirstName { get; set; }
		/// <summary>
		/// Gets or sets the last name of this <see cref="User"/>.
		/// </summary>
		[Required, MinLength(1)]
		public string LastName { get; set; }
		/// <summary>
		/// Gets or sets the phone number of this <see cref="User"/>.
		/// </summary>
		[Required, Phone]
		public string PhoneNumber { get; set; }
		
		/// <summary>
		/// Gets or sets the id of the <see cref="Location"/> of this <see cref="User"/>.
		/// </summary>
		public int LocationId { get; set; }
		/// <summary>
		/// Gets or sets the location where this <see cref="User"/> resides.
		/// </summary>
		public virtual Location Location { get; set; }

		public override string ToString() => Utils.GetDetailedString(this);

		/// <summary>
		/// Hashes the given <paramref name="password"/> together with <see cref="Email"/> and sets
		/// the <see cref="Password"/> property.
		/// </summary>
		/// <param name="password">The raw password of this user.</param>
		public void SetPassword(string password)
		{
			if (string.IsNullOrEmpty(Email))
				throw new InvalidOperationException($"{nameof(Password)} cannot be set if {nameof(Email)} is undefined or empty.");

			RawPassword = password;
			Password = GetPasswordHash(Email, password);
		}

		/// <summary>
		/// Generates and returns a new salted <see cref="SHA512"/> hash.
		/// </summary>
		/// <param name="salt">A unique string (preferably a username) to add before hashing.</param>
		/// <param name="password">The password that will be hashed.</param>
		/// <exception cref="ArgumentException"><paramref name="salt"/> or <paramref name="password"/> are null or empty.</exception>
		public static byte[] GetPasswordHash(string salt, string password)
		{
			if (string.IsNullOrEmpty(salt))
				throw new ArgumentException($"'{nameof(salt)}' cannot be null or empty", nameof(salt));
			if (string.IsNullOrEmpty(password))
				throw new ArgumentException($"'{nameof(password)}' cannot be null or empty", nameof(password));

			string saltedString = $"{salt}#:#{password}"; // Salt the password
			using var sha512 = SHA512.Create();
			return sha512.ComputeHash(Encoding.UTF8.GetBytes(saltedString));
		}
	}

	/// <summary>
	/// Represents a user with special privileges in the system.
	/// </summary>
	public sealed class Administrator : User { }
	/// <summary>
	/// Represents a franchise owner.
	/// </summary>
	public sealed class Franchisee : User { }
	/// <summary>
	/// Represents an employee working at <see cref="QARS"/>.
	/// </summary>
	public sealed class Employee : User { }
	/// <summary>
	/// Represents a customer at <see cref="QARS"/>.
	/// </summary>
	public sealed class Customer : User { }
}
