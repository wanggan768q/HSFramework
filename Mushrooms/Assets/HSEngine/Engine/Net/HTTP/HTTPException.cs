using System;

namespace HS.Net.HTTP
{
    public class HTTPException : Exception
    {
        public HTTPException(string message): base(message)
        {
        }
    }
}

