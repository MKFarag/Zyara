﻿namespace Domain.Settings;

public sealed class RateLimitingOptions
{
    public partial class PolicyNames
    {
        public const string IpLimit = "ipLimit";
        public const string UserLimit = "userLimit";
        public const string Fixed = "fixed";
    }
    public PolicyOptions IpPolicy { get; init; } = default!;
    public PolicyOptions UserPolicy { get; init; } = default!;
    public FixedWindowOptions FixedWindow { get; init; } = default!;
}

#region RateLimiters

public class PolicyOptions
{
    [Required]
    public int PermitLimit { get; init; }

    [Required]
    public int WindowInSeconds { get; init; }

    public int QueueLimit { get; init; }
}

public class FixedWindowOptions : PolicyOptions { }

#endregion