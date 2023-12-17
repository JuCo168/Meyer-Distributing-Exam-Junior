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
    CONSTRAINT existing_product FOREIGN KEY ([ProductNumber]) REFERENCES [dbo].[Product] ([ProductNumber]) ON DELETE CASCADE,
    CONSTRAINT existing_order FOREIGN KEY ([OrderNumber]) REFERENCES [dbo].[Orders] ([OrderNumber]) ON DELETE CASCADE
);

CREATE TABLE [dbo].[Returns] (
    [ReturnNumber] VARCHAR(50) PRIMARY KEY,
    [CustomerName] VARCHAR(50),
    [OriginalOrder] VARCHAR(50),
    [Timestamp] DATETIME DEFAULT GETDATE(),
    CONSTRAINT existing_order_return FOREIGN KEY ([OriginalOrder]) REFERENCES [dbo].[Orders] ([OrderNumber]) ON DELETE CASCADE
);

CREATE TABLE [dbo].[ReturnProducts] (
    [Id] INT IDENTITY(1,1) PRIMARY KEY,
    [OriginalOrder] VARCHAR(50),
    [ProductNumber] VARCHAR(50),
    [ReturnNumber] VARCHAR(50),
    CONSTRAINT existing_product_return FOREIGN KEY ([ProductNumber]) REFERENCES [dbo].[Product] ([ProductNumber]) ON DELETE CASCADE,
    CONSTRAINT existing_return FOREIGN KEY ([ReturnNumber]) REFERENCES [dbo].[Returns] ([ReturnNumber]) ON DELETE CASCADE

);

INSERT INTO [dbo].[Product] ([ProductNumber], [SellingPrice]) VALUES
    ('Rugged Liner F55U15', 150),
    ('Mobil 1 5W-30', 25),
    ('DrawTite 5504', 70),
    ('Sherman 036-87-1', 155);