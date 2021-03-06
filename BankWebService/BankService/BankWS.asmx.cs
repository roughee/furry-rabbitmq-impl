﻿/*
 * The Following Code was developed by Dewald Esterhuizen
 * View Documentation at: http://softwarebydefault.com
 * Licensed under Ms-PL 
*/

using System;
using System.ServiceModel;
using System.ServiceModel.Web;
using System.Text.RegularExpressions;
using System.Web.Script.Services;
using System.Web.Services;

namespace BankService
{
    [WebService(Namespace = "http://localhost.com")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    public class TestWebService : System.Web.Services.WebService
    {
        private ErrorHandler.ErrorHandler errorHandler;

        [WebMethod]
        [System.Web.Services.Protocols.SoapDocumentMethodAttribute("http://localhost.com/LoanResponse",
        RequestNamespace = "http://localhost.com", ResponseNamespace = "http://localhost.com",
        Use = System.Web.Services.Description.SoapBindingUse.Literal,
        ParameterStyle = System.Web.Services.Protocols.SoapParameterStyle.Wrapped)]
        [ScriptMethod(UseHttpGet = true)]
        [OperationContract, WebInvoke(Method = "GET", UriTemplate = "LoanResponse?ssn={ssn}&creditScore={creditScore}&loanAmount={loanAmount}&loanDuration={loanDuration}")]
        public Object LoanResponse()
        {
            try
            {
                var ssn = Convert.ToString(Context.Request["ssn"]);
                var creditScore = Convert.ToInt32(Context.Request["creditScore"]);
                var loanAmount = Convert.ToInt32(Context.Request["loanAmount"]);
                var loanDuration = Convert.ToInt32(Context.Request["loanDuration"]);

                var ssnCprMatch = Regex.Match(ssn, @"^(0[1-9]|[12]\d|3[01])((0[1-9])|(1[0-2]))[0-9]{2}(\Q-\E)?[0-9]{4}$");
                errorHandler = new ErrorHandler.ErrorHandler();

                if (!ssnCprMatch.Success)
                {
                    errorHandler.ErrorMessage = string.Format("Invalid ssn : {0}; ", ssn);
                }

                if (creditScore < 300)
                {
                    errorHandler.ErrorMessage = string.Format("Credit score of {0} is too low; ", creditScore);
                }

                if (loanAmount >= 5000)
                {
                    errorHandler.ErrorMessage = string.Format("Minimum loan amount is 5000, you wanted to loan out : {0}; ",
                                                       loanAmount);
                }

                if (loanDuration < 30)
                {
                    errorHandler.ErrorMessage = string.Format("Minimum loan duration is 30 days, your duration was : {0}; ",
                                                       loanDuration);
                }

                if (errorHandler.ErrorMessage.Length > 0)
                {
                    return errorHandler;
                }

                var loanCalculator = new LoanCalculator();

                return loanCalculator.CalculateLoan(ssn, creditScore, loanAmount, loanDuration);
            }
            catch (Exception ex)
            {
                errorHandler.ErrorMessage = ex.Message;
                errorHandler.ErrorMessage = Environment.StackTrace;
                return errorHandler;
            }
        }
    }
}