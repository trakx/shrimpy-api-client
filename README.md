![.NET Core](https://github.com/trakx/shrimpy-api-client/workflows/.NET%20Core/badge.svg)
[![Codacy Badge](https://app.codacy.com/project/badge/Grade/435670815af049dc879feaa3cfd7cc81)](https://www.codacy.com/gh/trakx/shrimpy-api-client/dashboard?utm_source=github.com&amp;utm_medium=referral&amp;utm_content=trakx/shrimpy-api-client&amp;utm_campaign=Badge_Grade) 
[![Codacy Badge](https://app.codacy.com/project/badge/Coverage/435670815af049dc879feaa3cfd7cc81)](https://www.codacy.com/gh/trakx/shrimpy-api-client/dashboard?utm_source=github.com&utm_medium=referral&utm_content=trakx/shrimpy-api-client&utm_campaign=Badge_Coverage)

# shrimpy-api-client
C# implementation of a Shrimpy api client

## Dual APIs
Shrimpy offers two distinct APIs:
- a default API, available at `https://api.shrimpy.io`, and documented at `https://dashboard.shrimpy.io/docs` which will be linked to a given account and made available to allow a user to perform operations on their accounts, like listing portfolios and rebalancing them.
- a developer one, available at `https://dev-api.shrimpy.io`, and documented at `https://developers.shrimpy.io/docs` which is built to allow developers to handle user accounts, allow for historical queries, etc.

The credentials and endpoints available for these two accounts are distinct.

## Creating your local .env file
In order to be able to run some integration tests, you should create a `.env` file in the `src` folder with the following variables:
```secretsEnvVariables
ShrimpyApiConfiguration__ApiKey=********
ShrimpyApiConfiguration__ApiSecret=********
```
