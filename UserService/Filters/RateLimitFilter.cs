using System;
using System.Net;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using GrainInterface;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using UserService.Data;
using UserService.Services;

namespace UserService.Filters
{
    public class RateLimitFilter : IAsyncActionFilter
    {
        public readonly BlackListContext context;

        public RateLimitFilter(BlackListContext context)
        {
            this.context = context;
        }

        public async Task OnActionExecutionAsync(ActionExecutingContext httpcontext, ActionExecutionDelegate next)
        {
            IPAddress ipAddress = httpcontext.HttpContext.Request.HttpContext.Connection.RemoteIpAddress;
            string ip = ipAddress.ToString();
            Console.WriteLine("\n\n\n\n--->from ip " + ipAddress);

            OrleanService orlean = await OrleanService.GetInstance();

            IRateLimit grain = orlean.GetRateLimit(ip);
            bool isOk = await grain.CheckRateLimit();

            if (!isOk)
            {
                BlackListItem item = await context.BlackLists.FindAsync(ip);
                if (item == null)
                {
                    item = new BlackListItem(ip);
                    context.BlackLists.Add(item);
                }
                else
                {
                    long unixTime = (long)DateTime.UtcNow.Subtract(new DateTime(1970, 1, 1)).TotalSeconds;
                    item.LastTime = unixTime;
                    context.BlackLists.Update(item);
                }
                await context.SaveChangesAsync();

                httpcontext.HttpContext.Response.StatusCode = 429;
                httpcontext.Result = new EmptyResult();
            }
            else
            {
                await next();
            }
        }
    }
}
