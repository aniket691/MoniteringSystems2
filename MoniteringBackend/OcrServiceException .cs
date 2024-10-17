namespace MoniteringBackend
{
    public class OcrServiceException : Exception
    {
        public OcrServiceException(string message) : base(message) { }

        public OcrServiceException(string message, Exception innerException) : base(message, innerException) { }
    }

}
