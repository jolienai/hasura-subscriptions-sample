using System;
using System.Collections.Generic;
using System.Linq;
using GraphQL;
using GraphQL.Client.Http;
using GraphQL.Client.Serializer.Newtonsoft;
using Newtonsoft.Json;

namespace VehicleLocationTracker
{
    public static class Program
    {
        public static void Main(string[] args)
        {
            var client = new GraphQLHttpClient("http://localhost:8080/v1/graphql", new NewtonsoftJsonSerializer());
            
            var vehicleLocationSubscription = new GraphQLRequest
            {
                Query = @"
                subscription getLocation($vehicleId: Int!) {
                  vehicle_location(where: {vehicle_id: {_eq: $vehicleId}}, order_by: {timestamp: desc}, limit: 1) {
                    id
                    location
                    timestamp
                    vehicle_id
                  }
                }",
                Variables = new { vehicleId = 1 }
            };

            var subscriptionStream
                = client.CreateSubscriptionStream<VehicleLocationSubscriptionResult>(vehicleLocationSubscription);

            while (true)
            {
                var subscription = subscriptionStream.Subscribe(response =>
                {
                    var lastVehicleLocation = response.Data.VehicleLocation.LastOrDefault();
                    
                    if (lastVehicleLocation != null)
                        Console.WriteLine(
                            $"vehicle {lastVehicleLocation.VehicleId} " +
                            $"- location:'{lastVehicleLocation.Location}' received" +
                            $"- timestamp: {lastVehicleLocation.Timestamp}");
                    
                });
            }
        }
    }
    
    public class VehicleLocation
    {
        [JsonProperty("id")]
        public int Id { get; set; }

        [JsonProperty("location")]
        public string Location { get; set; }

        [JsonProperty("timestamp")]
        public DateTime Timestamp { get; set; }
        
        [JsonProperty("vehicle_id")]
        public int VehicleId { get; set; }
    }

    public class VehicleLocationSubscriptionResult
    {
        [JsonProperty("vehicle_location")]
        public List<VehicleLocation> VehicleLocation { get; set; }
    }
}
