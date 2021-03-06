﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Encodings.Web;
using System.Threading.Tasks;
using Identity.API.Models;
using IdentityModel.Client;
using IdentityServer4.Services;
using JieDDDFramework.Module.Identity;
using JieDDDFramework.Module.Identity.Models;
using JieDDDFramework.Web;
using JieDDDFramework.Web.ModelValidate;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;

namespace Identity.API.Controllers
{
    [Route("api/[controller]/[action]")]
    public class AccountController : BaseController
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly IIdentityServerInteractionService _interaction;
        private readonly JwtSettings _jwtSettings;

        public AccountController(UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager, IIdentityServerInteractionService interaction, IOptions<JwtSettings> jwtSettings)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _interaction = interaction;
            _jwtSettings = jwtSettings.Value;
        }

        [HttpGet]
        public async Task<IActionResult> Login(string returnUrl)
        {
            var context = await _interaction.GetAuthorizationContextAsync(returnUrl);
            if (context?.IdP != null)
            {
                return Redirect(UrlEncoder.Default.Encode(returnUrl));
            }

            return await Login(new LoginViewModel()
            {
                ReturnUrl = returnUrl,
                Password = "123456",
                Email = "demouser@xx.com",
                RememberMe = true
            });
        }

        /// <summary>
        /// 登录
        /// </summary>
        /// <remarks>
        ///     {
        ///         "email":"demouser@xx.com",
        ///         "password":"123456"
        ///     }
        /// </remarks>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<IActionResult> Login([FromBody, Validator] LoginViewModel model)
        {
            var user = await _userManager.FindByEmailAsync(model.Email);
            if (user != null && await _userManager.CheckPasswordAsync(user, model.Password))
            {
                await _signInManager.SignInAsync(user, model.RememberMe);
                if (_interaction.IsValidReturnUrl(model.ReturnUrl))
                {
                    return Redirect(model.ReturnUrl);
                }
                return Redirect("~/");
            }
            return Redirect("~/");
        }
    }
}
