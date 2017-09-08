namespace MonthlyPayslip.Models
{
    public class Payslip
    {
        public string Name { get; set; }

        public string PayPeriod { get; set; }

        public int GrossIncome { get; set; }

        public int IncomeTax { get; set; }

        public int NetIncome { get; set; }

        public int Super { get; set; }

        public string ToCsv()
        {
          return $"{Name}, {PayPeriod}, {GrossIncome}, {IncomeTax}, {NetIncome}, {Super}";
        }
    }
}