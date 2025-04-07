using LibraryProject.Domain.Entities.Location;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibraryProject.Application.Interface.Location
{
    public interface ICountryApplication
    {
        Task<IEnumerable<CountryME>> GetAllCountries();
    }
}
