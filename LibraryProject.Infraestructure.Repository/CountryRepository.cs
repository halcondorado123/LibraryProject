using LibraryProject.Domain.Entities.Location;
using LibraryProject.Infraestructure.Data.DbContext;
using LibraryProject.Infraestructure.Interface.Location;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibraryProject.Infraestructure.Repository
{
    public class CountryRepository : ICountryRepository
    {
        private readonly LocationDbContext _context;

        public CountryRepository(LocationDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<CountryME>> GetAllCountriesAsync()
        {
            return await _context.Countries.ToListAsync();
        }
    }
}
