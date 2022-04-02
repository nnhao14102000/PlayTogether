using AutoMapper;
using Microsoft.EntityFrameworkCore;
using PlayTogether.Core.Dtos.Outcoming.Business.Recommend;
using PlayTogether.Core.Interfaces.Repositories.Business;
using PlayTogether.Infrastructure.Data;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace PlayTogether.Infrastructure.Repositories.Business.Recommend
{
    public class RecommendRepository : BaseRepository, IRecommendRepository
    {
        public RecommendRepository(IMapper mapper, AppDbContext context) : base(mapper, context)
        {
        }

        public async Task<bool> TrainModel()
        {
            // Environment.CurrentDirectory
            var path = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "DataFile", "Recommend_Data.txt");

            var path_directory = Path.Combine(AppDomain.CurrentDomain
            .BaseDirectory, "DataFile");


            var listRecommend = await _context.Recommends.OrderBy(x => x.CreatedDate).ToListAsync();
            var listAppend = new List<RecommendSerialize>();

            if (!File.Exists(path)) {

                for (int i = 0; i < listRecommend.Count() - 1; i++) {
                    var objRecommend = new RecommendSerialize {
                        UserId = listRecommend[i].UserId,
                        UserAge = listRecommend[i].UserAge,
                        UserGender = listRecommend[i].UserGender,
                        GameOrderId = listRecommend[i].GameOrderId,
                        PlayerId = listRecommend[i].PlayerId,
                        PlayerAge = listRecommend[i].PlayerAge,
                        PlayerGender = listRecommend[i].PlayerGender,
                        GameOfPlayerId = listRecommend[i].GameOfPlayerId,
                        Rate = listRecommend[i].Rate
                    };
                    listAppend.Add(objRecommend);
                }
                using (FileStream fs = File.Create(path))

                using (var tw = new StreamWriter(path)) {
                    foreach (var item in listAppend) {
                        tw.WriteLine(item);
                    }
                }
                return true;
            }
            else if (File.Exists(path)) {
                var logFile = File.ReadAllLines(path);
                var logList = new List<string>(logFile);

                if (logList.Count() == 0) {
                    for (int i = 0; i < listRecommend.Count() - 1; i++) {
                    var objRecommend = new RecommendSerialize {
                        UserId = listRecommend[i].UserId,
                        UserAge = listRecommend[i].UserAge,
                        UserGender = listRecommend[i].UserGender,
                        GameOrderId = listRecommend[i].GameOrderId,
                        PlayerId = listRecommend[i].PlayerId,
                        PlayerAge = listRecommend[i].PlayerAge,
                        PlayerGender = listRecommend[i].PlayerGender,
                        GameOfPlayerId = listRecommend[i].GameOfPlayerId,
                        Rate = listRecommend[i].Rate
                    };
                    listAppend.Add(objRecommend);
                }
                }
                else {
                    if (logList.Count() <= listRecommend.Count()) {
                        var countFile = logList.Count();
                        for (int i = (listRecommend.Count() - (logList.Count() + 1)); i < listRecommend.Count() - 1; i++) {
                            var objRecommend = new RecommendSerialize {
                                UserId = listRecommend[i].UserId,
                                UserAge = listRecommend[i].UserAge,
                                UserGender = listRecommend[i].UserGender,
                                GameOrderId = listRecommend[i].GameOrderId,
                                PlayerId = listRecommend[i].PlayerId,
                                PlayerAge = listRecommend[i].PlayerAge,
                                PlayerGender = listRecommend[i].PlayerGender,
                                GameOfPlayerId = listRecommend[i].GameOfPlayerId,
                                Rate = listRecommend[i].Rate
                            };
                            listAppend.Add(objRecommend);
                        }
                    }
                }

                using (var tw = new StreamWriter(path, true)) {
                    foreach (var item in listAppend) {
                        tw.WriteLine(item);
                    }
                }
                return true;
            }
            return false;
        }


    }
}