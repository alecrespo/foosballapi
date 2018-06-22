using System;
using System.Collections.Generic;
using System.Text;

namespace Ecovadis.DL.Infrastructures
{

    public enum ErrorMessagesEnum
    {
        EntityNull = 1,
        ModelValidation = 2,
        ExternalDuplicate =3,
        ExternalUnknown = 4,
        ExternalOK = 5,
        ConcurrentUpdateError = 6,
        ConcurrentDeleteError = 7
    }

    public interface IErrorHandler
    {
        string GetMessage(ErrorMessagesEnum message);
    }

}
