using PactNet;
using PactNet.Mocks.MockHttpService;
using System;
using PactNet.Models;

namespace PactClient.Pact
{
    public class ConsumerMyApiPact: IDisposable
    {
        public IPactBuilder PactBuilder { get; }
        public IMockProviderService MockProviderService { get; }

        public int MockServerPort => 1111;
        public string MockProviderServiceBaseUri => string.Format("http://localhost:{0}", MockServerPort);

        public ConsumerMyApiPact()
        {
            PactBuilder = new PactBuilder(new PactConfig
            {
                PactDir = @"C:\DevTech\Repos\pacts", 
                LogDir = @"C:\DevTech\Repos\logs" 
                
            });

            PactBuilder
                .ServiceConsumer("Consumer")
                .HasPactWith("User API");

            MockProviderService = PactBuilder.MockService(MockServerPort);
        }

        public void Dispose()
        {
            PactBuilder.Build(); 
        }
    }
}
