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

        [TestInitialize]
        public void Setup()
        {
            _firstName = "David";
            _lastName = "Rudd";

            var employees = new List<Employee>()
                            {
                                new Employee() { FirstName = _firstName, LastName = _lastName },
                            };

            _employeeRepository = Substitute.For<IRepository<Employee>>();
            _employeeRepository.FindAll().ReturnsForAnyArgs(employees);

            _payslipGenerator = new PayslipGenerator(_employeeRepository);
        }

        [TestMethod]
        public void GivenFirstNameAndLastNameThenPayslipNameIsGeneratedCorrectly()
        {
            var payslips = _payslipGenerator.GetPayslips();
            Assert.AreEqual($"{_firstName} {_lastName}", payslips.Single().Name);
        }
    }
}
