﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Newtonsoft.Json;
using PactNet;
using PactNet.Mocks.MockHttpService;
using PactNet.Models;

namespace PactClient.Pact
{
    public class ConsumerMyApiPact: IDisposable
    {
        public IPactBuilder PactBuilder { get; private set; }
        public IMockProviderService MockProviderService { get; private set; }

        public int MockServerPort { get { return 1111; } }
        public string MockProviderServiceBaseUri { get { return String.Format("http://localhost:{0}", MockServerPort); } }

        public ConsumerMyApiPact()
        {
            //PactBuilder = new PactBuilder(); //Defaults to specification version 1.1.0, uses default directories. PactDir: ..\..\pacts and LogDir: ..\..\logs
            //or
            //PactBuilder = new PactBuilder(new PactConfig { SpecificationVersion = "2.0.0" }); //Configures the Specification Version
            //or
            PactBuilder = new PactBuilder(new PactConfig { PactDir = @"C:\DevTech\Repos\pacts", LogDir = @"C:\DevTech\Repos\logs" }); //Configures the PactDir and/or LogDir.

            PactBuilder
                .ServiceConsumer("Consumer")
                .HasPactWith("Something API");

            MockProviderService = PactBuilder.MockService(MockServerPort); //Configure the http mock server
            //or
            //MockProviderService = PactBuilder.MockService(MockServerPort, true); //By passing true as the second param, you can enabled SSL. A self signed SSL cert will be provisioned by default.
            //or
            //MockProviderService = PactBuilder.MockService(MockServerPort, new JsonSerializerSettings()); //You can also change the default Json serialization settings using this overload    
            //or
            //MockProviderService = PactBuilder.MockService(MockServerPort, host: IPAddress.Any); //By passing host as IPAddress.Any, the mock provider service will bind and listen on all ip addresses

        }

        public void Dispose()
        {
            PactBuilder.Build(); //NOTE: Will save the pact file once finished
        }
    }
}
