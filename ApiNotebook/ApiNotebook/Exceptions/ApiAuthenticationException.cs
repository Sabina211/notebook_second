using System;

namespace ApiNotebook.Exceptions
{
    public class ApiAuthenticationException : Exception
    {
        public ApiAuthenticationException(string message = "Не удалось аутентифицироваться") : base(message)
        {
        }
    }
}