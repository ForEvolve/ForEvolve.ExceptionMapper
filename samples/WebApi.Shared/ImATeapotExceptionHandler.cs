using ForEvolve.ExceptionMapper;
using Microsoft.AspNetCore.Http;
using System;
using System.Threading.Tasks;

namespace WebApi.Shared;

public class ImATeapotExceptionHandler : IExceptionHandler
{
    public int Order => HandlerOrder.DefaultOrder;

    public async Task ExecuteAsync(ExceptionHandlingContext context)
    {
        var response = context.HttpContext.Response;
        response.StatusCode = StatusCodes.Status418ImATeapot;
        response.ContentType = "text/html";
        context.Result = new ExceptionHandledResult(context.Error);
        await response.WriteAsync("<html><body><pre style=\"font-family: SFMono-Regular,Menlo,Monaco,Consolas,'Liberation Mono','Courier New',monospace;\">");
        await response.WriteAsync(@"             ;,'
     _o_    ;:;'
 ,-.'---`.__ ;
((j`=====',-'
 `-\     /
    `-=-'     hjw
Source: <a href=""https://www.asciiart.eu/food-and-drinks/coffee-and-tea"" target=""_blank"">https://www.asciiart.eu/food-and-drinks/coffee-and-tea</a>");
        await response.WriteAsync("</pre></body></html>");
    }

    public Task<bool> KnowHowToHandleAsync(Exception exception)
    {
        return Task.FromResult(exception is ImATeapotException);
    }
}
