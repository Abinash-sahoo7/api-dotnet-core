CREATE PROCEDURE usp_CreateUser
	@FullName NVARCHAR(100),
	@Email NVARCHAR(256),
	@PasswordHash NVARCHAR(MAX)
AS
BEGIN
	SET NOCOUNT ON;
	BEGIN TRY
		INSERT INTO dbo.Users (FullName, Email, PasswordHash)
		VALUES (@FullName, @Email, @PasswordHash);


		SELECT CAST(1 AS BIT) AS Success;
	END TRY
		BEGIN CATCH
		-- You may want to handle unique key violation etc.
		SELECT CAST(0 AS BIT) AS Success;
	END CATCH
END