using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Models.Business;
using Models.Repositories.Candles;
using Robo.Database.Context;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Models.Repositories.Candle
{
    public class CandleRepository : ICandleRepository
    {
        private readonly TickerContext _context;
        private readonly IMapper _mapper;
        private readonly ILogger<CandleRepository> _logger;

        public CandleRepository(TickerContext context, IMapper mapper, ILogger<CandleRepository> logger)
        {
            _context = context;
            _mapper = mapper;
            _logger = logger;
        }

        public async Task AddCandleAsync(CandleModel candle)
        {
            try
            {
                await _context.Candles.AddAsync(_mapper.Map<Robo.Database.Models.Candles>(candle));
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException ex)
            {
                _logger.LogError(ex, $"При сохранении новой свечи произошла ошибка:{Environment.NewLine}{ex.Message}");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"При добавлении новой свечи произошла ошибка:{Environment.NewLine}{ex.Message}");
            }
        }

        public async Task AddRangeCandlesAsync(IEnumerable<CandleModel> candles)
        {
            try
            {
                await _context.Candles.AddRangeAsync(_mapper.Map<IEnumerable<Robo.Database.Models.Candles>>(candles));
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException ex)
            {
                _logger.LogError(ex, $"При сохранении новой свечи произошла ошибка:{Environment.NewLine}{ex.Message}");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"При добавлении новой свечи произошла ошибка:{Environment.NewLine}{ex.Message}");
            }
        }
    }
}
