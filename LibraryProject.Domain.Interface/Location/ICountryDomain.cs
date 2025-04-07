using LibraryProject.Domain.Entities.Location;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibraryProject.Domain.Interface.Location
{
    public interface ICountryDomain
    {
        Task<IEnumerable<CountryME>> GetAllCountriesAsync();
    }
}
