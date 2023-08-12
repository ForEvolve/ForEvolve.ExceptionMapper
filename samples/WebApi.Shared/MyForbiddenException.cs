using System;

namespace WebApi.Shared;

public class MyForbiddenException : Exception
{
    public MyForbiddenException()
        : base("Accessing this resource is forbidden.")
    {
        
    }
}
