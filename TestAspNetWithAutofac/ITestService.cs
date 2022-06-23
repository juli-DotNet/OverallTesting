namespace WebApplication1;

public interface ITestService
{
    string GetMessage();
}

public class TestService : ITestService
{
    public string GetMessage()
    {
        return "Something";
    }
}