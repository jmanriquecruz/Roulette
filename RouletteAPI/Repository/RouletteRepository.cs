using Newtonsoft.Json;
using RouletteAPI.Data;
using RouletteAPI.Entities;
using StackExchange.Redis;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RouletteAPI.Repository
{
    public class RouletteRepository : IRouletteRepository
    {
        private readonly IRouletteContext _context;
        private readonly string HASH_KEY = "rouletteKey" ;
        public RouletteRepository(IRouletteContext context)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
        }
        public async  Task<string> CreateRoulette(Roulette roulette)
        {
            HashEntry[] redisRouletteHash = { new HashEntry(roulette.RouletteId, JsonConvert.SerializeObject(roulette)) };
            if (!_context.Redis.HashExists(HASH_KEY, roulette.RouletteId))
            {
                await _context.Redis.HashSetAsync(HASH_KEY, redisRouletteHash);
               
                return roulette.RouletteId;
            }

            return string.Empty;
        }

        public async Task<Dictionary<string,string>> ListRoulette()
        {
            HashEntry[] redisRouletteHash =  await _context.Redis.HashGetAllAsync(HASH_KEY);
            Roulette roulette = new Roulette();
            Dictionary<string, string> result = new Dictionary<string, string>(); 
            foreach (HashEntry item in redisRouletteHash)
            {
                roulette = JsonConvert.DeserializeObject<Roulette>(item.Value);
                result.Add(roulette.RouletteId, roulette.Status ? "ABIERTA" : "CERRADA");
            }

            return result;
        }

        public async Task<bool> BetRoulette( string rouletteId, Bet bet)
        {
            var redisRouletteHash = await _context.Redis.HashGetAsync(HASH_KEY, rouletteId); 
            if (!redisRouletteHash.IsNullOrEmpty)
            {
                Roulette roulette = new Roulette();
                roulette = JsonConvert.DeserializeObject<Roulette>(redisRouletteHash);
                if (roulette.Status)
                {
                    (roulette.Bets ??= new List<Bet>()).Add(bet);
                    await _context.Redis.HashSetAsync(HASH_KEY, rouletteId, JsonConvert.SerializeObject(roulette));

                    return true;
                }
            }

            return false;
        }
        public async Task<bool> OpenRoulette(string rouletteId)
        {
            var redisRouletteHash = await _context.Redis.HashGetAsync(HASH_KEY, rouletteId);
            if (!redisRouletteHash.IsNullOrEmpty)
            {
                Roulette roulette = new Roulette();
                roulette = JsonConvert.DeserializeObject<Roulette>(redisRouletteHash);
                roulette.OpenDate = DateTime.Now;
                roulette.Status = true;
                await _context.Redis.HashSetAsync(HASH_KEY, rouletteId, JsonConvert.SerializeObject(roulette));

                return true;
            }

            return false;
        }

        public async Task<Roulette> GetRoulette(string rouletteId)
        {
            var redisRouletteHash = await _context.Redis.HashGetAsync(HASH_KEY, rouletteId);
            if (!redisRouletteHash.IsNullOrEmpty)
            {
                Roulette roulette = new Roulette();
                roulette = JsonConvert.DeserializeObject<Roulette>(redisRouletteHash);
                roulette.Bets?.Clear();

                return roulette;
            }

            return null;

        }

        public async Task<Lottery> CloseRoulette(string rouletteId)
        {
            Lottery lottery = null;
            var redisRouletteHash = await _context.Redis.HashGetAsync(HASH_KEY, rouletteId);
            if (!redisRouletteHash.IsNullOrEmpty)
            {
                Roulette roulette = new Roulette();
                roulette = JsonConvert.DeserializeObject<Roulette>(redisRouletteHash);
                if (roulette.Status)
                {
                    lottery = GetLottery(roulette);
                    roulette.OpenDate = DateTime.Now;
                    roulette.Status = false;
                    await _context.Redis.HashSetAsync(HASH_KEY, rouletteId, JsonConvert.SerializeObject(roulette));
                }

                return lottery;
            }

            return null;
        }

        private Lottery GetLottery(Roulette roulette)
        {

            List<int> participants = roulette.Bets.Select(u => u.Number).Distinct().ToList();
            Random r = new Random();
            int rInt = r.Next(0, participants.Count());
            Lottery lottery = new Lottery();
            lottery.RegisterDate = DateTime.Now;
            lottery.Close = roulette.OpenDate;
            lottery.Close = roulette.CloseDate;
            lottery.RouletteId = roulette.RouletteId;
            lottery.TotalBet = roulette.Bets.Sum(u => u.MoneyBet);
            lottery.WinningNumber = participants[rInt];
            lottery.WinnersQuantityNumber = roulette.Bets.Where(u => u.Number == lottery.WinningNumber).Count();
            lottery.WinnersNumber = GetWinners(roulette.Bets.Where(u => u.Number == lottery.WinningNumber).ToList(), 5);
            if (lottery.WinningNumber % 2 == 0)
            {
                lottery.TotalMoneyBetColor =  roulette.Bets.Where(u => u.Number % 2 == 0).Sum(u => u.MoneyBet);
                lottery.WinnersQuantityColor = roulette.Bets.Where(u => u.Number % 2 == 0).Count();
                lottery.WinnersColor = GetWinners(roulette.Bets.Where(u => u.Number % 2 == 0).ToList(),1.8);

            }
            else
            {
                lottery.WinnersQuantityColor = roulette.Bets.Where(u => u.Number % 2 != 0).Count();
                lottery.TotalMoneyBetNumber = roulette.Bets.Where(u => u.Number % 2 != 0).Sum(u => u.MoneyBet);
                lottery.WinnersColor = GetWinners(roulette.Bets.Where(u => u.Number % 2 != 0).ToList(), 1.8);
            }
                
            return lottery;
        }

        private List<Winner> GetWinners(List<Bet> bets, double factor)
        {
            List<Winner> winners = new List<Winner>();
            foreach (var item in bets)
            {
                Winner winner = new Winner();
                winner.UserId = item.UserId;
                winner.Number = item.Number;
                winner.EarnedMoney = item.MoneyBet * factor;
                winner.MoneyBet = item.MoneyBet;
                winners.Add(winner);
            }

            return winners;
        }

       
    }
}
