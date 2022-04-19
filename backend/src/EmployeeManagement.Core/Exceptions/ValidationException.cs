using FluentValidation.Results;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace EmployeeManagement.Core.Exceptions
{
    public class ValidationException : ApplicationException
    {
        public IDictionary<string, string[]> Failures { get; }

        private ValidationException() 
            : base("Validation error occured")
        {
            Failures = new Dictionary<string, string[]>();
        }

        public ValidationException(params ValidationFailure[] failures): this(failures.ToList()) { }
        public ValidationException(List<ValidationFailure> failures) : this()
        {
            var propertyNames = failures
                .Select(e => e.PropertyName)
                .Distinct();

            foreach (var propertyName in propertyNames)
            {
                var propertyFailures = failures
                    .Where(e => e.PropertyName == propertyName)
                    .Select(e => e.ErrorMessage)
                    .ToArray();

                Failures.Add(propertyName, propertyFailures);
            }
        }

        public ValidationException(string property, string message) : this()
        {
            Failures.Add(property, new string[]{ message });
        }
        public ValidationException(string message)  : this("*", message) { }
    }
}
