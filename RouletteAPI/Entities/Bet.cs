using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RouletteAPI.Entities
{
    public class Bet
    {
        public int UserId { get; set; }
        public int Number { get; set; }
        public int MoneyBet { get; set; }
    }
}
