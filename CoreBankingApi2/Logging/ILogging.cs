namespace CoreBankingApi2.Logging
{
    public interface ILogging
    {
        public void Log(string message,string type);
        public void Information(string message,string type);
    }
}
