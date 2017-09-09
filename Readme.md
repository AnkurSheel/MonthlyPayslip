**Monthly Payslip for MYOB**

**Usage:** MonthlyPayslip.ConsoleApp.exe _employeeCsvFile taxtableCsvFile payslipsCsvFile_

**Assumptions**

EmployeeCsvFile has data in the following format   
FirstName,LastName,AnnualSalary,SuperRate(%),PaymentStartDate  
**Example:** David,Rudd,60050,9%,01 March – 31 March

TaxTableCsvFile has data in the following format  
MaximumIncome,Tax,FixedTax  
**Example:** 80000,0.325, 3572  
For Taxbracket lwhich has no upper limit, MaximumIncome should be -1

Payslip information will be written to payslipsCsvFile and outputted to console.  
If the file exists it will be overwritten  
payslipsCsvFile has the following format  
Name,PayPeriod,GrossIncome,IncomeTax,NetIncome,Super  
**Example:** David Rudd,01 March – 31 March,5004,922,4082,450

There is no data validation, It is expected to be valid
