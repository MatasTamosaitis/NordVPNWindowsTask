using partycli.Services;

namespace partycli.Tests.ServiceTests
{
    [TestFixture]
    public class CountryServiceTests
    {
        private CountryService _sut;

        [SetUp]
        public void SetUp()
        {
            _sut = new CountryService();
        }

        [TestCase("france", 74)]
        [TestCase("France", 74)]
        [TestCase("ALBANIA", 2)]
        [TestCase("argentina", 10)]
        public void Get_Country_Id_By_Name_When_Country_Exists_Should_Return_Correct_Id(string countryName, int expectedId)
        {
            // Act
            var result = _sut.GetCountryIdByName(countryName);

            // Assert
            Assert.That(result, Is.EqualTo(expectedId));
        }

        [TestCase("lithuania")]
        [TestCase("")]
        public void Get_Country_Id_By_Name_When_Country_Is_Invalid_Or_Missing_Should_Return_Minus_One(string invalidCountry)
        {
            // Act
            var result = _sut.GetCountryIdByName(invalidCountry);

            // Assert
            Assert.That(result, Is.EqualTo(-1));
        }
    }
}