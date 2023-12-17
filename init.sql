IF NOT EXISTS (SELECT 1 FROM sys.databases WHERE name = 'InterviewTest')
BEGIN
    CREATE DATABASE InterviewTest;
END;

-- Switch to the new database context
USE InterviewTest;

-- Create tables and schema
CREATE TABLE [dbo].[Product] (
    [ProductNumber] VARCHAR(50) PRIMARY KEY,
    [SellingPrice] FLOAT
);

CREATE TABLE [dbo].[Orders] (
    [CustomerName] VARCHAR(50),
    [OrderNumber] VARCHAR(50) PRIMARY KEY,
    [Timestamp] DATETIME DEFAULT GETDATE()
);

CREATE TABLE [dbo].[OrderProducts] (
    [Id] INT IDENTITY(1,1) PRIMARY KEY,
    [OrderNumber] VARCHAR(50),
    [ProductNumber] VARCHAR(50),
    CONSTRAINT existing_product FOREIGN KEY ([ProductNumber]) REFERENCES [dbo].[Product] ([ProductNumber]),
    CONSTRAINT existing_order FOREIGN KEY ([OrderNumber]) REFERENCES [dbo].[Orders] ([OrderNumber])
);

CREATE TABLE [dbo].[Return] (
    [Id] INT IDENTITY(1,1) PRIMARY KEY,
    [ReturnOrder] VARCHAR(50),
    [ProductNumber] VARCHAR(50),
    CONSTRAINT existing_product_return FOREIGN KEY ([ProductNumber]) REFERENCES [dbo].[Product] ([ProductNumber]),
    CONSTRAINT existing_order_return FOREIGN KEY ([ReturnOrder]) REFERENCES [dbo].[Orders] ([OrderNumber])
);

INSERT INTO [dbo].[Product] ([ProductNumber], [SellingPrice]) VALUES
    ('Rugged Liner F55U15', 150),
    ('Mobil 1 5W-30', 25),
    ('DrawTite 5504', 70),
    ('Sherman 036-87-1', 155);