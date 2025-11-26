namespace AdventureWorks.Exceptions
{
    public class NotFoundException(string message) : Exception(message)
    {
    }
}
