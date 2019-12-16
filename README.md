# Redis Example App

Tested with .NET Core 2.2

## Deploy
- Create a Redis service instance: `cf create-service redis plan my-redis`
- Deploy application: `cf push redis-example --random-route`
- Bind the Redis service to the application: `cf bind-service redis-example my-redis`
- Restage the application: `cf restage redis-example`


## Usage
- Return a cached datetime: `redis-example-url.com/api/values`
- Remove the datetime from cache: `redis-example-url.com/api/values/reset`