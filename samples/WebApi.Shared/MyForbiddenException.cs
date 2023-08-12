using System;
using System.Text.Json.Serialization;

namespace WebApi.Shared;

public class MyForbiddenException : Exception
{
    public MyForbiddenException()
        : base("Accessing this resource is forbidden.")
    {
        
    }

    public string CustomProperty1 => "Lorem Ipsum 1";

    [JsonIgnore]
    public string CustomProperty2 => "Lorem Ipsum 2";
}
