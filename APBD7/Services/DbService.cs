using APBD7.DTOs.Requests;
using APBD7.DTOs.Responses;
using APBD7.Models;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.EntityFrameworkCore;
using System.Reflection.Metadata.Ecma335;

namespace APBD7.Services
{
    public class DbService : IDbService
    {

        private readonly S22323Context context;

        public DbService(S22323Context context)
        {
            this.context = context;
        }
        public async Task AssignClientToTrip(int idTrip, AssignClientToTripDTO clientDto)
        {
            Client client;
            bool clientExists = await context.Clients.AnyAsync(x => x.Pesel == clientDto.Pesel);
            if (!clientExists)
            {
                client = new Client
                {
                    IdClient = await context.Clients.Select(x => x.IdClient).MaxAsync() + 1,
                    FirstName = clientDto.FirstName,
                    LastName = clientDto.LastName,
                    Email = clientDto.Email,
                    Telephone = clientDto.Telephone,
                    Pesel = clientDto.Pesel
                };
            } else
            {
                client = await context.Clients.FirstOrDefaultAsync(x => x.Pesel == clientDto.Pesel);
            }

            bool TripExists = await context.Trips.AnyAsync(x => x.IdTrip == idTrip);
            if(!TripExists)
            {
                throw new Exception("Nie ma takiej wycieczki");
            }

            bool ClientAlreadyInTrip = await context.ClientTrips.AnyAsync(x => x.IdClient == client.IdClient && x.IdTrip == idTrip);
            if (ClientAlreadyInTrip)
            {
                throw new Exception("Klient jest już zapisany na wycieczkę");
            }

            await context.ClientTrips.AddAsync(new ClientTrip
            {
                IdClient = client.IdClient,
                IdTrip = idTrip,
                RegisteredAt = DateTime.Now,
                PaymentDate = clientDto.PaymentDate
            });
            await context.SaveChangesAsync();
        }

        public async Task DeleteClient(int id)
        {
            bool hasTrips = await context.ClientTrips.AnyAsync(x => x.IdClient == id);
            if(hasTrips)
            {
                throw new Exception("Client has trips, you can't delete him");
            }
            var client = await context.Clients.Where(x => x.IdClient == id).FirstOrDefaultAsync();
            context.Remove(client);

            await context.SaveChangesAsync();
        }

        public async Task<IEnumerable<GetTripsDTO>> GetTrips()
        {
            var result = await context.Trips.Select(x => new GetTripsDTO
            {
                Name = x.Name,
                Description = x.Description,
                DateFrom = x.DateFrom,
                DateTo = x.DateTo,
                MaxPeople = x.MaxPeople,
                Countries = x.CountryTrips.Select(x => new CountryDTO { name = x.IdCountryNavigation.Name }),
                Clients = x.ClientTrips.Select(x => new ClientDTO
                {
                    FirstName = x.IdClientNavigation.FirstName,
                    LastName = x.IdClientNavigation.LastName
                })
            }).OrderByDescending(y => y.DateFrom).ToListAsync();
            return result;
            
        }
    }
}
