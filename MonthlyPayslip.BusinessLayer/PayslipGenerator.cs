using System.Collections.Generic;
using MonthlyPayslip.DataLayer.Interfaces;
using MonthlyPayslip.Models;

namespace MonthlyPayslip.BusinessLayer
{
    public class PayslipGenerator
    {
        private readonly IRepository<Employee> _employeeRepository;

        public PayslipGenerator(IRepository<Employee> employeeRepository)
        {
            _employeeRepository = employeeRepository;
        }

        public IEnumerable<Payslip> GetPayslips()
        {
            var employees = _employeeRepository.FindAll();
            var payslips = new List<Payslip>();

            foreach (var employee in employees)
            {
                var payslip = new Payslip() { Name = $"{employee.FirstName} {employee.LastName}" };
                payslips.Add(payslip);
            }

            return payslips;
        }
    }
}
