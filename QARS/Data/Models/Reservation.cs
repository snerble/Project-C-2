using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;

namespace QARS.Data.Models
{
	/// <summary>
	/// Describes details about a <see cref="Models.Car"/> reservation made by a <see cref="Models.Customer"/>.
	/// </summary>
	public class Reservation
	{
		/// <summary>
		/// Default parameterless constructor. This is reserved for Entity Framework and should <b>NOT</b>
		/// be called manually.
		/// <para/>
		/// Please use the static constructor
		/// <see cref="Create(Customer, Car, DateTimeOffset, DateTimeOffset, ReservationExtra[])"/> instead.
		/// </summary>
		public Reservation() { }

		/// <summary>
		/// Gets or sets the primary key of this <see cref="Reservation"/>.
		/// </summary>
		public int Id { get; set; }

		/// <summary>
		/// Gets or sets the <see cref="Models.Customer"/> that made this <see cref="Reservation"/>.
		/// </summary>
		public Customer Customer { get; set; }
		/// <summary>
		/// Gets or sets the <see cref="Models.Car"/> that is being reserved.
		/// </summary>
		public Car Car { get; set; }

		public int CarId { get; set; }
		/// <summary>
		/// Gets or sets the initial <see cref="Store"/> location if the <see cref="Car"/>
		/// when this reservation was made.
		/// </summary>
		public Location CarLocation { get; set; }

		public int pickupLocation { get; set; }

		public int dropoffLocation { get; set; }

		/// <summary>
		/// Gets the mileage of the <see cref="Car"/> when this <see cref="Reservation"/>
		/// was made.
		/// </summary>
		/// <seealso cref="Car.Mileage"/>
		[Range(0, double.MaxValue)]
		public decimal InitialMileage { get; set; }

		// TODO: Add shadowed private fields and prevent Start from being larger or equal to End
		/// <summary>
		/// Gets or sets when this <see cref="Reservation"/> starts.
		/// </summary>
		public DateTimeOffset Start { get; set; }
		/// <summary>
		/// Gets or sets when this <see cref="Reservation"/> ends.
		/// </summary>
		public DateTimeOffset End { get; set; }

		public ReservationState Status { get; set; }

		/// <summary>
		/// Gets the duration of this <see cref="Reservation"/>.
		/// </summary>
		[NotMapped]
		public TimeSpan Duration => End.UtcDateTime - Start.UtcDateTime;

		/// <summary>
		/// Gets which stage this reservation is in currently.
		/// </summary>
		[NotMapped]
		public ReservationState State
		{
			get
			{
				DateTimeOffset now = DateTimeOffset.UtcNow;
				return DateTimeOffset.UtcNow switch
				{
					_ when now < Start => ReservationState.Planned,
					_ when now < End => ReservationState.InProgress,
					_ => ReservationState.Finished
				};
			}
		}

		/// <summary>
		/// Gets or sets a list of <see cref="ReservationExtra"/>s associated with this
		/// <see cref="Reservation"/>.
		/// </summary>
		public IList<ReservationExtra> Extras { get; set; }

		/// <summary>
		/// Calculates and returns the cost of this <see cref="Reservation"/> up to this moment.
		/// </summary>
		public decimal CalculateCost()
		{
			DateTimeOffset now = DateTimeOffset.UtcNow;

			TimeSpan timeDriven = new DateTimeOffset(Math.Clamp(now.Ticks, Start.Ticks, End.Ticks), now.Offset) - Start;
			decimal drivenKM = Math.Max(Car.Mileage - InitialMileage - Car.Model.FreeMileage, 0);

			return Extras.Sum(x => x.Extra.Cost)
				+ ((decimal)timeDriven.TotalDays * Car.Model.DayRate) // TODO: Consider casting timeDriven to an int to represent whole days
				+ (drivenKM * Car.Model.KMRate);
		}

		/// <summary>
		/// Initializes a new instance of <see cref="Reservation"/> with the specified parameters.
		/// </summary>
		/// <param name="customer">The customer that wants to create a new reservation.</param>
		/// <param name="car">The car that is going to be reserved.</param>
		/// <param name="start">The start date and time time for when this reservation will go in effect.</param>
		/// <param name="end">The end date and time for when this reservation will finish.</param>
		/// <param name="extras">A collection of extras to add to the new reservation.</param>
		/// <returns>A new <see cref="Reservation"/> instance.</returns>
		public static Reservation Create(Customer customer, Car car, int pickupLocation, int dropoffLocation, DateTimeOffset start, DateTimeOffset end, params Extra[] extras)
		{


			// Get the UTC times for start and end
			(start, end) = (start.UtcDateTime, end.UtcDateTime);

			lock (car)
			{
				if (start < DateTimeOffset.UtcNow)
					throw new ArgumentException("A new reservation may not start in the past.");
				if (end <= start)
					throw new ArgumentException("End must be a larger value than start.");

				try
				{
					// Create the new reservation
					var reservation = new Reservation()
					{
						Customer = customer,
						Car = car,
						CarId = car.Id,
						pickupLocation = pickupLocation,
						dropoffLocation = dropoffLocation,
						CarLocation = car.Location,
						InitialMileage = car.Mileage,
						Start = start,
						End = end
					};
					reservation.Extras = extras.Select(e => new ReservationExtra() { Reservation = reservation, Extra = e }).ToList();
					return reservation;
				}
				finally
				{
					// After returning the new reservation, mark the car as reserved.
				}
			}
		}
	}

	/// <summary>
	/// Describes the various states of a <see cref="Reservation"/>.
	/// </summary>
	public enum ReservationState
	{
		/// <summary>
		/// The reservation has not yet started.
		/// </summary>
		Planned = default,
		/// <summary>
		/// The reservation is currently in effect.
		/// </summary>
		InProgress,
		/// <summary>
		/// The current reservation has finished.
		/// </summary>
		Finished
	}
}
