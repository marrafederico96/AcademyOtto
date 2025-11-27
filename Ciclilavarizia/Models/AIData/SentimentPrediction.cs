using Microsoft.ML.Data;

namespace AdventureWorks.Models.AIData
{
    public class SentimentPrediction
    {
        [ColumnName("PredictedLabel")]
        public bool Label { get; set; }

        public float Probability { get; set; }

        public float Score { get; set; }
    }
}
