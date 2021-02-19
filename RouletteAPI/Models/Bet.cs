using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RouletteAPI.Models
{
    public class Bet
    {
        public string RouletteId { get; set; }
        public int UserId { get; set; }
        public int Number { get; set; }
        public decimal MoneyBet { get; set; }
    }
}
