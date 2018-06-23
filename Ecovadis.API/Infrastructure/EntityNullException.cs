using System;
using System.Collections.Generic;
using System.Text;

namespace Ecovadis.API.Infrastructures
{
    public class EntityNullException : Exception
    {
        public EntityNullException()
        {
        }

        public EntityNullException(string message)
            : base(message)
        {
        }

        public EntityNullException(string message, Exception inner)
            : base(message, inner)
        {
        }
    }
}
