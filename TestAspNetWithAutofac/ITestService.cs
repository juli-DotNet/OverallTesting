namespace WebApplication1;

public interface ITestService
{
     string Type { get; set; }
     string GetMessage();
}

public class TestService : ITestService
{
    public string Type { get; set; }

    public string GetMessage()
    {
        return "Something";
    }
}

public abstract class Test
{
    public virtual void DoSomething()
    {
        DoSoemthingElse();
    }

    public abstract void DoSoemthingElse();
}

public class B : Test
{
    public override void DoSoemthingElse()
    {
    }
}