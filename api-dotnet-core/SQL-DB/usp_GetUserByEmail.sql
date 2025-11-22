CREATE PROCEDURE usp_GetUserByEmail
	@Email NVARCHAR(256)
AS
BEGIN
	SET NOCOUNT ON;
	SELECT Id, FullName, Email, PasswordHash
	FROM dbo.Users
	WHERE Email = @Email;
END