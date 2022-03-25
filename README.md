![.NET Core](https://github.com/trakx/shrimpy-api-client/workflows/.NET%20Core/badge.svg)
[![Codacy Badge](https://app.codacy.com/project/badge/Grade/435670815af049dc879feaa3cfd7cc81)](https://www.codacy.com/gh/trakx/shrimpy-api-client/dashboard?utm_source=github.com&amp;utm_medium=referral&amp;utm_content=trakx/shrimpy-api-client&amp;utm_campaign=Badge_Grade) 
[![Codacy Badge](https://app.codacy.com/project/badge/Coverage/435670815af049dc879feaa3cfd7cc81)](https://www.codacy.com/gh/trakx/shrimpy-api-client/dashboard?utm_source=github.com&utm_medium=referral&utm_content=trakx/shrimpy-api-client&utm_campaign=Badge_Coverage)

# shrimpy-api-client
C# implementation of a Shrimpy api client

## Creating your local .env file
In order to be able to run some integration tests, you should create a `.env` file in the `src` folder with the following variables:
```secretsEnvVariables
ShrimpyApiConfiguration__ApiKey=********
ShrimpyApiConfiguration__ApiSecret=********
```

## AWS Parameters
In order to be able to run some integration tests you should ensure that you have access to the following AWS parameters :
```awsParams
/Trakx/Shrimpy/ApiClient/ShrimpyApiConfiguration/ApiKey
/Trakx/Shrimpy/ApiClient/ShrimpyApiConfiguration/ApiSecret
```

## How to regenerate C# API clients

-   If you work with external API, you probably need to update OpenAPI definition added to the project. It's usually openApi3.yaml file.
-   Do right click on the project and select Edit Project File. In the file change value of `GenerateApiClient` property to true.
-   Rebuild the project. `NSwag` target will be executed as post action.
-   The last thing to be done is to run integration test `OpenApiGeneratedCodeModifier` that will rewrite auto generated C# classes to use C# 9 features like records and init keyword.
