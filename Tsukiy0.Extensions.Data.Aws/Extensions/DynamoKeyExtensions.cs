using Tsukiy0.Extensions.Data.Aws.Models;

namespace Tsukiy0.Extensions.Data.Aws.Extensions
{
    public static class DynamoKeyExtensions
    {
        public static IDynamoPrimaryKey ToPrimaryKey(this DynamoKey key)
        {
            return new PrimaryKey { __PK = key.PK, __SK = key.SK };
        }

        public static IDynamoGsi1Key ToGsi1(this DynamoKey key)
        {
            return new Gsi1 { __GSI1_PK = key.PK, __GSI1_SK = key.SK };
        }

        public static IDynamoGsi2Key ToGsi2(this DynamoKey key)
        {
            return new Gsi2 { __GSI2_PK = key.PK, __GSI2_SK = key.SK };
        }

        public static IDynamoGsi3Key ToGsi3(this DynamoKey key)
        {
            return new Gsi3 { __GSI3_PK = key.PK, __GSI3_SK = key.SK };
        }

        private record PrimaryKey : IDynamoPrimaryKey
        {
            public string __PK { get; init; }
            public string __SK { get; init; }
        };

        private record Gsi1 : IDynamoGsi1Key
        {
            public string __GSI1_PK { get; init; }
            public string __GSI1_SK { get; init; }
        }
        private record Gsi2 : IDynamoGsi2Key
        {
            public string __GSI2_PK { get; init; }
            public string __GSI2_SK { get; init; }
        }

        private record Gsi3 : IDynamoGsi3Key
        {
            public string __GSI3_PK { get; set; }
            public string __GSI3_SK { get; set; }
        }
    }
}
