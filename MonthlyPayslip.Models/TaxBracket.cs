namespace MonthlyPayslip.Models
{
    public class TaxBracket
    {
        public int MaximumIncome { get; set; }

        public double Tax { get; set; }

        public int FixedTax { get; set; }

        public bool HasMaximumLimit
        {
            get { return MaximumIncome >= 0; }
        }
    }
}