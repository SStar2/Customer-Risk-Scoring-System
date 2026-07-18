# Customer-Risk-Scoring-System
Tools: SQL, VB.NET, Visual Studio, SSRS, Lucidchart

Overview:
A FICO-style customer risk scoring system built to model how financial institutions assess credit risk. The system includes a fully normalized relational database, a Windows Forms front end, and SSRS reports for risk analysis.


Database Design:

    ● 8 normalized tables: Customer, Account, TransactionHistory, PaymentHistory, CreditUtilization, FeatureAggregate, RiskScore, RiskTier
    
    ● 20 rows of sample data per table
    
    ● Includes a view, a user-defined function, and a stored procedure
    
    ● Schema validated in Lucidchart ER diagram


Windows Forms (VB.NET) - Built 4 forms in Visual Studio:

    ● Customer Detail — displays individual customer information
    
    ● Portfolio Grid — grid view of all customers and risk data
    
    ● Search by Tier — filters customers by assigned risk tier
    
    ● Customer Profile Tabs — tabbed view of full customer profile
    
    ● Startup Form — connects and launches all four forms


Key Technical Challenges Solved:

  ● Resolved null reference errors from connection string issues
  
  ● Fixed NuGet reference errors and VB vs C# syntax conflicts
  
  ● Debugged overlapping controls and grid column truncation
  
  ● Validated all table cardinalities in the ER diagram
