using System;
using System.Collections.Generic;
using System.Text;

namespace Ecovadis.API.Infrastructures
{
    public class TableSideException : Exception
    {
        public TableSideException()
        {
        }

        public TableSideException(string message)
            : base(message)
        {
        }

        public TableSideException(string message, Exception inner)
            : base(message, inner)
        {
        }
    }
}
