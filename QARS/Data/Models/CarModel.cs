using System;
using System.ComponentModel.DataAnnotations;

namespace QARS.Data.Models
{
	/// <summary>
	/// Describes the attributes of a particular automobile model.
	/// </summary>
	public class CarModel
	{
		/// <summary>
		/// Gets or sets the primary key of this <see cref="CarModel"/>.
		/// </summary>
		public int Id { get; set; }

		/// <summary>
		/// Gets or sets the brand of this <see cref="CarModel"/>.
		/// </summary>
		[Required, StringLength(50)]
		public string Brand { get; set; }
		/// <summary>
		/// Gets or sets the type of this <see cref="CarModel"/>.
		/// </summary>
		[Required, StringLength(50)]
		public string Type { get; set; }

		/// <summary>
		/// Gets or sets the <see cref="CarCategory"/> of this <see cref="CarModel"/>.
		/// </summary>
		[Filter(FilterMode.Blacklist, CarCategory.Undefined)]
		public CarCategory Category { get; set; }
		/// <summary>
		/// Gets or sets the <see cref="Models.FuelType"/> of this <see cref="CarModel"/>.
		/// </summary>
		[Filter(FilterMode.Blacklist, FuelType.Undefined)]
		public FuelType FuelType { get; set; }

		/// <summary>
		/// Gets or sets the day rate of this <see cref="CarModel"/>.
		/// </summary>
		[Range(0, double.MaxValue)]
		public decimal DayRate { get; set; }
		/// <summary>
		/// Gets or sets the kilometer rate of this <see cref="CarModel"/>.
		/// </summary>
		[Range(0, double.MaxValue)]
		public decimal KMRate { get; set; }
		/// <summary>
		/// Gets or sets how many kilometers may be driven with this <see cref="CarModel"/>
		/// before the <see cref="KMRate"/> is charged.
		/// </summary>
		[Range(0, double.MaxValue)]
		public decimal FreeMileage { get; set; }

		/// <summary>
		/// Gets or sets how many passengers fit in this <see cref="CarModel"/>.
		/// </summary>
		[Range(0, int.MaxValue)]
		public int Passengers { get; set; }
		/// <summary>
		/// Gets or sets the amount of doors on this <see cref="CarModel"/>.
		/// </summary>
		[Range(2, int.MaxValue)]
		public int Doors { get; set; }
		/// <summary>
		/// Gets or sets the amount of suitcases for this <see cref="CarModel"/>.
		/// </summary>
		/// <remarks>
		/// I have no idea what this means.
		/// </remarks>
		[Range(0, int.MaxValue)]
		public int SuitCases { get; set; }

		/// <summary>
		/// Gets or sets what kind of transmission this <see cref="CarModel"/> has.
		/// </summary>
		[Filter(FilterMode.Blacklist, TransmissionType.Undefined)]
		public TransmissionType Transmission { get; set; }

		/// <summary>
		/// Gets or sets whether this <see cref="CarModel"/> has airconditioning.
		/// </summary>
		public bool HasAirconditioning { get; set; }

		/// <summary>
		/// Gets or sets the efficiency of this <see cref="CarModel"/>.
		/// </summary>
		[Range(0, float.MaxValue)]
		public float Efficiency { get; set; }
		/// <summary>
		/// Gets or sets the emission of this <see cref="CarModel"/>.
		/// </summary>
		[Range(0, int.MaxValue)]
		public int Emission { get; set; }

		/// <summary>
		/// Gets or sets the description of this <see cref="CarModel"/>.
		/// </summary>
		[StringLength(1000)]
		public string Description { get; set; }
	}

	/// <summary>
	/// Specifies the type of transmission in an automobile.
	/// </summary>
	/// <see href="https://www.motorbiscuit.com/4-types-of-car-transmissions-and-how-they-work/">Source</see>
	public enum TransmissionType
	{
		Undefined = default,
		Manual,
		Automatic,
		ContinuouslyVariable,
		SemiAutomatic
	}

	/// <summary>
	/// Specifies the size classification of an automobile.
	/// </summary>
	/// <see href="https://en.wikipedia.org/wiki/Car_classification#Market_segments">
	/// Documentation on automobile size classification.</see>
	public enum CarCategory
	{
		Undefined = default,
		Mini,
		Compact,
		Economy,
		Midsize,
		Standard,
		Fullsize,
		Luxury
	}

	/// <summary>
	/// Collection of various types of fuel.
	/// </summary>
	/// <see href="https://en.wikipedia.org/wiki/Motor_fuel">Source</see>
	public enum FuelType
	{
		Undefined = default,
		Gasoline,
		Diesel,
		BioDiesel,
		BioGasoline,
		LeadReplacementPetrol,
		Kerosene,
		Compressednaturalgas,
		Hydrogen,
		Ethanol,
		ButanolFuel,
		RacingFuel, // Tetraethyllead
		Electric
	}


}
