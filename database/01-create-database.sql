/* Ejecutar en una instancia de SQL Server con permisos para crear bases de datos. */
IF DB_ID(N'VentasDemo') IS NULL
    CREATE DATABASE VentasDemo;
GO

USE VentasDemo;
GO

IF OBJECT_ID(N'dbo.SaleConcepts', N'U') IS NOT NULL DROP TABLE dbo.SaleConcepts;
IF OBJECT_ID(N'dbo.Sales', N'U') IS NOT NULL DROP TABLE dbo.Sales;
GO

CREATE TABLE dbo.Sales
(
    SaleId       INT IDENTITY(1,1) NOT NULL CONSTRAINT PK_Sales PRIMARY KEY,
    CustomerName NVARCHAR(120) NOT NULL,
    CreatedAt    DATETIME2(0) NOT NULL CONSTRAINT DF_Sales_CreatedAt DEFAULT SYSUTCDATETIME(),
    CONSTRAINT CK_Sales_CustomerName_NotEmpty CHECK (LEN(LTRIM(RTRIM(CustomerName))) > 0)
);
GO

CREATE TABLE dbo.SaleConcepts
(
    SaleConceptId INT IDENTITY(1,1) NOT NULL CONSTRAINT PK_SaleConcepts PRIMARY KEY,
    SaleId        INT NOT NULL,
    ProductName   NVARCHAR(160) NOT NULL,
    Quantity      INT NOT NULL,
    UnitPrice     DECIMAL(12,2) NOT NULL,
    CONSTRAINT FK_SaleConcepts_Sales FOREIGN KEY (SaleId) REFERENCES dbo.Sales(SaleId),
    CONSTRAINT CK_SaleConcepts_ProductName_NotEmpty CHECK (LEN(LTRIM(RTRIM(ProductName))) > 0),
    CONSTRAINT CK_SaleConcepts_Quantity_Positive CHECK (Quantity > 0),
    CONSTRAINT CK_SaleConcepts_UnitPrice_NonNegative CHECK (UnitPrice >= 0)
);
GO

CREATE INDEX IX_SaleConcepts_SaleId ON dbo.SaleConcepts(SaleId);
GO
