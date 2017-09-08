namespace MonthlyPayslip.Models
{
    public class Payslip
    {
        public string Name { get; set; }

        public string PayPeriod { get; set; }

        public int GrossIncome { get; set; }

        public int Super { get; set; }
    }
}