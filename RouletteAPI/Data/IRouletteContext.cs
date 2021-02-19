using StackExchange.Redis;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RouletteAPI.Data
{
    public interface IRouletteContext
    {
        IDatabase Redis { get; }
    }
}
