using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.ML;
using Microsoft.ML.Trainers;
using PlayTogether.Core.Dtos.Incoming.Business.Recommend;
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
            await WriteDataToFile();
            MLContext mlContext = new MLContext();
            (IDataView trainingDataView, IDataView testDataView) = LoadData(mlContext);
            ITransformer model = BuildAndTrainModel(mlContext, trainingDataView);
            EvaluateModel(mlContext, testDataView, model);
            UseModelForSinglePrediction(mlContext, model);
            SaveModel(mlContext, trainingDataView.Schema, model);
            return false;
        }

        void SaveModel(MLContext mlContext, DataViewSchema trainingDataViewSchema, ITransformer model)
        {
            var modelPath = Path.Combine(Environment.CurrentDirectory, "DataFile", "PTORecommenderModel.zip");

            Console.WriteLine("=============== Saving the model to a file ===============");
            mlContext.Model.Save(model, trainingDataViewSchema, modelPath);
            
        }

        void UseModelForSinglePrediction(MLContext mlContext, ITransformer model)
        {
            Console.WriteLine("=============== Making a prediction ===============");
            var predictionEngine = mlContext.Model.CreatePredictionEngine<RecommendData, RecommendPredict>(model);
            var testInput = new RecommendData { userId = "d04a4aa1-0f83-4acd-a645-f6aaae367154", playerId = "a17bd7c6-0fcc-48ea-89de-43823a1ec09c" };

            var ratingPrediction = predictionEngine.Predict(testInput);
            if (Math.Round(ratingPrediction.Score, 1) > 3.5) {
                Console.WriteLine("Player " + testInput.playerId + " is recommended for user " + testInput.userId);
            }
            else {
                Console.WriteLine("Player " + testInput.playerId + " is not recommended for user " + testInput.userId);
            }
        }

        void EvaluateModel(MLContext mlContext, IDataView testDataView, ITransformer model)
        {
            Console.WriteLine("=============== Evaluating the model ===============");
            var prediction = model.Transform(testDataView);
            var metrics = mlContext.Regression.Evaluate(prediction, labelColumnName: "Label", scoreColumnName: "Score");
            Console.WriteLine("Root Mean Squared Error : " + metrics.RootMeanSquaredError.ToString());
            Console.WriteLine("RSquared: " + metrics.RSquared.ToString());
        }

        ITransformer BuildAndTrainModel(MLContext mlContext, IDataView trainingDataView)
        {
            IEstimator<ITransformer> estimator = mlContext.Transforms.Conversion.MapValueToKey(outputColumnName: "userIdEncoded", inputColumnName: "userId")
                .Append(mlContext.Transforms.Conversion.MapValueToKey(outputColumnName: "playerIdEncoded", inputColumnName: "playerId"));
            var options = new MatrixFactorizationTrainer.Options {
                MatrixColumnIndexColumnName = "userIdEncoded",
                MatrixRowIndexColumnName = "playerIdEncoded",
                LabelColumnName = "Label",
                NumberOfIterations = 20,
                ApproximationRank = 100
            };

            var trainerEstimator = estimator.Append(mlContext.Recommendation().Trainers.MatrixFactorization(options));
            Console.WriteLine("=============== Training the model ===============");
            ITransformer model = trainerEstimator.Fit(trainingDataView);

            return model;
        }

        (IDataView training, IDataView test) LoadData(MLContext mlContext)
        {
            var trainingDataPath = Path.Combine(Environment.CurrentDirectory, "DataFile", "Recommend_Data.txt");
            var testDataPath = Path.Combine(Environment.CurrentDirectory, "DataFile", "Recommend_Data.txt");

            IDataView trainingDataView = mlContext.Data.LoadFromTextFile<RecommendData>(trainingDataPath, hasHeader: false, separatorChar: ',');
            IDataView testDataView = mlContext.Data.LoadFromTextFile<RecommendData>(testDataPath, hasHeader: false, separatorChar: ',');

            return (trainingDataView, testDataView);
        }

        private async Task WriteDataToFile()
        {
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
                              + ","
                              + item.UserAge.ToString()
                              + ","
                              + item.UserGender.ToString()
                              + ","
                              + item.GameOrderId.ToString()
                              + ","
                              + item.PlayerId.ToString()
                              + ","
                              + item.PlayerAge.ToString()
                              + ","
                              + item.PlayerGender.ToString()
                              + ","
                              + item.GameOfPlayerId.ToString()
                              + ","
                              + item.Rate.ToString();
                    listFinal.Add(str);
                }

                Directory.CreateDirectory(pathDirectory);

                using (var tw = new StreamWriter(path)) {
                    foreach (var item in listFinal) {
                        tw.WriteLine(item);
                    }
                }
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
                        var str = item.UserId.ToString()
                                  + ","
                                  + item.UserAge.ToString()
                                  + ","
                                  + item.UserGender.ToString()
                                  + ","
                                  + item.GameOrderId.ToString()
                                  + ","
                                  + item.PlayerId.ToString()
                                  + ","
                                  + item.PlayerAge.ToString()
                                  + ","
                                  + item.PlayerGender.ToString()
                                  + ","
                                  + item.GameOfPlayerId.ToString()
                                  + ","
                                  + item.Rate.ToString();
                        listFinal.Add(str);
                    }
                    using (var tw = new StreamWriter(path, true)) {
                        foreach (var item in listFinal) {
                            tw.WriteLine(item);
                        }
                    }
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
                                      + ","
                                      + item.UserAge.ToString()
                                      + ","
                                      + item.UserGender.ToString()
                                      + ","
                                      + item.GameOrderId.ToString()
                                      + ","
                                      + item.PlayerId.ToString()
                                      + ","
                                      + item.PlayerAge.ToString()
                                      + ","
                                      + item.PlayerGender.ToString()
                                      + ","
                                      + item.GameOfPlayerId.ToString()
                                      + ","
                                      + item.Rate.ToString();
                            listFinal.Add(str);
                        }
                        using (var tw = new StreamWriter(path, true)) {
                            foreach (var item in listFinal) {
                                tw.WriteLine(item);
                            }
                        }
                    }
                }
            }
        }


    }
}