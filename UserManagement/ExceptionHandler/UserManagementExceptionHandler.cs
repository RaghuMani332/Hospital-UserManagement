using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System.Runtime.CompilerServices;
using UserManagement.CustomException;

namespace UserManagement.ExceptionHandler
{
    public class UserManagementExceptionHandler : ExceptionFilterAttribute
    {
        private readonly Dictionary<Type, (String ErrorMessage, int Statuscode)> mapping = new Dictionary<Type, (string ErrorMessage, int Statuscode)>
        {
            { typeof(DuplicateEntryException), ("Duplicate value Added to DataBase", StatusCodes.Status409Conflict) },
            {typeof(EmailSendingException),("Exception occured in EmailSending",StatusCodes.Status500InternalServerError) },
            {typeof(UserNotFoundException),("User Not Found",StatusCodes.Status404NotFound) },
            {typeof(PasswordMissMatchException),("Incorrect password",StatusCodes.Status400BadRequest) }
        };
        public override void OnException(ExceptionContext context)
        {
            Type exceptionType = context.Exception.GetType();

            string errorMessage;
            int statusCode;

            if (mapping.TryGetValue(exceptionType, out var mappingValue))
            {
                errorMessage = mappingValue.ErrorMessage;
                statusCode = mappingValue.Statuscode;
            }
            else
            {
                errorMessage = "Unknown exception. Contact developer.";
                statusCode = StatusCodes.Status422UnprocessableEntity;
            }

            // Clear existing model state errors
            //context.ModelState.Clear();

            // Add specific error
            context.ModelState.AddModelError(errorMessage, context.Exception.Message);

            // Create problem details
            var problemDetails = new ValidationProblemDetails(context.ModelState)
            {
                Status = statusCode,
                Type = context.Exception.GetType().ToString(),
                Title = errorMessage,
                Detail = context.Exception.Message // Include stack trace for debugging
            };

            // Set the result
            context.Result = new ObjectResult(problemDetails)
            {
                StatusCode = statusCode
            };
        }

    }
}
