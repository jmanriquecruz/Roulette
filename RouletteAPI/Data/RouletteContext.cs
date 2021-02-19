using StackExchange.Redis;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RouletteAPI.Data
{
    public class RouletteContext : IRouletteContext
    {
        private readonly ConnectionMultiplexer _redisConnection;
        public RouletteContext(ConnectionMultiplexer redisConnection)
        {
            _redisConnection = redisConnection;
            Redis = redisConnection.GetDatabase();
        }

        public IDatabase Redis { get; }
    }
}
