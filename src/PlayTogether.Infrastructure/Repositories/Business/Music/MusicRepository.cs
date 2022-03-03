using AutoMapper;
using Microsoft.EntityFrameworkCore;
using PlayTogether.Core.Dtos.Incoming.Business.Music;
using PlayTogether.Core.Dtos.Outcoming.Business.Music;
using PlayTogether.Core.Dtos.Outcoming.Generic;
using PlayTogether.Core.Interfaces.Repositories.Business;
using PlayTogether.Core.Parameters;
using PlayTogether.Infrastructure.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PlayTogether.Infrastructure.Repositories.Business.Music
{
    public class MusicRepository : BaseRepository, IMusicRepository
    {
        public MusicRepository(IMapper mapper, AppDbContext context) : base(mapper, context)
        {
        }

        public async Task<MusicGetByIdResponse> CreateMusicAsync(MusicCreateRequest request)
        {
            var model = _mapper.Map<Entities.Music>(request);
            await _context.Musics.AddAsync(model);
            if ((await _context.SaveChangesAsync() >= 0)) {
                return _mapper.Map<MusicGetByIdResponse>(model);
            }
            return null;
        }

        public async Task<bool> DeleteMusicAsync(string id)
        {
            var music = await _context.Musics.FindAsync(id);
            if (music is null) {
                return false;
            }

            var musicOfPlayers = await _context.MusicOfPlayers.Where(x => x.MusicId == id).ToListAsync();
            if(musicOfPlayers.Count >= 0){
                _context.MusicOfPlayers.RemoveRange(musicOfPlayers);
                if(await _context.SaveChangesAsync() < 0){
                    return false;
                }
            }
            
            _context.Musics.Remove(music);
            return (await _context.SaveChangesAsync() >= 0);
        }

        public async Task<PagedResult<MusicGetByIdResponse>> GetAllMusicsAsync(MusicParameter param)
        {
            var musics = await _context.Musics.OrderByDescending(x => x.CreatedDate).ToListAsync();

            if (musics is not null) {
                if (!String.IsNullOrEmpty(param.Name)) {
                    var query = musics.AsQueryable();
                    query = query.Where(x => (x.Name).ToLower()
                                                   .Contains(param.Name.ToLower())).OrderByDescending(x => x.CreatedDate);
                    musics = query.ToList();
                }
                var response = _mapper.Map<List<MusicGetByIdResponse>>(musics);
                return PagedResult<MusicGetByIdResponse>.ToPagedList(response, param.PageNumber, param.PageSize);
            }

            return null;
        }

        public async Task<MusicGetByIdResponse> GetMusicByIdAsync(string id)
        {
            var music = await _context.Musics.FindAsync(id);

            if (music is null) {
                return null;
            }

            return _mapper.Map<MusicGetByIdResponse>(music);
        }

        public async Task<bool> UpdateMusicAsync(string id, MusicUpdateRequest request)
        {
            var music = await _context.Musics.FindAsync(id);

            if (music is null) {
                return false;
            }

            var model = _mapper.Map(request, music);
            _context.Musics.Update(model);
            return (await _context.SaveChangesAsync() >= 0);
        }
    }
}