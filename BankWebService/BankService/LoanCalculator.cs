namespace BankService
{
    public class LoanCalculator
    {
        public InterestRate CalculateLoan(int ssn, int creditScore, int loanAmount, int loanDuration)
        {
            return new InterestRate {ssn = ssn, interestRate = ((double) loanAmount*loanDuration)/creditScore};
        }
    }

    public class InterestRate
    {
        public int ssn { get; set; }
        public double interestRate { get; set; }
    }
}