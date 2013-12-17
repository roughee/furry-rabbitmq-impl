/*
 * The Following Code was developed by Dewald Esterhuizen
 * View Documentation at: http://softwarebydefault.com
 * Licensed under Ms-PL 
*/

using System;
using System.ServiceModel;
using System.ServiceModel.Web;
using System.Web.Script.Services;
using System.Web.Services;

namespace BankService
{
    [WebService(Namespace = "http://localhost.com")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    public class TestWebService : System.Web.Services.WebService
    {
        [WebMethod]
        [System.Web.Services.Protocols.SoapDocumentMethodAttribute("http://localhost.com/LoanResponse",
        RequestNamespace = "http://localhost.com", ResponseNamespace = "http://localhost.com",
        Use = System.Web.Services.Description.SoapBindingUse.Literal,
        ParameterStyle = System.Web.Services.Protocols.SoapParameterStyle.Wrapped)]
        [ScriptMethod(UseHttpGet = true)]
        [OperationContract, WebInvoke(Method = "GET", UriTemplate = "LoanResponse?ssn={ssn}&creditScore={creditScore}&loanAmount={loanAmount}&loanDuration={loanDuration}")]
        public InterestRate LoanResponse()
        {
            var ssn = Convert.ToString(Context.Request["ssn"]);
            var creditScore = Convert.ToInt32(Context.Request["ssn"]);
            var loanAmount = Convert.ToInt32(Context.Request["loanAmount"]);
            var loadDuration = Convert.ToInt32(Context.Request["loanDuration"]);

            var loanCalculator = new LoanCalculator();

            return loanCalculator.CalculateLoan(ssn, creditScore, loanAmount, loadDuration);
        }
    }
}