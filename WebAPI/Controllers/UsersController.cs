﻿using Business.Abstract;
using Business.Constants;
using Core.Entities.Concrete;
using Entities.Concrete;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        IUserService _userService;

        public UsersController(IUserService userService)
        {
            _userService = userService;
        }

        [HttpPost("add")]
        public IActionResult Add(User user)
        {
            var result = _userService.Add(user);
            if (result.Success)
            {
                return Ok(result);
            }
            return BadRequest(result);
        }

        [HttpPut]
        public IActionResult Update(User user)
        {
            var result = _userService.Update(user);
            if (result.Success)
            {
                return Ok(result);
            }
            return BadRequest(result);
        }

        [HttpDelete]
        public IActionResult Delete(User user)
        {
            var result = _userService.Delete(user);
            if (result.Success)
            {
                return Ok(result);
            }
            return BadRequest(result);
        }

        [HttpGet("getall")]
        public IActionResult GetAll()
        {
            var result = _userService.GetAll();
            if (result.Success)
            {
                return Ok(result);
            }
            return BadRequest(result);
        }

        [HttpGet("getclaims")]
        public IActionResult GetClaims(User user)
        {
            var result = _userService.GetClaims(user);
            if (result!=null)
            {
                return Ok(result.ToList());
            }
            return BadRequest(Messages.ClaimsNotFound);
        }

        [HttpGet("getbymail")]
        public IActionResult GetByMail(string email)
        {
            var result = _userService.GetByMail(email);
            if (result!=null)
            {
                return Ok(result);
            }
            return BadRequest(Messages.UserNotFoundForEmail);
        }
        [HttpGet("getuserdetails")]
        public IActionResult GetUserDetails(int id)
        {
            var result = _userService.GetUserDetails(id);
            if (result.Success)
            {
                return Ok(result);
            }
            return BadRequest(result);
        }
       
    }
}
