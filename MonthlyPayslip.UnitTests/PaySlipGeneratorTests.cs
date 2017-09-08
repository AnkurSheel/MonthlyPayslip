using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MonthlyPayslip.BusinessLayer;
using MonthlyPayslip.DataLayer.Interfaces;
using MonthlyPayslip.Models;
using NSubstitute;

namespace MonthlyPayslip.UnitTests
{
    [TestClass]
    public class PaySlipGeneratorTests
    {
        private const string PaymentDate = "01 March – 31 March";

        private IRepository<Employee> _employeeRepository;
        private PayslipGenerator _payslipGenerator;
        private string _firstName;
        private string _lastName;
        private List<Employee> _employees;
        private IRepository<TaxBracket> _taxTableRepository;

        [TestInitialize]
        public void Setup()
        {
            _firstName = "David";
            _lastName = "Rudd";

            _employees = new List<Employee>()
                         {
                             new Employee()
                             {
                                 FirstName = _firstName,
                                 LastName = _lastName,
                                 AnnualSalary = 60000,
                                 SuperRate = 0.09,
                                 PaymentStartDate = PaymentDate
                             }
                         };

            var taxBrackets = new List<TaxBracket>()
                              {
                                  new TaxBracket() { MaximumIncome = 18200, Tax = 0, FixedTax = 0},
                                  new TaxBracket() { MaximumIncome = 37000, Tax = 0.19, FixedTax = 0},
                                  new TaxBracket() { MaximumIncome = 80000, Tax = 0.325, FixedTax = 3572},
                                  new TaxBracket() { MaximumIncome = -1, Tax = 0.37, FixedTax = 17547}
                              };

            _employeeRepository = Substitute.For<IRepository<Employee>>();
            _employeeRepository.FindAll().ReturnsForAnyArgs(_employees);

            _taxTableRepository = Substitute.For<IRepository<TaxBracket>>();
            _taxTableRepository.FindAll().ReturnsForAnyArgs(taxBrackets); 

            _payslipGenerator = new PayslipGenerator(_employeeRepository, _taxTableRepository);
        }

        [TestMethod]
        public void GivenFirstNameAndLastNameThenPayslipNameIsGeneratedCorrectly()
        {
            var payslips = _payslipGenerator.GetPayslips();
            Assert.AreEqual($"{_firstName} {_lastName}", payslips.First().Name);
        }

        [TestMethod]
        public void GivenAnnualSalaryThenPayslipGrossIncomeIsRoundedDownCorrectly()
        {
            _employees.First().AnnualSalary = (int)(12 * 5004.49);
            var payslips = _payslipGenerator.GetPayslips();

            Assert.AreEqual(5004, payslips.First().GrossIncome);
        }

        [TestMethod]
        public void GivenAnnualSalaryThenPayslipGrossIncomeIsRoundedUpCorrectly()
        {
            _employees.First().AnnualSalary = (int)(12 * 5004.50);

            var payslips = _payslipGenerator.GetPayslips();

            Assert.AreEqual(5005, payslips.First().GrossIncome);
        }

        [TestMethod]
        public void GivenAnnualSalaryAndSuperRateThenPayslipSuperIsRoundedDownCorrectly()
        {
            _employees.First().SuperRate = (410.49 * 12) / _employees.First().AnnualSalary;

            var payslips = _payslipGenerator.GetPayslips();

            Assert.AreEqual(410, payslips.First().Super);
        }

        [TestMethod]
        public void GivenAnnualSalaryAndSuperRateThenPayslipSuperIsRoundedUpCorrectly()
        {
            _employees.First().SuperRate = (410.50 * 12) / _employees.First().AnnualSalary;

            var payslips = _payslipGenerator.GetPayslips();

            Assert.AreEqual(411, payslips.First().Super);
        }

        [TestMethod]
        public void GivenPaymentStartDateThenPayPeriodIsCorrect()
        {
            var payslips = _payslipGenerator.GetPayslips();

            Assert.AreEqual(PaymentDate, payslips.First().PayPeriod);
        }

        [TestMethod]
        public void GivenAnnualSalaryIsInLowestTaxBracketThenIncomeTaxIsCorrect()
        {
            _employees.First().AnnualSalary = 18200;

            var payslips = _payslipGenerator.GetPayslips();
            Assert.AreEqual(0, payslips.First().IncomeTax);
        }

        [TestMethod]
        public void GivenAnnualSalaryIsIn2ndTaxBracketThenIncomeTaxIsCorrect()
        {
            var annualSalary = 37000;
            _employees.First().AnnualSalary = annualSalary;

            var payslips = _payslipGenerator.GetPayslips();

            var expectedTax = (int)(Math.Round((.19 * (annualSalary - 18200)) / 12, MidpointRounding.AwayFromZero));
            Assert.AreEqual(expectedTax, payslips.First().IncomeTax);
        }

        [TestMethod]
        public void GivenAnnualSalaryIsIn3rdTaxBracketThenIncomeTaxIsCorrect()
        {
            var annualSalary = 60050;
            _employees.First().AnnualSalary = annualSalary;

            var payslips = _payslipGenerator.GetPayslips();

            var expectedTax = (int)(Math.Round((3572 + (.325 * (annualSalary - 37000))) / 12, MidpointRounding.AwayFromZero));
            Assert.AreEqual(expectedTax, payslips.First().IncomeTax);
        }

        [TestMethod]
        public void GivenAnnualSalaryIsInLastTaxBracketThenIncomeTaxIsCorrect()
        {
            var annualSalary = 100000;
            _employees.First().AnnualSalary = annualSalary;

            var payslips = _payslipGenerator.GetPayslips();

            var expectedTax = (int)(Math.Round((17547 + (.37 * (annualSalary - 80000))) / 12, MidpointRounding.AwayFromZero));
            Assert.AreEqual(expectedTax, payslips.First().IncomeTax);
        }
    }
}
