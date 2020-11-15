using System;
using System.Threading.Tasks;
using System.Collections.Generic;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Primitives;

using Orleans;
using Orleans.Runtime;
using Orleans.Configuration;
using System.Collections;
using Microsoft.AspNetCore.Mvc;

using UserService.Data;
using UserService.Data.Items;
using UserService.Services;

using GrainInterface;
using UserService.Filters;
using Microsoft.AspNetCore.Http;

namespace UserService.Controllers
{
    [Route("api")]
    [ApiController]
    public class UserController : Microsoft.AspNetCore.Mvc.ControllerBase
    {
        public UserContext _context { get; }

        public UserController(UserContext context)
        {
            _context = context;
        }

        [HttpPost]
        [Route("register")]
        [ServiceFilter(typeof(RateLimitFilter))]
        public async Task<ActionResult<string>> PostRegister(RegisterVo item)
        {
            // check params
            string ret = item.IsValid();
            if (!string.IsNullOrEmpty(ret))
            {
                return ret;
            }

            // check user exists
            User user = await _context.Users.FindAsync(item.username.ToLower());
            if (user != null)
            {
                return "user exists";
            }

            // save
            user = new User(item);
            _context.Users.Add(user);
            await _context.SaveChangesAsync();

            return "success";
        }

        [HttpPost]
        [Route("login")]
        [ServiceFilter(typeof(RateLimitFilter))]
        public async Task<ActionResult<string>> PostLogin(LoginVo item)
        {
            // check params
            string ret = item.IsValid();
            if (!string.IsNullOrEmpty(ret))
            {
                return ret;
            }

            // check has such user
            User user = await _context.Users.FindAsync(item.username.ToLower());
            if (user == null)
            {
                return "user not exists";
            }

            // check has logined
            OrleanService orlean = await OrleanService.GetInstance();
            IValue grain = orlean.GetValueGrain(item.username.ToLower());
            string jwt = await grain.GetAsync();
            if (!string.IsNullOrEmpty(jwt))
            {
                return jwt;
            }


            // check password
            if (user.Password != CryptoService.SHA1(item.password))
            {
                return "invalid password";
            }

            jwt = System.Guid.NewGuid().ToString();
            // cache session
            await grain.SetAsync(jwt);

            IValue grainR = orlean.GetValueGrain(jwt);
            await grainR.SetAsync(item.username.ToLower());

            return jwt;
        }

        [HttpGet]
        [Route("info")]
        [ServiceFilter(typeof(RateLimitFilter))]
        public async Task<ActionResult<UserInfoVo>> GetInfo()
        {
            // get jwt
            StringValues input;
            Request.Headers.TryGetValue("Authorization", out input);
            string auth = input.ToString();
            string jwt = auth.Substring("Bearer ".Length);
            if (string.IsNullOrEmpty(jwt))
            {
                return new UnauthorizedResult();
            }

            // get username from orleans
            OrleanService orlean = await OrleanService.GetInstance();
            IValue grain = orlean.GetValueGrain(jwt);
            string username = await grain.GetAsync();
            if (string.IsNullOrEmpty(username))
            {
                return new UnauthorizedResult();
            }

            User user = await _context.Users.FindAsync(username);
            UserInfoVo ret = new UserInfoVo(user);

            return new JsonResult(ret);
        }
    }
}
