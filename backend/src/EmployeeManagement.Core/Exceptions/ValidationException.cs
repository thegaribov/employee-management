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
        public IDictionary<string, IDictionary<string, string[]>> Errors { get; }

        private ValidationException()
            : base("Validation error occured")
        {
            Errors = new Dictionary<string, IDictionary<string, string[]>>();
        }

        public ValidationException(params ValidationFailure[] failures) : this(failures.ToList()) { }
        public ValidationException(List<ValidationFailure> failures) : this()
        {
            var failuresDictionary = new Dictionary<string, string[]>();

            var propertyNames = failures
              .Select(e => e.PropertyName)
              .Distinct();

            foreach (var propertyName in propertyNames)
            {
                var propertyFailures = failures
                    .Where(e => e.PropertyName == propertyName)
                    .Select(e => e.ErrorMessage)
                    .ToArray();

                failuresDictionary.Add(propertyName, propertyFailures);
            }

            Errors.Add("errors", failuresDictionary);
        }

        public ValidationException(string property, string message) : this()
        {
            Errors.Add("errors", new Dictionary<string, string[]>() { { property, new string[] { message } } });
        }
        public ValidationException(string message) : this(string.Empty, message) { }
    }
}
