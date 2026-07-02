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
            if (string.IsNullOrWhiteSpace(countryName))
            {
                return -1;
            }

            var country = _countries.FirstOrDefault(x =>
                x.CountryName.Equals(countryName, StringComparison.OrdinalIgnoreCase));

            return country != null ? country.CountryId : -1;
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
