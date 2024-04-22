# Super Hero API
Back-end Web API that supports CRUD operations to manage Super Heroes and their Powers.  Sample Hero data point looks like this:

```json
{
  "id": 2,
  "name": "Batman",
  "firstName": "Bruce",
  "lastName": "Wayne",
  "description": "He is the night.",
  "place": "Gotham City",
  "powers": {
    "values": [1,2,3]
  }
}
```

Sample Power looks like this:

```json
{
  "id": 8,
  "tag": "Flight",
  "description": "Ability to fly!"
}
```

# Endpoints

## Power
<code>GET</code> <code>/api/Power</code> Returns all powers.

<code>POST</code> <code>/api/Power</code> Creates a power.

<code>PUT</code> <code>/api/Power</code> Updates a power.

<code>GET</code> <code>/api/Power/:id</code> Get specific power.

<code>DELETE</code> <code>/api/Power/:id</code> Delete specific power.

## SuperHero

<code>GET</code> <code>/api/SuperHero</code> Returns all superheroes.

<code>POST</code> <code>/api/SuperHero</code> Creates a superhero.

<code>PUT</code> <code>/api/SuperHero</code> Updates a superhero.

<code>GET</code> <code>/api/SuperHero/:id</code> Get specific superhero.

<code>DELETE</code> <code>/api/SuperHero/:id</code> Delete specific superhero.

Pleae refer to swagger documentation for more detailed use of the API.

# Tests

Unit testing for both SuperHero and Power controllers can be found in Tests folder.

# Local Installation

1. Clone repo.
2. Configure MySQL database settings in ```appsettings.json```.
1. Start application.

# Tech Stack  

* .Net 8.0
* EF Core
* NUnit
* Moq
* Swagger (OpenAPI)
* MySQL

# License

[![License: MIT](https://img.shields.io/badge/License-MIT-yellow.svg)](https://opensource.org/licenses/MIT)