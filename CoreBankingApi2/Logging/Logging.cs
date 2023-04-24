namespace CoreBankingApi2.Logging
{
    public class Logging : ILogging
    {
        public void Information(string message, string type)
        {
            throw new NotImplementedException();
        }

        public void Log(string message, string type)
        {
            if(type == "error")
            {
                Console.WriteLine( "ERROR - ", message);
            }
            else
            {
                Console.WriteLine(message); 
            }
        }
    }
}
