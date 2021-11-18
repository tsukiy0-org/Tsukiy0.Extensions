using System;

using Tsukiy0.Extensions.Configuration.Aws.Models;

namespace Tsukiy0.Extensions.Configuration.Aws.Exceptions
{
    public class SsmParameterNotFoundException : Exception
    {
        public SsmParameterNotFoundException(SsmParameterMap map) : base($"Missing parameter with key {map.ParameterKey}")
        {
            Data.Add(nameof(map), map);
        }
    }
}