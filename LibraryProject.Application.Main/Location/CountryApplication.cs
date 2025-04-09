//using LibraryProject.Application.Interface.Location;
//using LibraryProject.Domain.Entities.Location;
//using LibraryProject.Infraestructure.Interface.Location;
//using LibraryProject.Transversal.Common;

//namespace LibraryProject.Application.Services.Location
//{
//    public class CountryApplication : ICountryApplication
//    {
//        private readonly ICountryRepository _countryRepository;
//        private readonly IAppLogger<CountryApplication> _logger;

//        public CountryApplication(ICountryRepository countryRepository, IAppLogger<CountryApplication> logger)
//        {
//            _countryRepository = countryRepository;
//            _logger = logger;
//        }

//        // Método corregido para ser asíncrono
//        //public async Task<IEnumerable<CountryME>> GetAllCountries()
//        //{
//        //    //return await _countryRepository.GetAllCountriesAsync();
//        //}
//    }
//}
