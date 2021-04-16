using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using RouletteAPI.Business;
using RouletteAPI.Entities;
using RouletteAPI.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace RouletteAPI.Controllers
{
    [Route("api/v1/[controller]")]
    [ApiController]
    public class RouletteController : Controller
    {
        private readonly IRouletteRepository _repository;
        private readonly ILogger<RouletteController> _logger;
        private readonly IMapper _mapper;

        public RouletteController(IRouletteRepository repository, IMapper mapper, ILogger<RouletteController> logger)
        {
            _repository = repository ?? throw new ArgumentNullException(nameof(repository));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        [Route("[action]")]
        [HttpPost]
        public async Task<ActionResult<string>> Create()
        {
            Roulette roulette = new Roulette();
            roulette.RouletteId = Guid.NewGuid().ToString();

            return Ok(await _repository.CreateRoulette(roulette));
        }

        [Route("[action]")]
        [HttpPost]
        public async Task<ActionResult> Open(string rouletteId)
        {
           
            return Ok(await _repository.OpenRoulette(rouletteId));
        }


        [Route("[action]")]
        [HttpPost]
        public async Task<ActionResult> Close(string rouletteId)
        {
            var roulette = await _repository.GetRoulette(rouletteId);
            if (roulette != null)
            {
                if (roulette.Status)
                {
                    return Ok(await _repository.CloseRoulette(rouletteId));
                }
                else
                {
                    return BadRequest("Error ruleta Cerrada");
                }
            }
            else
            {
                return BadRequest("Identificador de ruleta no existe");
            }
           
        }

        [Route("[action]")]
        [HttpGet]
        public async Task<ActionResult> List()
        {
            return Ok(await _repository.ListRoulette());
        }


        [Route("[action]")]
        [HttpPost]
        public async Task<ActionResult> Bet([FromBody] Models.Bet bet)
        {    
            if (ModelState.IsValid)
            {
                var roulette = await _repository.GetRoulette(bet.RouletteId);
                if (roulette != null)
                {
                    if (roulette.Status)
                    {
                        var result = _mapper.Map<Entities.Bet>(bet);
                        return Ok(await _repository.BetRoulette(bet.RouletteId, result));
                    }
                    else
                    {
                        return BadRequest("Error ruleta Cerrada");
                    }
                }
                else
                {
                    return BadRequest("Identificador de ruleta no existe");
                }
                
            }
           
            return BadRequest(ModelState);
        }


    }
}
