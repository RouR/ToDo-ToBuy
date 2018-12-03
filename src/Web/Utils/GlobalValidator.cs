using System;
using System.Collections.Generic;
using System.Linq;
using Domain.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Controllers;
using Microsoft.AspNetCore.Mvc.Filters;
using Utils;
using Utils.WebRequests;

namespace Web.Utils
{
    public class GlobalValidatorAttribute : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext context)
        {
            if (context.ModelState.IsValid)
                return;

            var retType = ((ControllerActionDescriptor) context.ActionDescriptor).MethodInfo.ReturnType;

            var canValidate = retType
                .GetInterfaces()
                .Any(x => x.IsGenericType && x.GetGenericTypeDefinition() == typeof(IErrorable<>));

            if (!canValidate)
                return;

            var instance = Activator.CreateInstance(retType);
            IErrorableHelper.SetError((dynamic) instance, "not valid");

            if (instance is IServerValidation)
            {
                var messages = new List<KeyValuePair<string, string>>();
                foreach (var stateValue in context.ModelState)
                foreach (var stateError in stateValue.Value.Errors)
                {
                    messages.Add(new KeyValuePair<string, string>(stateValue.Key, stateError.ErrorMessage));
                }

                ((IServerValidation) instance).ValidationErrors = messages;
            }
            
            context.Result = new OkObjectResult(instance);
        }
    }
}