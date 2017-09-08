using MonthlyPayslip.Models;

namespace MonthlyPayslip.DataLayer.Repositories
{
    public class TaxTableRepository : BaseCsvRepository<TaxBracket>
    {
        public TaxTableRepository(string inputFileName)
            : base(inputFileName)
        {
        }

        protected override TaxBracket Map(string[] data)
        {
            var taxBracket = new TaxBracket() { MaximumIncome = int.Parse(data[0]), Tax = double.Parse(data[1]), FixedTax = int.Parse(data[2]), };

            return taxBracket;
        }

        protected override string ToCsv(TaxBracket entity)
        {
            throw new System.NotImplementedException();
        }
    }
}
