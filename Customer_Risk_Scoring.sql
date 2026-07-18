/* ============================================================
   CUSTOMER RISK SCORING SYSTEM: A credit risk scoring database

   These are the business rules I used when making this:
	   1. Each customer has a unique ID and profile including their name, date of birth, income, and employment status
	   2. A customer can have multiple accounts, but each account belongs to only one customer
	   3. Each account has a type (Credit Card, Auto Loan, Mortgage, Personal Loan, or Student Loan), a credit limit, a current balance, and a status
	   4. Account balances cannot be negative and credit limits must be greater than zero
	   5. Each transaction belongs to one account and must have a non-zero amount
	   6. Every billing cycle, a payment record is created showing what was owed, what was paid, and how many days late the payment was
	   7. Credit utilization snapshots are taken periodically to record the ratio of balance to credit limit for each account, and the rate must be between 0 and 1
	   8. Engineered features are aggregated per account to summarize spending behavior, late payments, and utilization over time for use in scoring
	   9. Each customer receives a numeric risk score between 300 and 850 at a point in time
	  10. Every risk score is assigned a tier code (POOR, FAIR, GOOD, VGOOD, or EXCL) that references the RiskTier lookup table, ensuring only valid tiers can be assigned
	  11. The RiskTier table defines the score ranges for each tier and cannot overlap
   ============================================================ */


/* This is to create & use the database */
CREATE DATABASE CreditRiskDB;
GO

USE CreditRiskDB;
GO

/* I'll start creating the tables from here in the dependency order */

/* Table 1: RiskTier - This is a lookup table for risk tier labels and score ranges. 
			It's referenced by RiskScore as an FK. */
CREATE TABLE RiskTier (
	RiskTierCode VARCHAR(10) NOT NULL,
	TierLabel VARCHAR(20) NOT NULL,
	MinScore INT NOT NULL,
	MaxScore INT NOT NULL,
	Description VARCHAR(200) NULL,
	CONSTRAINT PK_RiskTier PRIMARY KEY (RiskTierCode),
	CONSTRAINT CK_RiskTier_Range CHECK (MinScore >= 300 AND MAXSCORE <= 850 AND MinScore < MaxScore)
);

/* Table 2: Customer - There's one row per person, storing demographics and 
   their profile data for each. */
CREATE TABLE Customer (
	CustomerID INT NOT NULL IDENTITY(1,1),
	FullName VARCHAR(100) NOT NULL,
	DOB DATE NOT NULL,
	Income DECIMAL(12,2) NULL,
	EmploymentStatus VARCHAR(30) NOT NULL,
	CreatedDate DATE NOT NULL DEFAULT GETDATE(),
	CONSTRAINT PK_Customer PRIMARY KEY (CustomerID),
	CONSTRAINT CK_Customer_Emp CHECK (EmploymentStatus IN ('Employed', 'Self-Employed', 'Unemployed', 'Retired', 'Student')),
	CONSTRAINT CK_Customer_DOB CHECK (DOB < GETDATE())
);

/* Table 3: Account - There is one account per row, linked to a customer.
   A rule to keep in mind is that a customer can have multiple accounts. */
CREATE TABLE Account (
	AccountID INT NOT NULL IDENTITY(1,1),
	CustomerID INT NOT NULL,
	AccountType VARCHAR(20) NOT NULL,
	CreditLimit DECIMAL(10,2) NOT NULL,
	Balance DECIMAL(10,2) NOT NULL,
	OpenedDate DATE NOT NULL,
	Status VARCHAR(15) NOT NULL DEFAULT 'Active',
	CONSTRAINT PK_Account PRIMARY KEY (AccountID),
	CONSTRAINT FK_Account_Customer FOREIGN KEY (CustomerID)
		REFERENCES Customer(CustomerID) ON UPDATE CASCADE ON DELETE CASCADE,
	CONSTRAINT CK_Account_Type CHECK (AccountType IN ('Credit Card', 'Auto Loan', 'Mortgage', 'Personal Loan', 'Student Loan')),
	CONSTRAINT CK_Account_Status CHECK (Status IN ('Active', 'Closed', 'Delinquent', 'Frozen')),
	CONSTRAINT CK_Account_Limit CHECK (CreditLimit > 0),
	CONSTRAINT CK_Account_Balance CHECK (Balance >=0)
);

/* Table 4: TransactionHistory - These are the individual transactions on an account.
   So each row could have one credit or debt action/event. */
CREATE TABLE TransactionHistory (
	TransactionID INT NOT NULL IDENTITY(1,1),
	AccountID INT NOT NULL,
	TransactionDate DATE NOT NULL,
	Amount DECIMAL(10,2) NOT NULL,
	TransactionType VARCHAR(20) NOT NULL,
	Description VARCHAR(200) NULL,
	CONSTRAINT PK_TransactionHistory PRIMARY KEY (TransactionID),
	CONSTRAINT FK_TransactionHisotry_Account FOREIGN KEY (AccountID)
		REFERENCES Account(AccountID) ON UPDATE CASCADE ON DELETE CASCADE,
	CONSTRAINT CK_Transaction_Type CHECK (TransactionTYpe IN ('Purchase', 'Payment', 'Fee', 'Interest', 'Refund')),
	CONSTRAINT CK_Transaction_Amount CHECK (Amount <> 0)
);

/* Table 5: PaymentHistory - There is one row per billing cycle for each account.
   So this is to track what's due vs. what's been paid. */
CREATE TABLE PaymentHistory (
	PaymentID INT NOT NULL IDENTITY(1,1),
	AccountID INT NOT NULL,
	BillingCycleDate DATE NOT NULL,
	AmountDue DECIMAL(10,2) NOT NULL,
	AmountPaid DECIMAL(10,2) NOT NULL DEFAULT 0,
	DaysLate INT NOT NULL DEFAULT 0,
	CONSTRAINT PK_PaymentHistory PRIMARY KEY (PaymentID),
	CONSTRAINT FK_PaymentHisory_Account FOREIGN KEY (AccountID)
		REFERENCES Account(AccountID) ON UPDATE CASCADE ON DELETE CASCADE,
	CONSTRAINT CK_PaymentHistory_Due CHECK (AmountDue >= 0),
	CONSTRAINT CK_PaymentHistory_Paid CHECK (AmountPaid >= 0),
	CONSTRAINT CK_PaymentHistory_Late CHECK (DaysLate >= 0)
);

/* Table 6: CreditUtilization - this is for periodic snapshots of credit utilization for
   each account. I'm using the formula: Utilization = Balance/CreditLimit */
CREATE TABLE CreditUtilization (
	CreditUtilizationID INT NOT NULL IDENTITY(1,1),
	AccountID INT NOT NULL,
	SnapshotDate DATE NOT NULL,
	UtilizationRate DECIMAL(5,4) NOT NULL,
	SnapshotBalance DECIMAL(10,2) NOT NULL,
	CONSTRAINT PK_CreditUtilization PRIMARY KEY (CreditUtilizationID),
	CONSTRAINT FK_CreditUtilization_Account FOREIGN KEY (AccountID)
		REFERENCES Account(AccountID) ON UPDATE CASCADE ON DELETE CASCADE,
	CONSTRAINT CK_Utilization_Rate CHECK (UtilizationRate >= 0 AND UtilizationRate <=1),
	CONSTRAINT CK_Utilization_Balance CHECK (SnapshotBalance >= 0)

);

/* Table 7: FeatureAggregate - This is engineering features for each account by using 
   the scoring inputs. Mainly aggregated from the transaction and payment data. */
CREATE TABLE FeatureAggregate (
	FeatureID INT NOT NULL IDENTITY(1,1),
	AccountID INT NOT NULL,
	ComputedDate DATE NOT NULL,
	AvgMonthlySpend DECIMAL(10,2) NOT NULL DEFAULT 0,
	LatePaymentCount INT NOT NULL DEFAULT 0,
	MissedPaymentCount INT NOT NULL DEFAULT 0,
	AvgUtilizationRate DECIMAL(5,4) NOT NULL DEFAULT 0,
	TotalTransactions INT NOT NULL DEFAULT 0,
	CONSTRAINT PK_FeatureAggregate PRIMARY KEY (FeatureID),
	CONSTRAINT FK_FeatureAggregate_Account FOREIGN KEY (AccountID)
		REFERENCES Account(AccountID) ON UPDATE CASCADE ON DELETE CASCADE,
	CONSTRAINT CK_Feature_LateCount CHECK (LatePaymentCount >= 0),
	CONSTRAINT CK_Feature_MissedCount CHECK (MissedPaymentCount >= 0 ),
	CONSTRAINT CK_Feature_AvgUtil CHECK (AvgUtilizationRate >= 0 AND AvgUtilizationRate <= 1)
);


/* Table 8: RiskScore - This gives the risk score for a customer at a point in time. 
   It's referencing RiskTier as a FK in order to enforce the valid tier codes. */ 
CREATE TABLE RiskScore (
	RiskScoreID INT NOT NULL IDENTITY(1,1),
	CustomerID INT NOT NULL,
	NumericScore INT NOT NULL,
	RiskTierCode VARCHAR(10) NOT NULL,
	ScoreDate DATE NOT NULL,
	CONSTRAINT PK_RiskScore PRIMARY KEY (RiskScoreID),
	CONSTRAINT FK_RiskScore_Customer FOREIGN KEY (CustomerID)
	REFERENCES Customer(CustomerID) ON UPDATE CASCADE ON DELETE CASCADE,
	CONSTRAINT FK_RiskScore_Tier FOREIGN KEY (RiskTierCode) 
	REFERENCES RiskTier(RiskTierCode),
	CONSTRAINT CK_RiskScore_Score CHECK (NumericScore BETWEEN 300 AND 850)
);


/* VIEW - vCustomerRiskSummary: This is suppose to return the most recent risk score
   for each customer, as well as their account summary. I feel like this would be
   useful for creating dashboards or risk analyst review. */
GO 
CREATE VIEW vCustomerRiskSummary AS
SELECT 
	c.CustomerID,
	c.FullName, 
	c.EmploymentStatus,
	rs.NumericScore,
	rs.RiskTierCode, 
	rt.TierLabel,
	rs.ScoreDate,
	COUNT(DISTINCT a.AccountID) AS TotalAccounts,
	SUM(a.Balance) AS TotalBalance,
	SUM(a.CreditLimit) AS TotalCreditLimit
FROM Customer c 
JOIN RiskScore rs
	ON c.CustomerID = rs.CustomerID
JOIN RiskTier rt
	ON rs.RiskTierCode = rt.RiskTierCode
JOIN Account a
	ON c.CustomerID = a.CustomerID
WHERE rs.ScoreDate = (
	SELECT MAX(rs2.ScoreDate)
	FROM RiskScore rs2
	WHERE rs2.CustomerID = c.CustomerID )
GROUP BY 
	c.CustomerID,
	c.FullName,
	c.EmploymentStatus,
	rs.NumericScore,
	rs.RiskTierCode,
	rt.TierLabel,
	rs.ScoreDate;
GO

/* USER-DEFINED FUNCTION - udf_GetRiskTierCode: This takes a numeric score as the input
   and then returns the corresponding RiskTierCode as the output. 
   I feel like this would be useful if you need to insert a new RiskScore row to 
   auto-assign the correct tier based on the score value. */
DROP FUNCTION IF EXISTS dbo.udf_GetRiskTierCode;
GO

CREATE FUNCTION dbo.udf_GetRiskTierCode (@Score INT)
RETURNS VARCHAR(10)
AS
BEGIN 
	DECLARE @TierCode VARCHAR(10);
	SELECT @TierCode = RiskTierCode
	FROM RiskTier
	WHERE @Score BETWEEN MinScore AND MaxScore;
	IF @TierCode IS NULL
		SET @TierCode = 'UNKNOWN';
	RETURN @TierCode;
END;
GO

/* STORED PROCEDURE - usp_GetCustomerRiskReport: this will reutrn a full risk report for
   the given customer like their profile, most recent risk score, payment history summary,
   and all accounts. 
   It'll accept the CustomerID as a parameter and if -1 is passed, then it returns the data
   for all the customers. */
DROP PROCEDURE IF EXISTS dbo.usp_GetCustomerRiskReport;
GO

CREATE PROCEDURE dbo.usp_GetCustomerRiskReport
	@CustomerID INT = -1
AS 
BEGIN
	SET NOCOUNT ON;

	SELECT 
		c.CustomerID,
		c.FullName,
		c.EmploymentStatus,
		c.Income,
		a.AccountID,
		a.AccountType,
		a.CreditLimit,
		a.Balance,
		a.Status AS AccountStatus,
		rs.NumericScore AS LatestScore,
		rt.TierLabel AS RiskTier,
		rs.ScoreDate,
		SUM(ph.DaysLate) AS TotalDaysLate,
		COUNT(CASE WHEN ph.AmountPaid < ph.AmountDue THEN 1 END) AS MissedPayments
	FROM Customer c
	JOIN Account a
		ON c.CustomerID = a.CustomerID
	LEFT JOIN RiskScore rs
		ON c.CustomerID = rs.CustomerID
		AND rs.ScoreDate = (
		SELECT MAX(rs2.ScoreDate)
		FROM RiskScore rs2
		WHERE rs2.CustomerID = c.CustomerID)
	LEFT JOIN RiskTier rt
		ON rs.RiskTierCode = rt.RiskTierCode
	LEFT JOIN PaymentHistory ph
		ON a.AccountID = ph.AccountID
	WHERE @CustomerID = -1
		OR c.CustomerID = @CustomerID
	GROUP BY 
		c.CustomerID, c.FullName, c.EmploymentStatus, c.Income, a.AccountID, a.AccountType,
		a.CreditLimit, a.Balance, a.Status, rs.NumericScore, rt.TierLabel, rs.ScoreDate
	ORDER BY 
		c.CustomerID, a.AccountID;

	RETURN 0;
END;
GO

/* SAMPLE DATA STARTS HERE - 20+ ROWS PER TABLE  */


/* --- RiskTier (lookup table, populated first) --- */
INSERT INTO RiskTier VALUES ('EXCL',  'Exceptional',  800, 850, 'Lowest risk. Best rates available.');
INSERT INTO RiskTier VALUES ('VGOOD', 'Very Good',    740, 799, 'Very low risk. Favorable terms.');
INSERT INTO RiskTier VALUES ('GOOD',  'Good',         670, 739, 'Low to moderate risk. Standard terms.');
INSERT INTO RiskTier VALUES ('FAIR',  'Fair',         580, 669, 'Moderate risk. Higher rates likely.');
INSERT INTO RiskTier VALUES ('POOR',  'Poor',         300, 579, 'High risk. Limited credit options.');

/* --- Customer (20 rows) --- */
INSERT INTO Customer (FullName, DOB, Income, EmploymentStatus, CreatedDate) VALUES
('Alice Monroe',      '1990-04-15', 72000.00,  'Employed',      '2021-01-10'),
('Brian Carter',      '1985-08-22', 95000.00,  'Employed',      '2020-06-15'),
('Carmen Diaz',       '1998-01-30', 45000.00,  'Employed',      '2022-03-01'),
('David Kim',         '1978-11-05', 130000.00, 'Self-Employed', '2019-09-20'),
('Elena Rossi',       '1992-07-18', 58000.00,  'Employed',      '2021-07-14'),
('Frank Torres',      '1988-03-25', 61000.00,  'Employed',      '2020-11-30'),
('Grace Lee',         '2000-09-09', 32000.00,  'Student',       '2023-01-05'),
('Henry Walsh',       '1975-12-01', 148000.00, 'Employed',      '2018-04-22'),
('Iris Nakamura',     '1995-06-14', 77000.00,  'Employed',      '2021-10-17'),
('James Okafor',      '1982-02-28', 88000.00,  'Self-Employed', '2020-02-08'),
('Karen Patel',       '1993-10-11', 54000.00,  'Employed',      '2022-06-19'),
('Leo Simmons',       '1970-05-30', 42000.00,  'Retired',       '2019-08-03'),
('Maya Johnson',      '1999-08-21', 39000.00,  'Employed',      '2023-03-15'),
('Nolan Pierce',      '1987-01-17', 102000.00, 'Employed',      '2020-05-27'),
('Olivia Grant',      '1994-04-04', 67000.00,  'Employed',      '2021-12-09'),
('Paul Reeves',       '1980-07-07', 115000.00, 'Employed',      '2019-01-14'),
('Quinn Foster',      '2001-11-23', 28000.00,  'Student',       '2023-06-01'),
('Rachel Huang',      '1989-09-13', 83000.00,  'Employed',      '2020-08-18'),
('Samuel Brooks',     '1976-03-19', 55000.00,  'Unemployed',    '2022-09-25'),
('Tina Yamamoto',     '1997-12-06', 71000.00,  'Employed',      '2021-04-30');

/* --- Account (20 rows, spread across customers) --- */
INSERT INTO Account (CustomerID, AccountType, CreditLimit, Balance, OpenedDate, Status) VALUES
(1,  'Credit Card',   5000.00,  1200.00,  '2021-02-01', 'Active'),
(2,  'Mortgage',      250000.00, 198000.00,'2020-07-01', 'Active'),
(3,  'Credit Card',   3000.00,  2800.00,  '2022-04-01', 'Active'),
(4,  'Auto Loan',     35000.00, 18000.00, '2021-01-15', 'Active'),
(5,  'Personal Loan', 10000.00, 4500.00,  '2021-08-01', 'Active'),
(6,  'Credit Card',   7500.00,  500.00,   '2020-12-01', 'Active'),
(7,  'Student Loan',  20000.00, 19500.00, '2023-02-01', 'Active'),
(8,  'Mortgage',      500000.00, 321000.00,'2018-05-01', 'Active'),
(9,  'Credit Card',   8000.00,  3200.00,  '2021-11-01', 'Active'),
(10, 'Auto Loan',     28000.00, 11000.00, '2020-03-01', 'Active'),
(11, 'Credit Card',   4000.00,  3900.00,  '2022-07-01', 'Delinquent'),
(12, 'Personal Loan', 6000.00,  2000.00,  '2019-09-01', 'Active'),
(13, 'Credit Card',   2500.00,  2400.00,  '2023-04-01', 'Active'),
(14, 'Mortgage',      320000.00, 289000.00,'2020-06-01', 'Active'),
(15, 'Auto Loan',     22000.00, 9500.00,  '2022-01-01', 'Active'),
(16, 'Credit Card',   15000.00, 2000.00,  '2019-02-01', 'Active'),
(17, 'Student Loan',  15000.00, 14800.00, '2023-07-01', 'Active'),
(18, 'Credit Card',   10000.00, 1500.00,  '2020-09-01', 'Active'),
(19, 'Personal Loan', 8000.00,  7500.00,  '2022-10-01', 'Delinquent'),
(20, 'Credit Card',   6000.00,  1800.00,  '2021-05-01', 'Active');

/* --- TransactionHistory (20 rows) --- */
INSERT INTO TransactionHistory (AccountID, TransactionDate, Amount, TransactionType, Description) VALUES
(1,  '2025-01-05', 250.00,   'Purchase',  'Amazon Online'),
(1,  '2025-01-15', -250.00,  'Payment',   'Monthly payment'),
(2,  '2025-01-01', -1500.00, 'Payment',   'Mortgage payment'),
(3,  '2025-01-10', 400.00,   'Purchase',  'Grocery store'),
(4,  '2025-01-01', -450.00,  'Payment',   'Auto loan payment'),
(5,  '2025-01-08', 200.00,   'Purchase',  'Medical bill'),
(6,  '2025-01-12', 85.00,    'Purchase',  'Gas station'),
(7,  '2025-01-03', -300.00,  'Payment',   'Student loan payment'),
(8,  '2025-01-01', -2800.00, 'Payment',   'Mortgage payment'),
(9,  '2025-01-18', 650.00,   'Purchase',  'Electronics'),
(10, '2025-01-02', -550.00,  'Payment',   'Auto loan payment'),
(11, '2025-01-20', 180.00,   'Fee',       'Late fee assessed'),
(12, '2025-01-10', -200.00,  'Payment',   'Personal loan payment'),
(13, '2025-01-15', 120.00,   'Purchase',  'Restaurant'),
(14, '2025-01-01', -2100.00, 'Payment',   'Mortgage payment'),
(15, '2025-01-05', -400.00,  'Payment',   'Auto loan payment'),
(16, '2025-01-22', 350.00,   'Purchase',  'Department store'),
(17, '2025-01-03', -150.00,  'Payment',   'Student loan payment'),
(18, '2025-01-11', 500.00,   'Purchase',  'Home improvement'),
(19, '2025-01-25', 75.00,    'Interest',  'Interest charged');

/* --- PaymentHistory (20 rows) --- */
INSERT INTO PaymentHistory (AccountID, BillingCycleDate, AmountDue, AmountPaid, DaysLate) VALUES
(1,  '2025-01-01', 250.00,   250.00,   0),
(2,  '2025-01-01', 1500.00,  1500.00,  0),
(3,  '2025-01-01', 400.00,   200.00,   15),
(4,  '2025-01-01', 450.00,   450.00,   0),
(5,  '2025-01-01', 200.00,   200.00,   0),
(6,  '2025-01-01', 85.00,    85.00,    0),
(7,  '2025-01-01', 300.00,   300.00,   0),
(8,  '2025-01-01', 2800.00,  2800.00,  0),
(9,  '2025-01-01', 650.00,   400.00,   10),
(10, '2025-01-01', 550.00,   550.00,   0),
(11, '2025-01-01', 300.00,   0.00,     30),
(12, '2025-01-01', 200.00,   200.00,   0),
(13, '2025-01-01', 120.00,   50.00,    22),
(14, '2025-01-01', 2100.00,  2100.00,  0),
(15, '2025-01-01', 400.00,   400.00,   0),
(16, '2025-01-01', 350.00,   350.00,   0),
(17, '2025-01-01', 150.00,   150.00,   0),
(18, '2025-01-01', 500.00,   500.00,   0),
(19, '2025-01-01', 350.00,   0.00,     45),
(20, '2025-01-01', 180.00,   180.00,   0);

/* --- CreditUtilization (20 rows) --- */
INSERT INTO CreditUtilization (AccountID, SnapshotDate, UtilizationRate, SnapshotBalance) VALUES
(1,  '2025-01-31', 0.2400, 1200.00),
(2,  '2025-01-31', 0.7920, 198000.00),
(3,  '2025-01-31', 0.9333, 2800.00),
(4,  '2025-01-31', 0.5143, 18000.00),
(5,  '2025-01-31', 0.4500, 4500.00),
(6,  '2025-01-31', 0.0667, 500.00),
(7,  '2025-01-31', 0.9750, 19500.00),
(8,  '2025-01-31', 0.6420, 321000.00),
(9,  '2025-01-31', 0.4000, 3200.00),
(10, '2025-01-31', 0.3929, 11000.00),
(11, '2025-01-31', 0.9750, 3900.00),
(12, '2025-01-31', 0.3333, 2000.00),
(13, '2025-01-31', 0.9600, 2400.00),
(14, '2025-01-31', 0.9031, 289000.00),
(15, '2025-01-31', 0.4318, 9500.00),
(16, '2025-01-31', 0.1333, 2000.00),
(17, '2025-01-31', 0.9867, 14800.00),
(18, '2025-01-31', 0.1500, 1500.00),
(19, '2025-01-31', 0.9375, 7500.00),
(20, '2025-01-31', 0.3000, 1800.00);

/* --- FeatureAggregate (20 rows) --- */
INSERT INTO FeatureAggregate (AccountID, ComputedDate, AvgMonthlySpend, LatePaymentCount, MissedPaymentCount, AvgUtilizationRate, TotalTransactions) VALUES
(1,  '2025-01-31', 250.00,   0, 0, 0.2400, 12),
(2,  '2025-01-31', 1500.00,  0, 0, 0.7920, 24),
(3,  '2025-01-31', 420.00,   2, 0, 0.8800, 18),
(4,  '2025-01-31', 450.00,   0, 0, 0.5143, 24),
(5,  '2025-01-31', 200.00,   0, 0, 0.4200, 10),
(6,  '2025-01-31', 90.00,    0, 0, 0.0550, 15),
(7,  '2025-01-31', 300.00,   0, 0, 0.9600, 6),
(8,  '2025-01-31', 2800.00,  0, 0, 0.6200, 84),
(9,  '2025-01-31', 680.00,   1, 0, 0.3800, 20),
(10, '2025-01-31', 550.00,   0, 0, 0.3750, 24),
(11, '2025-01-31', 300.00,   3, 1, 0.9500, 8),
(12, '2025-01-31', 210.00,   0, 0, 0.3100, 18),
(13, '2025-01-31', 130.00,   2, 0, 0.9200, 9),
(14, '2025-01-31', 2100.00,  0, 0, 0.8900, 60),
(15, '2025-01-31', 410.00,   0, 0, 0.4200, 24),
(16, '2025-01-31', 380.00,   0, 0, 0.1200, 36),
(17, '2025-01-31', 155.00,   0, 0, 0.9700, 6),
(18, '2025-01-31', 520.00,   0, 0, 0.1400, 18),
(19, '2025-01-31', 360.00,   4, 2, 0.9200, 12),
(20, '2025-01-31', 190.00,   0, 0, 0.2900, 14);

/* --- RiskScore (20 rows) --- */
INSERT INTO RiskScore (CustomerID, NumericScore, RiskTierCode, ScoreDate) VALUES
(1,  742, 'VGOOD', '2025-01-31'),
(2,  810, 'EXCL',  '2025-01-31'),
(3,  605, 'FAIR',  '2025-01-31'),
(4,  780, 'VGOOD', '2025-01-31'),
(5,  690, 'GOOD',  '2025-01-31'),
(6,  755, 'VGOOD', '2025-01-31'),
(7,  620, 'FAIR',  '2025-01-31'),
(8,  835, 'EXCL',  '2025-01-31'),
(9,  710, 'GOOD',  '2025-01-31'),
(10, 765, 'VGOOD', '2025-01-31'),
(11, 530, 'POOR',  '2025-01-31'),
(12, 698, 'GOOD',  '2025-01-31'),
(13, 560, 'POOR',  '2025-01-31'),
(14, 788, 'VGOOD', '2025-01-31'),
(15, 720, 'GOOD',  '2025-01-31'),
(16, 820, 'EXCL',  '2025-01-31'),
(17, 615, 'FAIR',  '2025-01-31'),
(18, 775, 'VGOOD', '2025-01-31'),
(19, 498, 'POOR',  '2025-01-31'),
(20, 730, 'GOOD',  '2025-01-31');