using System;
using System.Collections.Generic;
using System.Text;

namespace Ecovadis.DL.Infrastructures
{
    public class ErrorHandler : IErrorHandler
    {
        public string GetMessage(ErrorMessagesEnum message)
        {
            switch (message)
            {
                case ErrorMessagesEnum.EntityNull:
                    return "The entity passed is null";
                case ErrorMessagesEnum.ModelValidation:
                    return "The request data is not correct.";
                case ErrorMessagesEnum.ExternalDuplicate:
                    return "The request data is not valid.";
                case ErrorMessagesEnum.ExternalUnknown:
                    return "External unknown error.";
                case ErrorMessagesEnum.ExternalOK:
                    return "External OK";
                case ErrorMessagesEnum.ConcurrentDeleteError:
                    return "The request data is was deleted from other thread.";
                case ErrorMessagesEnum.ConcurrentUpdateError:
                    return "The request data is was updated from other thread.";
                default:
                    throw new ArgumentOutOfRangeException(nameof(message), message, null);
            }

        }
    }
}
