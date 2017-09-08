using System;
using System.IO;
using MonthlyPayslip.BusinessLayer;
using MonthlyPayslip.DataLayer.Repositories;

namespace MonthlyPayslip.ConsoleApp
{
    internal static class Program
    {
        private static void Main(string[] args)
        {
            var employeeCsv = "employee.csv";
            var taxtableCsv = "taxTable.csv";
            var payslipsCsv = "payslips.csv";

            var employeeRepository = new EmployeeRepository(employeeCsv);
            var taxTableRepository = new TaxTableRepository(taxtableCsv);
            var payslipRepository = new PayslipRepository(payslipsCsv);

            var payslipGenerator = new PayslipGenerator(employeeRepository, taxTableRepository);

            var payslips = payslipGenerator.GetPayslips();

            File.Delete(payslipsCsv);

            foreach (var payslip in payslips)
            {
                Console.WriteLine(payslip.ToCsv());
                payslipRepository.Add(payslip);
            }

            Console.ReadLine();
        }
    }
}
