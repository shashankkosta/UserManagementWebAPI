namespace UserManagement.Data
{
    public interface ITokenManager
    {
        string GenerateToken(int id);

        int ValidateToken(string token);
    }
}