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

        public async Task<bool> WriteToFile()
        {
            // Environment.CurrentDirectory
            // AppContext.BaseDirectory
            // AppDomain.CurrentDomain.BaseDirectory
            var path = Path.Combine(Environment.CurrentDirectory, "DataFile", "Recommend_Data.txt");
            var pathDirectory = Path.Combine(Environment.CurrentDirectory, "DataFile");


            var listRecommend = await _context.Recommends.OrderBy(x => x.CreatedDate).ToListAsync();
            var listAppend = new List<RecommendSerialize>();
            var listFinal = new List<String>();

            if (!File.Exists(path)) {

                for (int i = 0; i < listRecommend.Count(); i++) {
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

                // using (FileStream fs = File.Create(path))

                foreach (var item in listAppend) {
                    var str = item.UserId.ToString()
                              + " "
                              + item.UserAge.ToString()
                              + " "
                              + item.UserGender.ToString()
                              + " "
                              + item.GameOrderId.ToString()
                              + " "
                              + item.PlayerId.ToString()
                              + " "
                              + item.PlayerAge.ToString()
                              + " "
                              + item.PlayerGender.ToString()
                              + " "
                              + item.GameOfPlayerId.ToString()
                              + " "
                              + item.Rate.ToString();
                    listFinal.Add(str);
                }

                if(!Directory.Exists(pathDirectory)){
                    Directory.CreateDirectory(pathDirectory);
                }
                using (var tw = new StreamWriter(path)) {
                    foreach (var item in listFinal) {
                        tw.WriteLine(item);
                    }
                }
                return true;
            }
            else if (File.Exists(path)) {
                var logFile = File.ReadAllLines(path);
                var logList = new List<string>(logFile);

                if (logList.Count() == 0) {
                    for (int i = 0; i < listRecommend.Count(); i++) {
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
                    foreach (var item in listAppend) {
                        var str = item.UserId.ToString() + " " + item.UserAge.ToString() + " " + item.UserGender.ToString() + " " + item.GameOrderId.ToString() + " " + item.PlayerId.ToString() + " " + item.PlayerAge.ToString() + " " + item.PlayerGender.ToString() + " " + item.GameOfPlayerId.ToString() + " " + item.Rate.ToString();
                        listFinal.Add(str);
                    }
                    using (var tw = new StreamWriter(path, true)) {
                        foreach (var item in listFinal) {
                            tw.WriteLine(item);
                        }
                    }
                    return true;
                }
                else {
                    if (listRecommend.Count() > logList.Count()) {

                        for (int i = listRecommend.Count() - (listRecommend.Count() - logList.Count()); i < listRecommend.Count(); i++) {
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
                        foreach (var item in listAppend) {
                            var str = item.UserId.ToString()
                                      + " "
                                      + item.UserAge.ToString()
                                      + " "
                                      + item.UserGender.ToString()
                                      + " "
                                      + item.GameOrderId.ToString()
                                      + " "
                                      + item.PlayerId.ToString()
                                      + " "
                                      + item.PlayerAge.ToString()
                                      + " "
                                      + item.PlayerGender.ToString()
                                      + " "
                                      + item.GameOfPlayerId.ToString()
                                      + " "
                                      + item.Rate.ToString();
                            listFinal.Add(str);
                        }
                        using (var tw = new StreamWriter(path, true)) {
                            foreach (var item in listFinal) {
                                tw.WriteLine(item);
                            }
                        }
                        return true;
                    }
                }
            }
            return false;
        }


    }
}