﻿using System;
using System.Net;
using System.Net.Http;
using Newtonsoft.Json;

namespace PactClient.Pact
{
    public class UserApiClient
    {
        public string BaseUri { get; set; }

        public UserApiClient(string baseUri = null)
        {
            BaseUri = baseUri ?? "http://localhost:1111";
        }

        public User GetUser(string id)
        {
            string reasonPhrase;

            using (var client = new HttpClient {BaseAddress = new Uri(BaseUri)})
            {
                var request = new HttpRequestMessage(HttpMethod.Get, "/user/" + id);
                request.Headers.Add("Accept", "application/json");

                var response = client.SendAsync(request);

                var content = response.Result.Content.ReadAsStringAsync().Result;
                var status = response.Result.StatusCode;

                reasonPhrase =
                    response.Result
                        .ReasonPhrase; //NOTE: any Pact mock provider errors will be returned here and in the response body

                request.Dispose();
                response.Dispose();

                if (status == HttpStatusCode.OK)
                {
                    return !String.IsNullOrEmpty(content)
                        ? JsonConvert.DeserializeObject<User>(content)
                        : null;
                }
            }

            throw new Exception(reasonPhrase);
        }
    }

}