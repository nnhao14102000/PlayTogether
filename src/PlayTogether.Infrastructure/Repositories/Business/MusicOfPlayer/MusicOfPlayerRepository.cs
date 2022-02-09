using AutoMapper;
using Microsoft.EntityFrameworkCore;
using PlayTogether.Core.Dtos.Incoming.Business.MusicOfPlayer;
using PlayTogether.Core.Dtos.Outcoming.Business.MusicOfPlayer;
using PlayTogether.Core.Interfaces.Repositories.Business;
using PlayTogether.Infrastructure.Data;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PlayTogether.Infrastructure.Repositories.Business.MusicOfPlayer
{
    public class MusicOfPlayerRepository : BaseRepository, IMusicOfPlayerRepository
    {
        public MusicOfPlayerRepository(
            IMapper mapper,
            AppDbContext context) : base(mapper, context)
        {
        }

        public async Task<MusicOfPlayerGetByIdResponse> CreateMusicOfPlayerAsync(
            string playerId,
            MusicOfPlayerCreateRequest request)
        {
            var player = await _context.Players.FindAsync(playerId);
            if (player is null) {
                return null;
            }

            var music = await _context.Musics.FindAsync(request.MusicId);
            if (music is null) {
                return null;
            }

            var musicExist = await _context.MusicOfPlayers.Where(x => x.PlayerId == playerId)
                                                          .AnyAsync(x => x.MusicId == request.MusicId);
            if (musicExist) {
                return null;
            }

            var model = _mapper.Map<Entities.MusicOfPlayer>(request);
            model.PlayerId = playerId;

            await _context.MusicOfPlayers.AddAsync(model);
            if ((await _context.SaveChangesAsync() >= 0)) {
                return _mapper.Map<MusicOfPlayerGetByIdResponse>(model);
            }
            return null;
        }

        public async Task<bool> DeleteMusicOfPlayerAsync(string id)
        {
            var musicOfPlayer = await _context.MusicOfPlayers.FindAsync(id);
            if (musicOfPlayer is null) {
                return false;
            }
            _context.MusicOfPlayers.Remove(musicOfPlayer);
            return (await _context.SaveChangesAsync() >= 0);
        }

        public async Task<IEnumerable<MusicOfPlayerGetAllResponse>> GetALlMusicOfPlayerAsync(string playerId)
        {
            var player = await _context.Players.FindAsync(playerId);
            if (player is null) {
                return null;
            }

            var musicOfPlayer = await _context.MusicOfPlayers.Where(x => x.PlayerId == playerId)
                                                             .OrderByDescending(x => x.CreatedDate)
                                                             .ToListAsync();

            foreach (var item in musicOfPlayer) {
                await _context.Entry(item).Reference(x => x.Music).Query().LoadAsync();
            }

            return _mapper.Map<IEnumerable<MusicOfPlayerGetAllResponse>>(musicOfPlayer);
        }

        public async Task<MusicOfPlayerGetByIdResponse> GetMusicOfPlayerByIdAsync(string id)
        {
            var musicOfPlayer = await _context.MusicOfPlayers.FindAsync(id);
            if (musicOfPlayer is null) {
                return null;
            }
            await _context.Entry(musicOfPlayer)
                          .Reference(x => x.Music)
                          .Query()
                          .LoadAsync();
            return _mapper.Map<MusicOfPlayerGetByIdResponse>(musicOfPlayer);
        }
    }
}