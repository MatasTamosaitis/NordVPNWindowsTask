using partycli.Services;

namespace partycli.Tests.ServiceTests
{
    [TestFixture]
    public class NetworkProtocolServiceTests
    {
        private NetworkProtocolService _sut;

        [SetUp]
        public void SetUp()
        {
            _sut = new NetworkProtocolService();
        }

        [TestCase("UDP", 3)]
        [TestCase("udp", 3)]
        [TestCase("TCP", 5)]
        [TestCase("tCp", 5)]
        [TestCase("NordLynx", 35)]
        [TestCase("NORDLYNX", 35)]
        public void Get_Network_Protocol_Id_By_Name_When_Protocol_Exists_Should_Return_Correct_Id(string protocolName, int expectedId)
        {
            // Act
            var result = _sut.GetNetworkProtocolIdByName(protocolName);

            // Assert
            Assert.That(result, Is.EqualTo(expectedId));
        }

        [TestCase("OpenVPN")]
        [TestCase("")]
        public void Get_Network_Protocol_Id_By_Name_When_Protocol_Is_Invalid_Or_Missing_Should_Return_Minus_One(string invalidProtocol)
        {
            // Act
            var result = _sut.GetNetworkProtocolIdByName(invalidProtocol);

            // Assert
            Assert.That(result, Is.EqualTo(-1));
        }
    }
}