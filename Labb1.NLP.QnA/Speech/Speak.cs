
using Microsoft.CognitiveServices.Speech;
using Microsoft.CognitiveServices.Speech.Audio;
using Microsoft.CognitiveServices.Speech.Translation;
using Microsoft.Extensions.Configuration;
using System.Text;
using System.Transactions;

namespace Labb1.NLP.QnA.Speech
{
    public class Speak
    {
        private static SpeechConfig speechConfig;
        private static SpeechTranslationConfig translationConfig;
        public async Task<string> QnASpeak()
        {
            IConfigurationBuilder builder = new ConfigurationBuilder().AddJsonFile("appsettings.json");
            IConfigurationRoot configuration = builder.Build();
            string cogSvcKey = configuration["speechKey"];
            string location = configuration["speechLocation"];

            speechConfig = SpeechConfig.FromSubscription(cogSvcKey, location);
           
            string command = "";

            speechConfig.SpeechSynthesisVoiceName = "en-GB-RyanNeural";
            using SpeechSynthesizer speechSynthesizer = new SpeechSynthesizer(speechConfig);

            Console.WriteLine("Speak now..");
            using SpeechRecognizer speechRecognizer = new SpeechRecognizer(speechConfig);
            SpeechRecognitionResult speech = await speechRecognizer.RecognizeOnceAsync();
            if (speech.Reason == ResultReason.RecognizedSpeech)
            {
                command = speech.Text;
                Console.WriteLine(command);
            }
            else
            {
                Console.WriteLine(speech.Reason);
                if (speech.Reason == ResultReason.Canceled)
                {
                    var cancellation = CancellationDetails.FromResult(speech);
                    Console.WriteLine(cancellation.Reason);
                    Console.WriteLine(cancellation.ErrorDetails);
                }
            }
            return command;

        }
        public async Task TranscribeCommand(string targetLanguage, string anwser)
        {
            IConfigurationBuilder builder = new ConfigurationBuilder().AddJsonFile("appsettings.json");
            IConfigurationRoot configuration = builder.Build();
            string cogSvcKey = configuration["speechKey"];
            string location = configuration["speechLocation"];
            speechConfig = SpeechConfig.FromSubscription(cogSvcKey, location);
            var voices = new Dictionary<string, string>
            {
                ["sv"] = "sv-SE-MattiasNeural",
                ["en"] = "en-GB-RyanNeural",
                ["es"] = "es-ES-ArnauNeural",
                ["hi"] = "hi-IN-MadhurNeural"
            };
            // Configure speech recognition
            speechConfig.SpeechSynthesisVoiceName = voices[targetLanguage];
            using SpeechSynthesizer speechSynthesizer = new SpeechSynthesizer(speechConfig);

            SpeechSynthesisResult speak = await speechSynthesizer.SpeakTextAsync(anwser);
            if (speak.Reason != ResultReason.SynthesizingAudioCompleted)
            {
                Console.WriteLine(speak.Reason);
            }
        }
    }
}
