using Xunit;

public class UnitTest1
{

    [Theory]
    [InlineData(3)]
    [InlineData(7)]
    [InlineData(4)]
    public void MyFirstTheory(int value)
    {
        Assert.True(IsOdd(value));
    }

    bool IsOdd(int value) 
    {
        return value % 2 == 1;
    }
}