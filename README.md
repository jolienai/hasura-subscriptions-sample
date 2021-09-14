# GraphQL Subscription using Hasura C# example

- .Net core 5
- GraphQL.Client 3.2.4
- GraphQL.Client.Serializer.Newtonsoft 3.2.4

## Documentation

<https://hasura.io/docs/latest/graphql/core/databases/postgres/subscriptions/index.html>

## subscription

See below the subscription as an example based on 2 tables vehicle and vehicle_location to get the lasted vehicle location

```graphQL
subscription getLocation($vehicleId: Int!) {
  vehicle_location(where: {vehicle_id: {_eq: $vehicleId}}, order_by: {timestamp: desc}, limit: 1) {
    id
    location
    timestamp
    vehicle {
      id
      vehicle_number
    }
  }
}
```

## How do run

1. Run Hasura using the docker compose file

```@docker-compose
docker-compose up -d
```

2. Create the tables vehicle and  vehicle_location

3. Run the C# console app

4. Add some data using the Hasura UI, should see the app is print the latest location.
