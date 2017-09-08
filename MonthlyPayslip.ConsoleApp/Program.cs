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
            if (args.Length < 3)
            {
                Console.WriteLine("Usage: MonthlyPayslip.ConsoleApp.exe employeeCsvFile taxtableCsvFile payslipsCsvFile");
                return;
            }

            var employeeCsv = args[0];
            var taxtableCsv = args[1];
            var payslipsCsv = args[2];

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
        }
    }
}
