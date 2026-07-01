using NordVPNModels.Models;
using partycli.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;

namespace partycli.Services
{
    public class CountryService : ICountryService
    {
        public int GetCountryIdByName(string countryName)
        {
            var country = _countries.Where(x => x.CountryName == countryName).First();

            return country.CountryId;
        }

        #region
        private static readonly List<Country> _countries = new List<Country>
            {
                new Country
                {
                    CountryId = 74,
                    CountryName = "france"
                },
                new Country
                {
                    CountryId = 2,
                    CountryName = "albania"
                },
                new Country
                {
                    CountryId = 10,
                    CountryName = "argentina"
                }
            };
        #endregion
    }
}
