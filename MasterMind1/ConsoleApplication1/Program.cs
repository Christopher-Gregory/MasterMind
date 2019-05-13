using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApplication1
{
    class MasterMindEntry
    {
        public const short DIGIT_COUNT = 4;
        public const short DIGIT_RANGE = 6;
        public const short GUESSES_ALLOWED = 10;
        public const bool DEBUG = false;
        public short?[] SecretAnswer = new short?[DIGIT_COUNT];
        public short?[] GamerAnswer  = new short?[DIGIT_COUNT];
        public char?[] GamerAnswer_Eval = new char?[DIGIT_COUNT];

        static void Main(string[] args)
        {

            MasterMindEntry mme = new MasterMindEntry();
            mme.GenerateSecretAnswer();

            Console.WriteLine("\nA random sequence of four digits, each between 1 and 6, has been generated.");
            Console.WriteLine("A '+' means the digit is correct. A '-' means you've boxed the digit.");
            Console.WriteLine("You have " + GUESSES_ALLOWED.ToString() + " tries to guess it.");
            Console.WriteLine("Enter your first guess, then press ENTER.");
            Console.WriteLine("\npress CTL-C to exit");

            //
            if( DEBUG )
            {
                Console.WriteLine("\nANSWER FOR DEBUGGING: ");
                for (int i = 0; i < DIGIT_COUNT; i++)
                {
                    Console.Write(mme.SecretAnswer[i].ToString());
                }
                Console.WriteLine("\n");
            }


            bool UserWon = false;
            int guesscount = 0;

            while (guesscount < GUESSES_ALLOWED)
            {
                guesscount++;
                Console.Write("\nEnter Guess " + guesscount.ToString() + " Here ===> ");

                mme.GetGamersNextGuess();
                mme.EvaluateAnswer();

                Console.Write("\nResuls of Guess " + guesscount.ToString() + "  ===> ");
                for (int i = 0; i < DIGIT_COUNT; i++)
                {
                    Console.Write(mme.GamerAnswer_Eval[i].ToString());
                }
                Console.WriteLine("\n*****************************");

                if (mme.CheckIfWinner())
                {
                    UserWon = true;
                    break;
                }

            }

            Console.Write("\n The Correct Anser Is ===> ");
            for (int i = 0; i < DIGIT_COUNT; i++)
            {
                Console.Write(mme.SecretAnswer[i].ToString());
            }


            if (UserWon)
            {
                Console.WriteLine("\nHip, Hip, Hooray! You Won!");
            }
            else
            {
                Console.WriteLine("\nYou didn't guess number - but nice try.");
            }


            Console.WriteLine("\nPRESS any key to exit.");
            ConsoleKeyInfo  keystroke = Console.ReadKey();
           
        }

        public bool CheckIfWinner()
        {
            for (int k = 0; k < DIGIT_COUNT; k++)
            {
                if (GamerAnswer_Eval[k] != '+')
                {
                    return false;
                }
            }

            return true;
        }

        public void EvaluateAnswer()
        {
            // Clear previous eval
            for (int k = 0; k < DIGIT_COUNT; k++)
            {
                GamerAnswer_Eval[k] = ' ';
            }

            for (int k_SubjectDigit = 0; k_SubjectDigit < DIGIT_COUNT; k_SubjectDigit++)
            {
                int RelativeStart = k_SubjectDigit;

                for (int j_BaseAnswerDigit = 0; j_BaseAnswerDigit <  DIGIT_COUNT; j_BaseAnswerDigit++)
                {
                    if (k_SubjectDigit == j_BaseAnswerDigit)
                    {
                        if (GamerAnswer[j_BaseAnswerDigit] == SecretAnswer[j_BaseAnswerDigit])
                        {
                            GamerAnswer_Eval[j_BaseAnswerDigit] = '+';
                        }
                    }
                    else
                    {
                        if (GamerAnswer[k_SubjectDigit] == SecretAnswer[j_BaseAnswerDigit])
                        {
                            if (GamerAnswer_Eval[k_SubjectDigit] != '+')
                            {
                                GamerAnswer_Eval[k_SubjectDigit] = '-';
                            }
                        }
                    }
                }
            }
        }

        public void GetGamersNextGuess()
        {

            bool UserEnteredCompleteAnswer = false;
            short GoodDigitsCount = 0;

            while (GoodDigitsCount < DIGIT_COUNT || !UserEnteredCompleteAnswer)
            {
                ConsoleKeyInfo ValidKeyPress = GetAValidGameKeystroke();
                if (ValidKeyPress.Key == ConsoleKey.Enter)
                {
                    if (GoodDigitsCount >= (DIGIT_COUNT))
                    {
                        UserEnteredCompleteAnswer = true;
                        //Console.WriteLine("\nEnter pressed");
                    }
                    else
                    {
                        Console.SetCursorPosition((GoodDigitsCount + 24), Console.CursorTop);
                    }
                }
                else if (ValidKeyPress.Key == ConsoleKey.Backspace)
                {
                    Console.Write(" \b");
                    GoodDigitsCount = GoodDigitsCount > 0 ? --GoodDigitsCount : (short)0;

                }
                else
                {
                    if (GoodDigitsCount < DIGIT_COUNT)
                    {
                        short digit;
                        short.TryParse(ValidKeyPress.KeyChar.ToString(), out digit);
                        GamerAnswer[GoodDigitsCount] = digit;
                        GoodDigitsCount++;
                    }
                    else
                    {
                        Console.Write("\b \b");
                    }
                }
            }


        }

        public ConsoleKeyInfo GetAValidGameKeystroke()
        {
            ConsoleKeyInfo keystroke;

            while (true)
            {
                keystroke = Console.ReadKey();

                if ( (keystroke.Key == ConsoleKey.Enter) || (keystroke.Key == ConsoleKey.Backspace))
                {
                    break; 
                }

                else if ( Char.IsDigit(keystroke.KeyChar))
                {
                    short digit;
                    if (short.TryParse(keystroke.KeyChar.ToString(), out digit))
                    {
                        if (digit >= 1 && digit <= 6)
                        {
                            break;
                        }
                        else
                        {
                            Console.Write("\b \b");
                        }
                    }
                }
                else
                {
                    Console.Write("\b \b");
                }
            }
            return keystroke;
        }

        public void GenerateSecretAnswer()
        {
            Random radomizer = new Random(Guid.NewGuid().GetHashCode());
            for (int j = 0; j < DIGIT_COUNT; j++)
            {
                SecretAnswer[j] = (short)((radomizer.Next() % DIGIT_RANGE) + 1);
            }
            
        }

        #region LOCAL SAVE OFF AREA
        public void EvaluateAnswer_SAVE1()
        {
            // Clear previous eval
            for (int k = 0; k < DIGIT_COUNT; k++)
            {
                GamerAnswer_Eval[k] = ' ';
            }

            for (int k = 0; k < DIGIT_COUNT; k++)
            {
                int RelativeStart = k;

                for (int j = RelativeStart; j < (RelativeStart + DIGIT_COUNT); j++)
                {
                    int sIndex = j % DIGIT_COUNT;
                    if (j == RelativeStart)
                    {
                        if (GamerAnswer[j] == SecretAnswer[j])
                        {
                            GamerAnswer_Eval[j] = '+';
                        }
                    }
                    else
                    {
                        if (GamerAnswer[RelativeStart] == SecretAnswer[sIndex])
                        {
                            if (GamerAnswer_Eval[sIndex] != '+')
                            {
                                GamerAnswer_Eval[sIndex] = '-';
                            }
                        }
                    }
                }
            }
        }

        #endregion


    }

}
