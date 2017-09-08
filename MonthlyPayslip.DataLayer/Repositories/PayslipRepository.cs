using MonthlyPayslip.Models;

namespace MonthlyPayslip.DataLayer.Repositories
{
    public class PayslipRepository : BaseCsvRepository<Payslip>
    {
        public PayslipRepository(string inputFileName)
            : base(inputFileName)
        {
        }

        protected override Payslip Map(string[] data)
        {
            throw new System.NotImplementedException();
        }

        protected override string ToCsv(Payslip payslip)
        {
            return payslip.ToCsv();
        }
    }
}
