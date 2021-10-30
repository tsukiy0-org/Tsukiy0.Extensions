using Tsukiy0.Extensions.Data.Aws.Models;

namespace Tsukiy0.Extensions.Data.Aws.Extensions
{
    public static class DynamoKeyExtensions
    {
        public static IDynamoPrimaryKey ToPrimaryKey(this DynamoKey key)
        {
            return new PrimaryKey(key.PK, key.SK);
        }

        public static IDynamoGsi1Key ToGsi1(this DynamoKey key)
        {
            return new Gsi1(key.PK, key.SK);
        }

        public static IDynamoGsi2Key ToGsi2(this DynamoKey key)
        {
            return new Gsi2(key.PK, key.SK);
        }

        public static IDynamoGsi3Key ToGsi3(this DynamoKey key)
        {
            return new Gsi3(key.PK, key.SK);
        }

        private record PrimaryKey(string __PK, string __SK) : IDynamoPrimaryKey;
        private record Gsi1(string __GSI1_PK, string __GSI1_SK) : IDynamoGsi1Key;
        private record Gsi2(string __GSI2_PK, string __GSI2_SK) : IDynamoGsi2Key;
        private record Gsi3(string __GSI3_PK, string __GSI3_SK) : IDynamoGsi3Key;

    }

}
