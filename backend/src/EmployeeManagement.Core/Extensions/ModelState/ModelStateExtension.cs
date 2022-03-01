using Microsoft.AspNetCore.Mvc.ModelBinding;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EmployeeManagement.Core.Extensions.ModelState
{
    public static class ModelStateHelper
    {
        public static IDictionary<string, string[]> SerializeErrors(this ModelStateDictionary modelState)
        {
            if (!modelState.IsValid)
            {
                var errorsDict = modelState
                    .Where(kvp => kvp.Value.Errors.Any()) //Filter the keys which doesn't have any errors
                    .ToDictionary(
                        kvp => kvp.Key,
                        kvp => kvp.Value.Errors.Select(e => e.ErrorMessage).ToArray()
                );

                return errorsDict;
            }

            return null;
        }
    }
}
