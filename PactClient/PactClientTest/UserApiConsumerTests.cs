using PactClient.Pact;
using PactNet.Mocks.MockHttpService;
using PactNet.Mocks.MockHttpService.Models;
using System.Collections.Generic;
using System.Net;
using Xunit;

namespace PactClientTest
{
    public class UserApiConsumerTests : IClassFixture<ConsumerMyApiPact>
    {
        private readonly IMockProviderService _mockProviderService;
        private readonly string _mockProviderServiceBaseUri;

        public UserApiConsumerTests(ConsumerMyApiPact data)
        {
            _mockProviderService = data.MockProviderService;
            _mockProviderService.ClearInteractions();
            _mockProviderServiceBaseUri = data.MockProviderServiceBaseUri;
        }

        [Fact]
        public void GetUser_WhenTheTestUserExists_ReturnsUser()
        {
            //Arrange
            _mockProviderService
                .Given("There is a user with id 'test'")
                .UponReceiving("A GET request to retrieve user with id 'test'")
                .With(new ProviderServiceRequest
                {
                    Method = HttpVerb.Get,
                    Path = "/user/test",
                    Headers = new Dictionary<string, object>
                    {
                        { "Accept", "application/json" }
                    }
                })
                .WillRespondWith(new ProviderServiceResponse
                {
                    Status = 200,
                    Headers = new Dictionary<string, object>
                    {
                        { "Content-Type", "application/json; charset=utf-8" }
                    },
                    Body = new
                    {
                        Id = "test",
                        FirstName = "Milos",
                        LastName = "Kerkez"
                    }
                });

            var consumer = new UserApiClient(_mockProviderServiceBaseUri);

            //Act
            var result = consumer.GetUser("test");

            //Assert
            Assert.Equal("test", result.Id);

            _mockProviderService.VerifyInteractions();
        }

        [Fact]
        public void AddUser_WhenUserDataAreProvided_ReturnsStatusCreated()
        {
            //Arrange
            _mockProviderService
                .Given("Create a user with id 'test'")
                .UponReceiving("A POST request to create user with id 'test'")
                .With(new ProviderServiceRequest
                {
                    Method = HttpVerb.Post,
                    Path = "/user",
                    Headers = new Dictionary<string, object>
                    {
                        { "Content-Type", "application/json; charset=utf-8"}
                    },
                    Body = new
                    {
                        id = "test",
                        firstName = "Milos",
                        lastName = "Kerkez"
                    }
                })
                .WillRespondWith(new ProviderServiceResponse
                {
                    Status = 201
                });

            var consumer = new UserApiClient(_mockProviderServiceBaseUri);

            //Act
            var userToAdd = new User
            {
                Id = "test",
                FirstName = "Milos",
                LastName = "Kerkez"
            };
            var result = consumer.AddUser(userToAdd);

            ////Assert
            Assert.Equal(HttpStatusCode.Created, result);

            _mockProviderService.VerifyInteractions();
        }
    }
}
