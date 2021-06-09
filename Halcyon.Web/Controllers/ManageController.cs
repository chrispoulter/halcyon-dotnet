﻿using Halcyon.Web.Data;
using Halcyon.Web.Models;
using Halcyon.Web.Models.Manage;
using Halcyon.Web.Models.User;
using Halcyon.Web.Services.Hash;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Net;
using System.Threading.Tasks;

namespace Halcyon.Web.Controllers
{
    [ApiController]
    [Produces("application/json")]
    [ProducesResponseType(typeof(ApiResponse), (int)HttpStatusCode.Unauthorized)]
    [Route("api/[controller]")]
    [Authorize]
    public class ManageController : BaseController
    {
        private readonly HalcyonDbContext _context;

        private readonly IHashService _hashService;

        public ManageController(HalcyonDbContext context, IHashService hashService)
        {
            _context = context;
            _hashService = hashService;
        }

        [HttpGet]
        [ProducesResponseType(typeof(ApiResponse<GetProfileResponse>), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(ApiResponse), (int)HttpStatusCode.NotFound)]
        public async Task<IActionResult> GetProfile()
        {
            var user = await _context.Users
                .FirstOrDefaultAsync(u => u.Id == CurrentUserId);

            if (user == null || user.IsLockedOut)
            {
                return Generate(
                    HttpStatusCode.NotFound,
                    InternalStatusCode.USER_NOT_FOUND,
                    "User not found.");
            }

            var result = new GetProfileResponse
            {
                Id = user.Id,
                EmailAddress = user.EmailAddress,
                FirstName = user.FirstName,
                LastName = user.LastName,
                DateOfBirth = user.DateOfBirth.ToUniversalTime()
            };

            return Generate(HttpStatusCode.OK, result);
        }

        [HttpPut]
        [ProducesResponseType(typeof(ApiResponse<UserActionResponse>), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(ApiResponse), (int)HttpStatusCode.NotFound)]
        [ProducesResponseType(typeof(ApiResponse), (int)HttpStatusCode.BadRequest)]
        public async Task<IActionResult> UpdateProfile(UpdateProfileModel model)
        {
            var user = await _context.Users
                 .FirstOrDefaultAsync(u => u.Id == CurrentUserId);

            if (user == null || user.IsLockedOut)
            {
                return Generate(
                    HttpStatusCode.NotFound,
                    InternalStatusCode.USER_NOT_FOUND,
                    "User not found.");
            }


            if (!model.EmailAddress.Equals(user.EmailAddress, StringComparison.InvariantCultureIgnoreCase))
            {
                var existing = await _context.Users
                    .FirstOrDefaultAsync(u => u.EmailAddress == model.EmailAddress);

                if (existing != null)
                {
                    return Generate(
                        HttpStatusCode.BadRequest,
                        InternalStatusCode.DUPLICATE_USER,
                        $"User name \"{model.EmailAddress}\" is already taken.");
                }
            }

            user.EmailAddress = model.EmailAddress;
            user.FirstName = model.FirstName;
            user.LastName = model.LastName;
            user.DateOfBirth = model.DateOfBirth.Value.ToUniversalTime();

            await _context.SaveChangesAsync();

            var result = new UserActionResponse
            {
                Id = user.Id
            };

            return Generate(
                HttpStatusCode.OK,
                InternalStatusCode.PROFILE_UPDATED,
                result,
                "Your profile has been updated.");
        }

        [HttpPut("changepassword")]
        [ProducesResponseType(typeof(ApiResponse<UserActionResponse>), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(ApiResponse), (int)HttpStatusCode.NotFound)]
        [ProducesResponseType(typeof(ApiResponse), (int)HttpStatusCode.BadRequest)]
        public async Task<IActionResult> ChangePassword(ChangePasswordModel model)
        {
            var user = await _context.Users
                .FirstOrDefaultAsync(u => u.Id == CurrentUserId);

            if (user == null || user.IsLockedOut)
            {
                return Generate(
                   HttpStatusCode.NotFound,
                   InternalStatusCode.USER_NOT_FOUND,
                   "User not found.");
            }

            var verified = _hashService.VerifyHash(model.CurrentPassword, user.Password);
            if (!verified)
            {
                return Generate(
                    HttpStatusCode.BadRequest,
                    InternalStatusCode.INCORRECT_PASSWORD,
                    "Incorrect password.");
            }

            user.Password = _hashService.GenerateHash(model.NewPassword);
            user.PasswordResetToken = null;

            await _context.SaveChangesAsync();

            var result = new UserActionResponse
            {
                Id = user.Id
            };

            return Generate(
                HttpStatusCode.OK,
                InternalStatusCode.PASSWORD_CHANGED,
                result,
                "Your password has been changed.");
        }

        [HttpDelete]
        [ProducesResponseType(typeof(ApiResponse<UserActionResponse>), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(ApiResponse), (int)HttpStatusCode.NotFound)]
        public async Task<IActionResult> DeleteProfile()
        {
            var user = await _context.Users
                .FirstOrDefaultAsync(u => u.Id == CurrentUserId);

            if (user == null || user.IsLockedOut)
            {
                return Generate(
                   HttpStatusCode.NotFound,
                   InternalStatusCode.USER_NOT_FOUND,
                   "User not found.");
            }

            _context.Users.Remove(user);

            await _context.SaveChangesAsync();

            var result = new UserActionResponse
            {
                Id = user.Id
            };

            return Generate(
                HttpStatusCode.OK,
                InternalStatusCode.ACCOUNT_DELETED,
                result,
                "Your account has been deleted.");
        }
    }
}
