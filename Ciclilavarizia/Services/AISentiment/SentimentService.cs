using AdventureWorks.Data;
using AdventureWorks.Models.AIData;
using Microsoft.EntityFrameworkCore;
using Microsoft.ML;
using static Microsoft.ML.DataOperationsCatalog;

namespace AdventureWorks.Services.AISentiment
{
    public class SentimentService(AdventureWorksContext context) : ISentimentService
    {
        public MLContext mlContext = new();
        private string mlModelPath = Path.Combine(Directory.GetCurrentDirectory(), "Models", "AIData", "Sentiment_MLModel.zip");

        private ITransformer? transformerModel;

        private async Task InitializeAsync()
        {
            if (File.Exists(mlModelPath))
            {
                using var modelStream = new FileStream(mlModelPath, FileMode.Open, FileAccess.Read);
                transformerModel = mlContext.Model.Load(modelStream, out var schema);
            }
            else
            {
                var trainTestData = await LoadData();
                transformerModel = BuildTrainTransformer(trainTestData.TrainSet);
                mlContext.Model.Save(transformerModel, trainTestData.TrainSet.Schema, mlModelPath);
            }
        }

        public async Task<SentimentPrediction> AnalyzeSentenceSentiment(string review)
        {
            await InitializeAsync();
            var input = new Sentiment { ReviewText = review };

            var predictionEngine = mlContext.Model
                .CreatePredictionEngine<Sentiment, SentimentPrediction>(transformerModel);

            var predictionResult = predictionEngine.Predict(input);

            return new SentimentPrediction
            {
                Probability = predictionResult.Probability,
                Score = predictionResult.Score,
                Label = predictionResult.Label,
            };
        }

        private ITransformer BuildTrainTransformer(IDataView dataView)
        {
            var pipeline = mlContext.Transforms.Text.FeaturizeText("Features", nameof(Sentiment.ReviewText))
                .Append(mlContext.BinaryClassification.Trainers.SdcaLogisticRegression(
                    labelColumnName: "Label",
                    featureColumnName: "Features",
                    maximumNumberOfIterations: 100));

            return pipeline.Fit(dataView);
        }

        private async Task<TrainTestData> LoadData()
        {
            var data = await context.Reviews
                .Select(s => new Sentiment
                {
                    ReviewText = s.ReviewText,
                    Label = s.Class
                })
                .ToListAsync();

            IDataView dataView = mlContext.Data.LoadFromEnumerable(data);
            return mlContext.Data.TrainTestSplit(dataView, testFraction: 0.2);
        }

    }

}
