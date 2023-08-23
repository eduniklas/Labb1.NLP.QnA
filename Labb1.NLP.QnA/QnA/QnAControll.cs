using Azure;
using Azure.AI.TextAnalytics;
using Azure.AI.Language.QuestionAnswering;
using Labb1.NLP.QnA.Translate;
using Microsoft.Extensions.Configuration;
using Labb1.NLP.QnA.Speech;
using System;
using static System.Net.Mime.MediaTypeNames;

namespace Labb1.NLP.QnA.QnA
{
    public class QnAControll
    {
        public void StartQnA()
        {
            IConfigurationBuilder builder = new ConfigurationBuilder().AddJsonFile("appsettings.json");
            IConfigurationRoot configuration = builder.Build();
            string cogSvcEndpoint = configuration["CognitiveServicesEndpoint"];
            string cogSvcKey = configuration["CognitiveServiceKey"];
            string projectName = "LearnFAQ";
            string deploymentName = "production";

            Uri endpoint = new Uri(cogSvcEndpoint);
            AzureKeyCredential credentials = new AzureKeyCredential(cogSvcKey);
            TextAnalyticsClient CogClient = new TextAnalyticsClient(endpoint, credentials);

            QuestionAnsweringClient client = new QuestionAnsweringClient(endpoint, credentials);
            QuestionAnsweringProject project = new QuestionAnsweringProject(projectName, deploymentName);

            TranslateText translate = new TranslateText();
            DetectedLanguage detectedLanguage = new DetectedLanguage();
            Speak speak = new Speak();

            string question = "";
            string text = "";
            bool exitMenu = false;
            while (!exitMenu)
            {
                DisplayMenu();

                Console.Write("Enter your selection: ");
                string input = Console.ReadLine();
                Console.Clear();

                switch (input)
                {
                    case "1":
                        Console.WriteLine("Ask me anyting or say quit to exit\n");
                        while (question.ToLower() != "quit.")
                        {
                            
                            var s = speak.QnASpeak();
                            question = s.Result;

                            if (question != string.Empty)
                            {   
                                Response<AnswersResult> response = client.GetAnswers(question, project);
                                foreach (KnowledgeBaseAnswer answer in response.Value.Answers)
                                {
                                    text = $"Alan:{answer.Answer}";
                                    WriteTextWithDelay(text, delayMilliseconds:250);
                                    speak.TranscribeCommand("en", answer.Answer);
                                    Console.ReadLine();
                                }
                            }
                        }
                        break;
                    case "2":
                        while (question.ToLower() != "quit")
                        {
                            Console.WriteLine("Ask me anyting or write quit to exit");
                            question = Console.ReadLine();

                            if (question != string.Empty)
                            {
                                detectedLanguage = CogClient.DetectLanguage(question);

                                if (detectedLanguage.Iso6391Name != "en")
                                {
                                    var q = translate.TranslateTe(question, detectedLanguage.Iso6391Name);

                                    Response<AnswersResult> response = client.GetAnswers(q.Result, project);
                                    foreach (KnowledgeBaseAnswer answer in response.Value.Answers)
                                    {
                                        var a = translate.TranslateTe(answer.Answer, detectedLanguage.Iso6391Name);
                                        text = $"Alan:{a.Result}";
                                        WriteTextWithDelay(text, delayMilliseconds: 250);
                                        speak.TranscribeCommand(detectedLanguage.Iso6391Name, a.Result);
                                        Console.ReadLine();
                                    }
                                }
                                else
                                {
                                    Response<AnswersResult> response = client.GetAnswers(question, project);
                                    foreach (KnowledgeBaseAnswer answer in response.Value.Answers)
                                    {
                                        text = $"Alan:{answer.Answer}";
                                        WriteTextWithDelay(text, delayMilliseconds: 250);
                                        speak.TranscribeCommand(detectedLanguage.Iso6391Name, answer.Answer);
                                        Console.ReadLine();
                                    }
                                }
                            }
                            else
                                Console.WriteLine("Enter any input");
                        }
                        break;
                    case "3":
                        while (question.ToLower() != "quit")
                        {
                            Console.WriteLine("Ask me anyting or write quit to exit");
                            question = Console.ReadLine();

                            if (question != string.Empty)
                            {
                                detectedLanguage = CogClient.DetectLanguage(question);

                                if (detectedLanguage.Iso6391Name != "en")
                                {
                                    var q = translate.TranslateTe(question, detectedLanguage.Iso6391Name);

                                    Response<AnswersResult> response = client.GetAnswers(q.Result, project);
                                    foreach (KnowledgeBaseAnswer answer in response.Value.Answers)
                                    {
                                        var a = translate.TranslateTe(answer.Answer, detectedLanguage.Iso6391Name);
                                        text = $"Alan:{a.Result}";
                                        WriteTextWithDelay(text, delayMilliseconds: 150);
                                        Console.ReadLine();
                                    }
                                }
                                else
                                {
                                    Response<AnswersResult> response = client.GetAnswers(question, project);
                                    foreach (KnowledgeBaseAnswer answer in response.Value.Answers)
                                    {
                                        text = $"Alan:{answer.Answer}";
                                        WriteTextWithDelay(text, delayMilliseconds: 150);
                                        Console.ReadLine();
                                    }
                                }
                            }
                            else
                                Console.WriteLine("Enter any input");
                        }
                        break;
                    default:
                        Console.WriteLine("Invalid selection. Please try again.");
                        break;
                }
                exitMenu = true;
            }
        }
        static void DisplayMenu()
        {
            Console.WriteLine("╔═══════════════════════════════════════════╗");
            Console.WriteLine("║             Choose how to interact        ║");
            Console.WriteLine("╠═══════════════════════════════════════════╣");
            Console.WriteLine("║     1. Speek with Alan (English only)     ║");
            Console.WriteLine("║                                           ║");
            Console.WriteLine("║     2. Write to Alan (He talks back)      ║");
            Console.WriteLine("║                                           ║");
            Console.WriteLine("║            3. Write with Alan             ║");
            Console.WriteLine("╚═══════════════════════════════════════════╝");
        }
        static async Task WriteTextWithDelay(string text, int delayMilliseconds)
        {
            int count = 0;
            string[] words = text.Split(' ');
            foreach (string word in words)
            {
                if (count == 12)
                {
                    Console.Write("\n");
                    count = 0;
                }
                Console.Write(word + " ");
                await Task.Delay(delayMilliseconds);
                count++;
            }
            Thread.Sleep(delayMilliseconds);
            Console.WriteLine("\n\nPress enter to continue...\n");
        }
    }
}