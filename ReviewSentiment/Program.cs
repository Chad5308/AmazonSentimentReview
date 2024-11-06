// See https://aka.ms/new-console-template for more information

namespace ReviewSentiment;

public class Program()
{
    public static Dictionary<List<string>, int> testSet = new();
    public static string testFilePath = "/test-reviews-micro.csv";

        
        
    static void Main(string[] args)
    {
        LoadData();
        Console.WriteLine("The prediction accuracy is " + PredictData());
    }




    public static void LoadData()
    {
        var path = Path.Combine(Environment.CurrentDirectory, $"data\\{testFilePath}");
        var lines = File.ReadAllLines(path);
        

        foreach (var line in lines)
        {
            if (line == "sentiment,title,review_text") continue;
            var sentiment = int.Parse(line.Substring(0, 1));
            var title = line.Substring(line.IndexOf(",") + 1, line.IndexOf(",", 3) - 1);
            var review = line.Substring(line.IndexOf(",", 3) + 1);
            List<string> title_review = new();

            title_review.Add(title);
            title_review.Add(review);
            testSet.Add(title_review, sentiment);
        }

    }

    public static float PredictData()
    {
        float predictedSentiment = 0;
        float predictionAccuracy = 0;
        var accuracies = testSet.Values.ToList();
        int count = -1;
        float correct = 0;
        var wrong = 0;

        foreach (var title_review in testSet.Keys)
            //for(var i = 0; i< testSet.Keys.Count; i++)
        {
            count++;
            var review = title_review[1];
            //var review = testSet[testSet.Keys.ElementAt(i)];

            var sampleData = new SentimentReviewModel.ModelInput()
            {
                Review_text = review
                //Review_text = @"Despite the fact that I have only played a small portion of the game, the music I heard (plus the connection to Chrono Trigger which was great as well) led me to purchase the soundtrack, and it remains one of my favorite albums. There is an incredible mix of fun, epic, and emotional songs. Those sad and beautiful tracks I especially like, as there's not too many of those kinds of songs in my other video game soundtracks. I must admit that one of the songs (Life-A Distant Promise) has brought tears to my eyes on many occasions.My one complaint about this soundtrack is that they use guitar fretting effects in many of the songs, which I find distracting. But even if those weren't included I would still consider the collection worth it.",
            };

            var result = (double)(new decimal(SentimentReviewModel.Predict(sampleData).PredictedLabel));
                

            if (result == accuracies[count])
            {
                correct++;
            }

            predictionAccuracy = correct / accuracies.Count;
        }

        return predictionAccuracy * 100;
    }




}