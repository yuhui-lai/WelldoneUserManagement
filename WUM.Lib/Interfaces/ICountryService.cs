using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WUM.Lib.Models.Common;
using WUM.Lib.Models.Country;

namespace WUM.Lib.Interfaces
{
    public interface ICountryService
    {
        CommAPIModel<List<CountrySelectOption>> GetCountries();
    }
}
