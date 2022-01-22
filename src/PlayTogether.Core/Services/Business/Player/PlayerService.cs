using Microsoft.Extensions.Logging;
using PlayTogether.Core.Dtos.Outcoming.Business.Player;
using PlayTogether.Core.Dtos.Outcoming.Generic;
using PlayTogether.Core.Interfaces.Repositories.Business.Player;
using PlayTogether.Core.Interfaces.Services.Business.Player;
using PlayTogether.Core.Parameters;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace PlayTogether.Core.Services.Business.Player
{
    public class PlayerService : IPlayerService
    {
        private readonly IPlayerRepository _playerRepository;
        private readonly ILogger<PlayerService> _logger;

        public PlayerService(IPlayerRepository PlayerRepository, ILogger<PlayerService> logger)
        {
            _playerRepository = PlayerRepository ?? throw new ArgumentNullException(nameof(PlayerRepository));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public async Task<PagedResult<PlayerResponse>> GetAllPlayersAsync(PlayerParameters param)
        {
            try {
                return await _playerRepository.GetAllPlayersAsync(param);
            }
            catch (Exception ex) {
                _logger.LogError($"Error while trying to call GetAllPlayerAsync in service class, Error Message: {ex}.");
                throw;
            }
        }
    }
}
