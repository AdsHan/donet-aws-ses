namespace AmazonSimpleEmail.API.Utilis;

public abstract class BaseService
{
    protected BaseResult result;

    protected BaseService()
    {
        result = new BaseResult();
    }

    protected void AddError(string message)
    {
        result.Errors.Add(message);
    }
}
