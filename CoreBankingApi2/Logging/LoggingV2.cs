namespace CoreBankingApi2.Logging
{
    public class LoggingV2: ILogging
    {
        public void Log(string message, string type)
        {
            if (type == "error")
            { 
                Console.BackgroundColor = ConsoleColor.DarkBlue;
                Console.WriteLine("FATAL ERROR - ", message);
                Console.BackgroundColor = ConsoleColor.Green;
            }
            else
            {
                if (type == "warning")
                {
                    Console.BackgroundColor = ConsoleColor.DarkYellow;  
                    Console.WriteLine("ERROR - ", message);
                    Console.BackgroundColor = ConsoleColor.Black;
                }
                else
                {
                    Console.WriteLine(message);
                }
            }
        }

        public void Information(string message, string type)
        {
            if (type == "error")
            {
                Console.BackgroundColor = ConsoleColor.DarkBlue;
                Console.WriteLine("FATAL ERROR - ", message);
                Console.BackgroundColor = ConsoleColor.Green;
            }
            else
            {
                if (type == "warning")
                {
                    Console.BackgroundColor = ConsoleColor.DarkYellow;
                    Console.WriteLine("ERROR - ", message);
                    Console.BackgroundColor = ConsoleColor.Black;
                }
                else
                {
                    Console.WriteLine(message);
                }
            }
        }
    }
}
