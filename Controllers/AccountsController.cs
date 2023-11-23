using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System.Data;
using VpniFy.Backend.Common.Exceptions;
using VpniFy.Backend.Contracts;
using VpniFy.Backend.Model;
using VpniFy.Backend.Services;

namespace VpniFy.Backend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountsController : ControllerBase
    {
        private readonly UserManager<User> userManager;
        private readonly RoleManager<Role> roleManager;
        private readonly SignInManager<User> signInManager;
        private readonly IJwtService jwtService;
        private readonly IUserRepository userRepository;

        public AccountsController(UserManager<User> userManager, RoleManager<Role> roleManager, SignInManager<User> signInManager, IUserRepository userRepository, IJwtService jwtService)
        {
            this.userManager=userManager;
            this.roleManager=roleManager;
            this.signInManager=signInManager;
            this.userRepository=userRepository;
            this.jwtService=jwtService;
        }


        [HttpGet("[Action]")]
        [Authorize(Roles = "Admin")]
        public virtual async Task<ActionResult<List<User>>> Get(CancellationToken cancellationToken)
        {
            //var userName = HttpContext.User.Identity.GetUserName();
            //userName = HttpContext.User.Identity.Name;
            //var userId = HttpContext.User.Identity.GetUserId();
            //var userIdInt = HttpContext.User.Identity.GetUserId<int>();
            //var phone = HttpContext.User.Identity.FindFirstValue(ClaimTypes.MobilePhone);
            //var role = HttpContext.User.Identity.FindFirstValue(ClaimTypes.Role);

            
            var users = await userRepository.TableNoTracking.ToListAsync(cancellationToken);
            return Ok(users);
        }

        [HttpGet("{id:int}")]
        public virtual async Task<ActionResult<User>> Get(int id, CancellationToken cancellationToken)
        {
            var user2 = await userManager.FindByIdAsync(id.ToString());
            var role = await roleManager.FindByNameAsync("Admin");

            
            if (user2 == null)
                return NotFound();

            await userManager.UpdateSecurityStampAsync(user2);
            //await userRepository.UpdateSecurityStampAsync(user, cancellationToken);

            return user2;
        }

        /// <summary>
        /// This method generate JWT Token
        /// </summary>
        /// <param name="tokenRequest">The information of token request</param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        [HttpPost("[action]")]
        [AllowAnonymous]
        public virtual async Task<ActionResult> Token([FromForm] TokenRequest tokenRequest, CancellationToken cancellationToken)
        {


            //var user = await userRepository.GetByUserAndPass(username, password, cancellationToken);
            var user = await userManager.FindByNameAsync(tokenRequest.username);
            if (user == null)
                throw new BadRequestException("نام کاربری یا رمز عبور اشتباه است");

            var isPasswordValid = await userManager.CheckPasswordAsync(user, tokenRequest.password);
            if (!isPasswordValid)
                throw new BadRequestException("نام کاربری یا رمز عبور اشتباه است");


            //if (user == null)
            //    throw new BadRequestException("نام کاربری یا رمز عبور اشتباه است");

            var jwt = await jwtService.GenerateAsync(user);
            return new JsonResult(jwt);
        }

        /// <summary>
        /// ساخت SuperAdmin
        /// </summary>
        /// <param name="userDto"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>

        [HttpPost("[Action]")]
        [AllowAnonymous]
        public virtual async Task<ActionResult<User>> Create(UserDto userDto, CancellationToken cancellationToken)
        {
           

            var exists = await userRepository.TableNoTracking.AnyAsync(p => p.UserName == userDto.UserName);
            if (exists)
                return BadRequest("نام کاربری تکراری است");


            var user = new User
            {
                UserName = userDto.UserName,
                Email = userDto.Email,
       
            };
            var result = await userManager.CreateAsync(user, userDto.Password);

            var result2 = await roleManager.CreateAsync(new Role
            {
                Name = "SuperAdmin",
          
            });

            var result3 = await userManager.AddToRoleAsync(user, "SuperAdmin");

            //await userRepository.AddAsync(user, userDto.Password, cancellationToken);
            return user;
        }





  

        [HttpPut("[Action]")]
        public virtual async Task<ActionResult> Update(int id, User user, CancellationToken cancellationToken)
        {
            var updateUser = await userRepository.GetByIdAsync(cancellationToken, id);

            updateUser.UserName = user.UserName;
            updateUser.PasswordHash = user.PasswordHash;
       

            await userRepository.UpdateAsync(updateUser, cancellationToken);

            return Ok();
        }

        [HttpDelete("[Action]")]
        public virtual async Task<ActionResult> Delete(int id, CancellationToken cancellationToken)
        {
            var user = await userRepository.GetByIdAsync(cancellationToken, id);
            await userRepository.DeleteAsync(user, cancellationToken);

            return Ok();
        }

     

    }
}
