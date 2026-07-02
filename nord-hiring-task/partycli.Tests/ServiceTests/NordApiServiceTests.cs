using System.Net;
using NSubstitute;
using partycli.Services;

namespace partycli.Tests.ServiceTests
{
    [TestFixture] 
    public class NordApiServiceTests
    {
        private HttpMessageHandler _httpMessageHandlerMock;
        private HttpClient _httpClient;
        private NordApiService _sut;

        [SetUp] 
        public void SetUp()
        {
            _httpMessageHandlerMock = Substitute.For<HttpMessageHandler>();
            _httpClient = new HttpClient(_httpMessageHandlerMock);
            _sut = new NordApiService(_httpClient);
        }

        [TearDown] 
        public void TearDown()
        {
            _httpClient?.Dispose();
            _httpMessageHandlerMock?.Dispose();
        }

        private void MockHttpMessageHandlerResponse(HttpResponseMessage response, Func<HttpRequestMessage, bool> requestMatcher)
        {
            _httpMessageHandlerMock
                .GetType()
                .GetMethod("SendAsync", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance)
                ?.Invoke(_httpMessageHandlerMock, new object[]
                {
                    Arg.Is<HttpRequestMessage>(req => requestMatcher(req)),
                    Arg.Any<CancellationToken>()
                })
                .Returns(Task.FromResult(response));
        }

        [Test] 
        public async Task Get_All_Servers_List_Should_Return_Expected_Json()
        {
            // Arrange
            var expectedResponse = "[{'id': 1, 'name': 'Lithuania Server'}]";
            var httpResponse = new HttpResponseMessage
            {
                StatusCode = HttpStatusCode.OK,
                Content = new StringContent(expectedResponse)
            };

            MockHttpMessageHandlerResponse(httpResponse, req => true);

            // Act
            var result = await _sut.GetAllServersListAsync();

            // Assert
            Assert.That(result, Is.EqualTo(expectedResponse)); 
        }

        [Test] 
        public async Task Get_All_Server_By_Country_List_Should_Append_Country_Id_To_Url()
        {
            // Arrange
            int countryId = 5;
            var expectedResponse = "[{'id': 5, 'country': 'UK'}]";
            var httpResponse = new HttpResponseMessage
            {
                StatusCode = HttpStatusCode.OK,
                Content = new StringContent(expectedResponse)
            };

            MockHttpMessageHandlerResponse(httpResponse, req => req.RequestUri.ToString().EndsWith(countryId.ToString()));

            // Act
            var result = await _sut.GetAllServerByCountryListAsync(countryId);

            // Assert
            Assert.That(result, Is.EqualTo(expectedResponse));
        }

        [Test] 
        public async Task Get_All_Server_By_Protocol_List_Should_Append_Protocol_To_Url()
        {
            // Arrange
            int protocolId = 2;
            var expectedResponse = "[{'id': 5, 'protocol': 'TCP'}]";
            var httpResponse = new HttpResponseMessage
            {
                StatusCode = HttpStatusCode.OK,
                Content = new StringContent(expectedResponse)
            };

            MockHttpMessageHandlerResponse(httpResponse, req => req.RequestUri.ToString().EndsWith(protocolId.ToString()));

            // Act
            var result = await _sut.GetAllServerByProtocolListAsync(protocolId);

            // Assert
            Assert.That(result, Is.EqualTo(expectedResponse));
        }
    }
}