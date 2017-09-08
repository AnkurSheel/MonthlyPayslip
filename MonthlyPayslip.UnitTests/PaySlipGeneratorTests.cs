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
        private IRepository<Employee> _employeeRepository;
        private PayslipGenerator _payslipGenerator;
        private string _firstName;
        private string _lastName;
        private List<Employee> _employees;

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
                             }
                         };

            _employeeRepository = Substitute.For<IRepository<Employee>>();
            _employeeRepository.FindAll().ReturnsForAnyArgs(_employees);

            _payslipGenerator = new PayslipGenerator(_employeeRepository);
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
    }
}
