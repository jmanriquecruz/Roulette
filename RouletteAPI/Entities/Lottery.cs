using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RouletteAPI.Entities
{
    public class Lottery
    {
        public DateTime RegisterDate{ get; set; }
        public string RouletteId { get; set; }
        public DateTime Open { get; set; }
        public DateTime Close { get; set; }
        public long TotalBet { get; set; }
        public int WinningNumber { get; set; }
        public int WinnersQuantityColor { get; set; }
        public int WinnersQuantityNumber { get; set; }
        public int TotalMoneyBetNumber { get; set; }
        public int TotalMoneyBetColor { get; set; }
        public List<Winner> WinnersNumber { get; set; }
        public List<Winner> WinnersColor { get; set; }
    }
}
