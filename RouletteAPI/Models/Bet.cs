using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace RouletteAPI.Models
{
    public class Bet
    {
        public string RouletteId { get; set; }
        public int UserId { get; set; }
        [Range(1,36,ErrorMessage = "Los numeros permitidos estan entre  1 y asdasdasd")]
        public int Number { get; set; }
        [Range(1,10000, ErrorMessage ="El monto apostado debe estar entre 1 y 10.asdasdasdasd"),]
        public decimal MoneyBet { get; set; }
    }
}
