using RouletteAPI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RouletteAPI.Business
{
    public class RouletteBusiness
    {
        public string ValidateBet(Bet bet) 
        {
            string error = "";
            if (bet.Number < 0 && bet.Number > 36)
            {
                error += "Los numero permitidos son entre 0 y 36 "; 
            }
            if (bet.MoneyBet < 0 && bet.MoneyBet > 100000)
            {
                if (!string.IsNullOrEmpty(error))
                    error += "| ";
                error += "La apuesta permitida debe ser mayor a 0 y menor a 10.0000";
            }

            return error;
        }
    }
}
