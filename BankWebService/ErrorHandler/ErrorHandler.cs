using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ErrorHandler
{
    public class ErrorHandler
    {
        static StringBuilder errMessage = new StringBuilder();

        //Make class immutable
        static ErrorHandler()
        {
        }

        public string ErrorMessage
        {
            get {return errMessage.ToString();}
            set
            {
                errMessage.AppendLine(value);
            }
        }
    }
}
