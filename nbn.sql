CREATE PROCEDURE UpdateCustomerDetails
    @CustomerID INT,
    @Name varchar(255),
    @Contact NVARCHAR(50)
AS
BEGIN
    UPDATE Customers
    SET Name = @Name,
        Contact = @Contact
    WHERE CustomerID = @CustomerID;
END;
