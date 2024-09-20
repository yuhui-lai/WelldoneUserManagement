using Nager.Country;
using System.Globalization;
using WUM.Lib.Interfaces;
using WUM.Lib.Models.Common;
using WUM.Lib.Models.Country;

namespace WUM.Lib.Services
{
    public class CountryService : ICountryService
    {
        public CountryService() { }

        public CommAPIModel<List<CountrySelectOption>> GetCountries()
        {
            List<CountrySelectOption> list = new List<CountrySelectOption>();

            var countryProvider = new CountryProvider();
            var countries = countryProvider.GetCountries();
            foreach ( var country in countries )
            {
                CountrySelectOption c = new CountrySelectOption();
                c.Name = country.NativeName;
                c.EngName = country.CommonName;
                c.Code = country.Alpha3Code.ToString();
                list.Add(c);
            }
            return new CommAPIModel<List<CountrySelectOption>>
            {
                Data = list
            };
        }
    }
}
