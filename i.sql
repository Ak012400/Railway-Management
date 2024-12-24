CREATE PROCEDURE UpdatePassword 
    @CustomerID INT,
    @Password NVARCHAR(255),
    @StatusMessage NVARCHAR(255) OUTPUT
AS
BEGIN
    -- Password update karne ke liye query
    UPDATE Customers
    SET Password = @Password
    WHERE CustomerID = @CustomerID;
    
    -- Agar update successful ho, to ek message set karein
    IF @@ROWCOUNT = 0
    BEGIN
        SET @StatusMessage = 'No user found with the given UserID';
    END
    ELSE
    BEGIN
        SET @StatusMessage = 'Password updated successfully';
    END
END
