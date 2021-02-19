using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using RouletteAPI.Entities;
namespace RouletteAPI.Repository
{
    public interface IRouletteRepository
    {
        Task<string> CreateRoulette(Roulette roulette);
        Task<bool> OpenRoulette(string rouletteId);
        Task<bool> BetRoulette(string rouletteId,Bet roulette);
        Task<Lottery> CloseRoulette(string rouletteId);
        Task<Dictionary<string, string>> ListRoulette();
        Task<Roulette> GetRoulette(string rouletteId);


    }
}
