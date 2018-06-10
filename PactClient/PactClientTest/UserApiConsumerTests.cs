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
            _mockProviderServiceBaseUri = data.MockProviderServiceBaseUri;
            _mockProviderService.ClearInteractions();
        }

        [Fact]
        public void UserGetAddDelete()
        {
            //Arrange
            SetArrangeForGetUser();
            SetArrangeForAddUser();
            SetArrangeForDeleteUser();

            var consumer = new UserApiClient(_mockProviderServiceBaseUri);

            //Act
            var getUserResult = consumer.GetUser("test");
            var userToAdd = new User
            {
                Id = "test",
                FirstName = "Milos",
                LastName = "Kerkez"
            };
            var addUserResult = consumer.AddUser(userToAdd);
            var deleteUserResult = consumer.DeleteUser("test");

            //Assert
            Assert.Equal("test", getUserResult.Id);
            Assert.Equal(HttpStatusCode.Created, addUserResult);
            Assert.Equal(HttpStatusCode.OK, deleteUserResult);

            _mockProviderService.VerifyInteractions();
        }

        private void SetArrangeForDeleteUser()
        {
            const string userId = "test";
            _mockProviderService
                .Given("Delete a user with id 'test'")
                .UponReceiving("A DELETE request to remove user with id 'test' from database")
                .With(new ProviderServiceRequest
                {
                    Method = HttpVerb.Delete,
                    Path = "/user/" + userId
                })
                .WillRespondWith(new ProviderServiceResponse
                {
                    Status = 200
                });
        }

        private void SetArrangeForAddUser()
        {
            _mockProviderService
                .Given("Create a user with id 'test'")
                .UponReceiving("A POST request to create user with id 'test'")
                .With(new ProviderServiceRequest
                {
                    Method = HttpVerb.Post,
                    Path = "/user",
                    Headers = new Dictionary<string, object>
                    {
                        {"Content-Type", "application/json; charset=utf-8"}
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
        }

        private void SetArrangeForGetUser()
        {
            _mockProviderService
                .Given("There is a user with id 'test'")
                .UponReceiving("A GET request to retrieve user with id 'test'")
                .With(new ProviderServiceRequest
                {
                    Method = HttpVerb.Get,
                    Path = "/user/test",
                    Headers = new Dictionary<string, object>
                    {
                        {"Accept", "application/json"}
                    }
                })
                .WillRespondWith(new ProviderServiceResponse
                {
                    Status = 200,
                    Headers = new Dictionary<string, object>
                    {
                        {"Content-Type", "application/json; charset=utf-8"}
                    },
                    Body = new
                    {
                        Id = "test",
                        FirstName = "Milos",
                        LastName = "Kerkez"
                    }
                });
            
        }
    }
}
