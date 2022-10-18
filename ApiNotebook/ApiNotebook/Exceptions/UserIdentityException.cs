using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;

namespace ApiNotebook.Exceptions
{
    public class UserIdentityException: Exception, ISerializable
    {
        public override string Message { get; }

        public UserIdentityException(string message = "Не удалось выполнить действие") : base(message)
        {
        }

        public UserIdentityException(IEnumerable<IdentityError> errorsList)
        {
            /*foreach (var error in errorsList)
                Message = $"{Message} {error.Description} ";*/

            var result = string.Join(';', errorsList.Select(x=>x.Description));//вынести в хелпер
        }
    }
}
