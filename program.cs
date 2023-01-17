using System;
using System.Speech.Recognition;
using System.Speech.Synthesis;

class VoiceAssistant
{
    static void Main(string[] args)
    {
        // Initialize speech recognition engine
        var recognizer = new SpeechRecognizer();

        // Add a simple grammar for recognizing "Hello" and "Goodbye"
        recognizer.LoadGrammar(new Grammar(new GrammarBuilder("Hello")));
        recognizer.LoadGrammar(new Grammar(new GrammarBuilder("Goodbye")));

        // Add a grammar for recognizing questions
        var questionBuilder = new GrammarBuilder();
        questionBuilder.Append("What is");
        questionBuilder.Append(new Choices("your", "the"));
        questionBuilder.Append("name", "time");
        recognizer.LoadGrammar(new Grammar(questionBuilder));

        // Attach event handlers for recognizing and rejecting speech
        recognizer.SpeechRecognized += RecognizerOnSpeechRecognized;
        recognizer.SpeechRecognitionRejected += RecognizerOnSpeechRecognitionRejected;

        // Initialize speech synthesis engine
        var synthesizer = new SpeechSynthesizer();

        // Start listening for speech
        recognizer.SetInputToDefaultAudioDevice();
        recognizer.RecognizeAsync(RecognizeMode.Multiple);

        // Wait for user to say "Goodbye"
        while (Console.ReadKey().Key != ConsoleKey.Q) { }

        // Cleanup
        recognizer.Dispose();
        synthesizer.Dispose();
    }

    private static void RecognizerOnSpeechRecognized(object sender, SpeechRecognizedEventArgs e)
    {
        // Get the recognized text
        string text = e.Result.Text;

        // Respond to recognized speech
        if (text == "Hello")
        {
            Console.WriteLine("Hello, how can I help you?");
        }
        else if (text == "Goodbye")
        {
            Console.WriteLine("Goodbye, have a nice day!");
        }
        else if (text.StartsWith("What is your name"))
        {
            Console.WriteLine("My name is VoiceAssistant.");
        }
        else if (text.StartsWith("What is the time"))
        {
            Console.WriteLine("The current time is " + DateTime.Now.ToString("h:mm tt"));
        }
    }

    private static void RecognizerOnSpeechRecognitionRejected(object sender, SpeechRecognitionRejectedEventArgs e)
    {
        Console.WriteLine("Speech not recognized, please try again.");
    }
}
