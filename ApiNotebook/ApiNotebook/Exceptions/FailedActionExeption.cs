using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace ApiNotebook.Exceptions
{
    public class FailedActionExeption: Exception, ISerializable
    {
        public override string Message { get; }
        public FailedActionExeption(string message = "Не удалось выполнить действие") : base(message)
        {
        }

        public FailedActionExeption(IEnumerable<IdentityError> errorsList)
        {
            foreach (var error in errorsList)
                Message = $"{Message} {error.Description} ";
        }
    }
}
