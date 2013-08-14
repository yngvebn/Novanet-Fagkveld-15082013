using System;
using System.Collections.Generic;
using System.Linq;

namespace Rikstoto.WebAPI.Exceptions
{
    public class BadFormatException : Exception
    {
        List<string> errors = new List<string>();

        public void AddError(string errorMessage)
        {
            errors.Add(errorMessage);
        }

        public bool HasErrors()
        {
            return errors.Count > 0;
        }

        public override string Message
        {
            get
            {
                var numberedErrorMessages = errors.Select((e, i) => string.Format("[{0}] {1}", i + 1, e));
                return string.Join(",", numberedErrorMessages);
            }
        }
    }
}