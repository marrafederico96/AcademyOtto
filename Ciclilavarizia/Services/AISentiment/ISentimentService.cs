using AdventureWorks.Models.AIData;

namespace AdventureWorks.Services.AISentiment
{
    public interface ISentimentService
    {
        public Task<SentimentPrediction> AnalyzeSentenceSentiment(string review);
    }
}
