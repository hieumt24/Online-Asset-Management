namespace AssetManagement.Infrastructure.Exceptions
{
    public class NotFoundRepositoryException : Exception
    {
        public NotFoundRepositoryException(string message) : base(message)
        {
        }
    }
}