using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RouletteAPI.Entities
{
    public class Roulette
    {
        public string RouletteId { get; set; }
        public DateTime OpenDate {get;set;}
        public DateTime CloseDate { get; set; }
        public bool Status { get; set; }  
        public List<Bet> Bets { get; set; }

    }
}
