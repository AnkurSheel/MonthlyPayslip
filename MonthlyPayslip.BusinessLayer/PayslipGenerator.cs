﻿using System;
using System.Collections.Generic;
using MonthlyPayslip.DataLayer.Interfaces;
using MonthlyPayslip.Models;

namespace MonthlyPayslip.BusinessLayer
{
    public class PayslipGenerator
    {
        private const int NumberOfMonths = 12;

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
                var grossIncome = GetGrossIncome(employee.AnnualSalary);
                var payslip = new Payslip() { Name = $"{employee.FirstName} {employee.LastName}", GrossIncome = grossIncome };
                payslips.Add(payslip);
            }

            return payslips;
        }

        private int GetGrossIncome(int annualSalary)
        {
            var grossIncome = Math.Round((double)annualSalary / NumberOfMonths, 2);
            var roundedGrossIncome = (int)Math.Round(grossIncome, MidpointRounding.AwayFromZero);
            return roundedGrossIncome;
        }
    }
}
