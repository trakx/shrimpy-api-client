﻿using Trakx.Common.Attributes;

namespace Trakx.Shrimpy.ApiClient;

public record ShrimpyApiConfiguration
{
#nullable disable
    public string BaseUrl { get; init; }

    [AwsParameter]
    [SecretEnvironmentVariable]
    public string ApiKey { get; init; }

    [AwsParameter]
    [SecretEnvironmentVariable]
    public string ApiSecret { get; init; }

    public List<string> FavouriteExchanges { get; init; }

    [AwsParameter]
    public string FavouriteExchangesAsCsv { get; init; }
#nullable restore
}
