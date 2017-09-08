using System.Globalization;
using MonthlyPayslip.Models;

namespace MonthlyPayslip.DataLayer.Repositories
{
    public class EmployeeRepository : BaseCsvRepository<Employee>
    {
        public EmployeeRepository(string inputFileName)
            : base(inputFileName)
        {
        }

        protected override Employee Map(string[] data)
        {
            var employee = new Employee
                           {
                               FirstName = data[0],
                               LastName = data[1],
                               AnnualSalary = int.Parse(data[2]),
                               SuperRate = double.Parse(data[3].TrimEnd(CultureInfo.CurrentCulture.NumberFormat.PercentSymbol.ToCharArray())) / 100.0,
                               PaymentStartDate = data[4]
                           };

            return employee;
        }

        protected override string ToCsv(Employee entity)
        {
            throw new System.NotImplementedException();
        }
    }
}
