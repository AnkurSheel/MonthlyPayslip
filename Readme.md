**Monthly Payslip for MYOB**

**Usage:** MonthlyPayslip.ConsoleApp.exe _employeeCsvFile taxtableCsvFile payslipsCsvFile_

**Assumptions**

EmployeeCsvFile has data in the following format   
FirstName,LastName,AnnualSalary,SuperRate(%),PaymentStartDate  
**Example:** David,Rudd,60050,9%,01 March – 31 March

TaxTableCsvFile has data in the following format   
MaximumIncome,Tax,FixedTax  
**Example:** 80000,0.325, 3572  
For Taxbracket like 180,000 and over, MaximumIncome should be -1

There is no data validation, It is expected to be valid
