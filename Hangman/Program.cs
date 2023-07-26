using System;
using System.Collections.Generic;
using System.Text;
using System.Net;
using System.Runtime.CompilerServices;
using System.Security.Cryptography.X509Certificates;
using static System.Net.Mime.MediaTypeNames;

namespace Hangman {
    internal class Program {
        //the following function is used to print the visual hangman given the wrong attempts of the user
        private static void printHangman(int wrongAttempts) {
            if (wrongAttempts == 0) {
                Console.WriteLine("\n+---+");
                Console.WriteLine("    |");
                Console.WriteLine("    |");
                Console.WriteLine("    |");
                Console.WriteLine("   ===");
            }
            else if (wrongAttempts == 1) {
                Console.WriteLine("\n+---+");
                Console.WriteLine("O   |");
                Console.WriteLine("    |");
                Console.WriteLine("    |");
                Console.WriteLine("   ===");
            }
            else if (wrongAttempts == 2) {
                Console.WriteLine("\n+---+");
                Console.WriteLine("O   |");
                Console.WriteLine("|   |");
                Console.WriteLine("    |");
                Console.WriteLine("   ===");
            }
            else if (wrongAttempts == 3) {
                Console.WriteLine("\n+---+");
                Console.WriteLine(" O  |");
                Console.WriteLine("/|  |");
                Console.WriteLine("    |");
                Console.WriteLine("   ===");
            }
            else if (wrongAttempts == 4) {
                Console.WriteLine("\n+---+");
                Console.WriteLine(" O  |");
                Console.WriteLine("/|\\ |");
                Console.WriteLine("    |");
                Console.WriteLine("   ===");
            }
            else if (wrongAttempts == 5) {
                Console.WriteLine("\n+---+");
                Console.WriteLine(" O  |");
                Console.WriteLine("/|\\ |");
                Console.WriteLine("/   |");
                Console.WriteLine("   ===");
            }
            else if (wrongAttempts == 6) {
                Console.WriteLine("\n+---+");
                Console.WriteLine(" O  |");
                Console.WriteLine("/|\\ |");
                Console.WriteLine("/ \\ |");
                Console.WriteLine("   ===");
            }
        }
        //this function will print the word above the dashed line once the user guessed letters
        private static int printWord(List<char> guessedLetters, string randomWord) {
            int counter = 0; //a counter to iterate through the random word
            int rightLetters = 0; //number of the letter in the word that the user guessed correctly
            Console.Write("\r\n");
            //iterate through each character inside the string of the random word
            foreach (char c in randomWord) {
                if (guessedLetters.Contains(c)) {
                    Console.Write(c + " ");
                    rightLetters++;
                }
                else {
                    Console.Write("  ");
                }
                counter++;
            }
            return rightLetters;
        }
        //this is a method to print the lines underneath the random word
        private static void printLines(String randomWord, string unicode) {
            Console.Write("\r");
            foreach (char c in randomWord) {
                Console.OutputEncoding = System.Text.Encoding.Unicode;
                Console.Write(unicode + " "); // later down the line will use \u0305 for the character combining overline and \u0332 for character combining low line
            }
        }

        static void Main(string[] args) {

            Console.WriteLine("Welcome to Hangman! xD");
            Console.WriteLine("----------------------------------");

            //random word api is used in order to generate a random word from their library
            WebClient client = new WebClient();
            string downloadedString = client.DownloadString("https://random-word-api.herokuapp.com/word?number=1");
            string randomWord = downloadedString.Replace("[", "").Replace("]", "").Replace(@"""", "");//replaced the square brakets and double qoutes with nothing

            printLines(randomWord, "\u0332"); //printing the the lines for the user to know the length of the word

            //initializing all the variables necessary
            int lengthOfWordToGuess = randomWord.Length;
            int amountOfTimesWrong = 0;
            List<char> currentLettersGuessed = new List<char>();
            int currentLettersRight = 0;

            // the loop needed to check if the game is over by too many wrong guesses or by user guessing the word given
            while (amountOfTimesWrong != 6 && currentLettersRight != lengthOfWordToGuess) {
                Console.Write("\nLetters guessed so far: ");
                foreach (char letter in currentLettersGuessed) {
                    Console.Write(letter + " ");
                }
                //Prompt user for input
                Console.Write("\nGuess a letter: ");
                char letterGuessed = Console.ReadLine()[0];

                //Check If letter has already been guessed
                if (currentLettersGuessed.Contains(letterGuessed)) {
                    Console.Write("\r\nYou have already guessed this letter.");
                    printHangman(amountOfTimesWrong);
                    currentLettersRight = printWord(currentLettersGuessed, randomWord);
                    Console.Write("\r\n");
                    printLines(randomWord, "\u0305");
                }
                else {
                    //Check if letter is in the word
                    bool right = false;
                    for (int i = 0; i < randomWord.Length; i++) {
                        if (letterGuessed == randomWord[i]) {
                            right = true;
                        }
                    }

                    //User is right case
                    if (right) {
                        printHangman(amountOfTimesWrong);
                        currentLettersGuessed.Add(letterGuessed);
                        currentLettersRight = printWord(currentLettersGuessed, randomWord);
                        Console.Write("\r\n");
                        printLines(randomWord, "\u0305");
                    }

                    //User is wrong case
                    else {
                        amountOfTimesWrong++;
                        currentLettersGuessed.Add(letterGuessed);
                        //Update the drawing
                        printHangman(amountOfTimesWrong);
                        //Print the word
                        currentLettersRight = printWord(currentLettersGuessed, randomWord);
                        Console.Write("\r\n");
                        printLines(randomWord, "\u0305");
                    }
                }
            }
            if (amountOfTimesWrong != 6) {
                Console.WriteLine("\r\nCongratulation! \\( ﾟヮﾟ)/");
                Console.WriteLine("\r\nYou managed to guess the word.");
                Console.WriteLine("\r\nWould you like to play again? (Please type yes or no)");
                string playAgain = Console.ReadLine().ToLower();
                //Case when user wants to play again, rerun the executable inside the bin folder
                if (playAgain != "yes") {
                    Environment.Exit(0);
                }
                else {
                    Console.WriteLine("\r\n(⌐■_■)");
                    Console.WriteLine("\r\n");
                    string FriendlyName = "Hangman.exe";
                    System.Diagnostics.Process.Start(System.AppDomain.CurrentDomain.FriendlyName);
                }
            }
            else {
                Console.WriteLine("\r\nUnfortunately you did not guess the word this time. (╥_╥)");
                Console.WriteLine("\r\nThe word was " + randomWord);
                Console.WriteLine("\r\nWould you like to try again? (Please type yes or no) ( ◔ ʖ̯ ◔ )");
                string playAgain = Console.ReadLine().ToLower();
                //Case when user wants to play again, rerun the executable inside the bin folder
                if (playAgain == "yes") {
                    Console.WriteLine("\r\n(⌐■_■)");
                    Console.WriteLine("\r\n");
                    string FriendlyName = "Hangman.exe";
                    System.Diagnostics.Process.Start(System.AppDomain.CurrentDomain.FriendlyName);
                }
                else {
                    Environment.Exit(0);
                }

            }
        }
    }
}