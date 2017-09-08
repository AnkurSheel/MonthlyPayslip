using System;
using System.Collections.Generic;
using System.Linq;
using MonthlyPayslip.DataLayer.Interfaces;
using MonthlyPayslip.Models;

namespace MonthlyPayslip.BusinessLayer
{
    public class PayslipGenerator
    {
        private const int NumberOfMonths = 12;

        private readonly IRepository<Employee> _employeeRepository;
        private readonly IRepository<TaxBracket> _taxTableRepository;

        public PayslipGenerator(IRepository<Employee> employeeRepository, IRepository<TaxBracket> taxTableRepository)
        {
            _employeeRepository = employeeRepository;
            _taxTableRepository = taxTableRepository;
        }

        public IEnumerable<Payslip> GetPayslips()
        {
            var employees = _employeeRepository.FindAll();
            var payslips = new List<Payslip>();

            foreach (var employee in employees)
            {
                var grossIncome = GetGrossIncome(employee.AnnualSalary);
                var super = GetSuper(grossIncome, employee.SuperRate);
                var incomeTax = GetIncomeTax(employee.AnnualSalary);

                var payslip = new Payslip()
                              {
                                  Name = $"{employee.FirstName} {employee.LastName}",
                                  PayPeriod = employee.PaymentStartDate,
                                  GrossIncome = grossIncome,
                                  IncomeTax = incomeTax,
                                  Super = super
                              };
                payslips.Add(payslip);
            }

            return payslips;
        }

        private double CalculateIncomeTax(int annualSalary, TaxBracket taxBracket, int previousTaxBracketIncome)
        {
            var incomeTax = taxBracket.FixedTax + ((annualSalary - previousTaxBracketIncome) * taxBracket.Tax);
            return incomeTax;
        }

        private int GetGrossIncome(int annualSalary)
        {
            var grossIncome = Math.Round((double)annualSalary / NumberOfMonths, 2);
            var roundedGrossIncome = (int)Math.Round(grossIncome, MidpointRounding.AwayFromZero);
            return roundedGrossIncome;
        }

        private int GetIncomeTax(int annualSalary)
        {
            var previousTaxBracketIncome = 0;
            var incomeTax = 0.0;
            var found = false;
            var taxBrackets = _taxTableRepository.FindAll().ToList();
            TaxBracket lastTaxBracket = null;

            foreach (var taxBracket in taxBrackets)
            {
                lastTaxBracket = taxBracket;
                if (annualSalary <= taxBracket.MaximumIncome)
                {
                    incomeTax = CalculateIncomeTax(annualSalary, taxBracket, previousTaxBracketIncome);
                    found = true;
                    break;
                }

                if (taxBracket.HasMaximumLimit)
                {
                    previousTaxBracketIncome = taxBracket.MaximumIncome;
                }
            }

            if (found)
            {
                return (int)Math.Round(incomeTax / 12, MidpointRounding.AwayFromZero);
            }

            if (lastTaxBracket == null)
            {
                return 0;
            }

            if (lastTaxBracket.HasMaximumLimit)
            {
                // TODO : Move validation to Data Layer
                throw new InvalidOperationException("Last Tax Bracket should have max income of -1");
            }

            incomeTax = CalculateIncomeTax(annualSalary, lastTaxBracket, previousTaxBracketIncome);
            return (int)Math.Round(incomeTax / 12, MidpointRounding.AwayFromZero);
        }

        private int GetSuper(int grossIncome, double superRate)
        {
            var super = Math.Round(grossIncome * superRate, 2);
            var roundedSuper = (int)Math.Round(super, MidpointRounding.AwayFromZero);
            return roundedSuper;
        }
    }
}
