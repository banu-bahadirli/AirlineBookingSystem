using AirlineBookingSystem.Flights.Core.Entities;
using AirlineBookingSystem.Flights.Core.Repositories;
using Dapper;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AirlineBookingSystem.Flights.Infrastructure.Repositories
{
	public class FlightRepository : IFlightRepository
	{
		private readonly IDbConnection _dbConnection;

		public FlightRepository(IDbConnection dbConnection)
		{
			_dbConnection = dbConnection;
		}
		public async Task AddFlightAsync(Flight flight)
		{
			const string sql = @"
				INSERT INTO Flights (Id, FlightNumber, Origin, Destination, DepartureTime, ArrivalTime)
				VALUES (@Id, @FlightNumber, @Origin, @Destination, @DepartureTime, @ArrivalTime)";

			await _dbConnection.ExecuteAsync(sql, flight);
		}

		public async Task DeleteFlightAsync(Guid id)
		{
			const string sql = "DELETE FROM Flights WHERE Id = @Id";
			await _dbConnection.ExecuteAsync(sql, new { Id = id });
		}

		public async Task<IEnumerable<Flight>> GetAllFlightsAsync()
		{
			const string sql = "SELECT * FROM Flights";
			return await _dbConnection.QueryAsync<Flight>(sql);
		}
	}
}
