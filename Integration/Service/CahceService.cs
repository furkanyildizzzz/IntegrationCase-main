using StackExchange.Redis;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace Integration.Service
{
    public  class CacheService
    {
        private readonly ConnectionMultiplexer _client;

        public CacheService()
        {
            ConfigurationOptions options = new ConfigurationOptions
            {
                SyncTimeout = 500000,
                EndPoints =
                {
                    { "redis-14448.c100.us-east-1-4.ec2.redns.redis-cloud.com", 14448 }
                },
                AbortOnConnectFail = true,
                ConnectTimeout = 10000,
                ResolveDns = true,
                User = "default",
                Password = "a63XZ6Bf19Q04mFSoy09kpPB8mHJ6u2a",

            };
            _client = ConnectionMultiplexer.Connect(options);

        }

        public IDatabase GetDatabase()
        {
            return _client.GetDatabase();
        }

        public bool LockTake(string key)
        {
            var lockKey = $"lock:{key}";
            var lockToken = GenerateTokenFromKey(key);
            return _client.GetDatabase().LockTake(lockKey, lockToken, TimeSpan.FromSeconds(10));
        }

        public bool LockRelease(string key)
        {
            var lockKey = $"lock:{key}";
            var lockToken = GenerateTokenFromKey(key);
            return _client.GetDatabase().LockRelease(lockKey, lockToken);
        }

        private string GenerateTokenFromKey(string key)
        {
            using (var sha256 = SHA256.Create())
            {
                var keyBytes = Encoding.UTF8.GetBytes(key);
                var hashBytes = sha256.ComputeHash(keyBytes);
                return BitConverter.ToString(hashBytes).Replace("-", "").ToLower();
            }
        }
    }
}
