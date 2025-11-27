using Microsoft.ML.Data;

namespace AdventureWorks.Models.AIData
{
    public class Sentiment
    {
        [LoadColumn(0)]
        public string ReviewText { get; set; } = string.Empty;

        [LoadColumn(1)]
        public bool Label { get; set; }
    }
}
