using System;
using System.Collections.Generic;
using System.Text;

namespace Ecovadis.API.Infrastructures
{
    public class FinishedMatchException : Exception
    {
        public FinishedMatchException()
        {
        }

        public FinishedMatchException(string message)
            : base(message)
        {
        }

        public FinishedMatchException(string message, Exception inner)
            : base(message, inner)
        {
        }
    }
}
