using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RouletteAPI.Mapping
{
    public class RouletteMapping : Profile
    {
        public RouletteMapping()
        {
            CreateMap<Models.Bet, Entities.Bet>().ReverseMap();
        }
        
    }
}
