using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using UserManagement.Models;
using UserManagement.Data;
using AutoMapper;
using Microsoft.AspNetCore.JsonPatch;
using UserManagement.Helpers;
using Microsoft.AspNetCore.Authorization;
using System;
using System.Threading;
using System.Security.Claims;
using System.Linq;

namespace UserManagement.Controllers
{
    [Authorize]
    [ApiController]
    // [NonController]
    [Route("api/users")]
    public class UsersController : ControllerBase
    {
        private readonly IUserManagement _repository;
        private readonly IMapper _mapper;
        private readonly IUserAuthentication _userAuth;
        private readonly ITokenManager _tokenManager;

        public UsersController(
            IUserManagement repository,
            IMapper mapper,
            IUserAuthentication userAuth,
            ITokenManager tokenManager)
        {
            _repository = repository;
            _mapper = mapper;
            _userAuth = userAuth;
            _tokenManager = tokenManager;
            // System.Console.WriteLine("Controller " + userAuth.GetHashCode());
        }

        // GET api/users
        [HttpGet]
        [Authorize(AuthenticationSchemes = "BasicAuth")]
        public List<UserGet> GetAllUsers()
        {
            var users = _repository.GetAllUsers();

            // return _repository.GetAllUsers();
            return _mapper.Map<List<UserGet>>(users);
        }

        // GET api/users/{id}
        // [Authorize]
        [HttpGet("{id}", Name = "GetUserById")]
        public ActionResult<UserGet> GetUserById(int id)
        {
            var user = _repository.GetUserById(id);
            if (user == null)
            {
                return NotFound();
            }
            // var userGet = new UserGet() {
            //     n_UserID = user.n_UserID, 
            //     s_UserCode = user.s_UserCode, 
            //     s_UserName = user.s_UserName,
            //     s_Email = user.s_Email
            // };

            var userGet = _mapper.Map<UserGet>(user);

            return Ok(userGet);
        }

        // POST api/users
        // RETURNS 201 Created
        // [BasicAuthentication]
        [HttpPost]
        public ActionResult<User> CreateUser([FromBody] UserPost userPost)
        {
            // System.Console.WriteLine("Hello");
            // if (!ModelState.IsValid)
            // {
            //     return BadRequest();
            // }

            // if (user == null)
            //     return BadRequest();

            var userModel = _mapper.Map<User>(userPost);
            userModel.n_UserType = 1;
            userModel.d_CreatedDateTime = System.DateTime.Now;
            userModel.s_Password = Helper.CalculateSha(userPost.s_Password);

            _repository.CreateUser(userModel);

            if (_repository.SaveChanges())
            {
                // return Ok("Sample web request");
                return CreatedAtRoute("GetUserById", new { id = userModel.n_UserID }, _mapper.Map<UserGet>(userModel));
            }

            return BadRequest();
        }

        // DELETE   api/users/{id}
        // RETURNS  404 Not Found | 204 No Content
        [HttpDelete("{id}")]
        public IActionResult DeleteUser(int id)
        {
            var userModel = _repository.GetUserById(id);
            if (userModel == null)
            {
                return NotFound();
            }

            _repository.DeleteUser(userModel);
            if (_repository.SaveChanges())
            {
                return NoContent();
            }
            return BadRequest();
        }

        // PUT api/users
        // RETURNS 404 Not Found | 204 No Content
        [HttpPut("{id}")]
        public IActionResult UpdateUser(int id, UserPost userPost)
        {
            var userModel = _repository.GetUserById(id);
            if (userModel == null)
            {
                return NotFound();
            }

            // Source -> Target
            _mapper.Map(userPost, userModel);

            // userModel.s_UserCode = user.s_UserCode;
            // userModel.s_UserName = user.s_UserName;
            // userModel.s_Email = user.s_Email;

            // ~~ Not Required for _mapper.Map() and PatchDoc.ApplyTo
            // _repository.UpdateUser(userModel);
            if (_repository.SaveChanges())
            {
                return NoContent();
            }
            return BadRequest();
        }

        // PATCH api/users
        // RETURNS 404 Not Found | 204 No Content
        [HttpPatch("{id}")]
        public IActionResult PartialUpdateUser(int id, JsonPatchDocument<User> userPostDocument)
        {
            var userModel = _repository.GetUserById(id);
            if (userModel == null)
            {
                return NotFound();
            }

            // Creating UserPost object from UserModel
            // var userPostObject = _mapper.Map<UserPost>(userModel);

            // Applying Patch document operations to UserPost object
            userPostDocument.ApplyTo(userModel, ModelState);

            // Retrieving User object from UserPost object
            // var user = _mapper.Map<User>(userPostObject);
            // _mapper.Map(userPostObject, userModel);

            // ~~ Not Required for _mapper.Map() and PatchDoc.ApplyTo
            // _repository.UpdateUser(userModel);
            if (_repository.SaveChanges())
            {
                return NoContent();
            }
            return BadRequest();
        }

        // GET api/users/authenticate
        // RETURNS 404 Not Found | 200 OK
        // [HttpPost]
        // [Route("authenticate")]
        // [AllowAnonymous]
        // public ActionResult<UserGet> AuthenticateUser(UserLogin userLogin)
        // {
        //     var user = _userAuth.AuthenticateUser(userLogin);
        //     if (user == null)
        //         return NotFound("Invalid User Code or Password");

        //     user.s_Token = Guid.NewGuid().ToString();

        //     _repository.UpdateUser(user);
        //     if (_repository.SaveChanges())
        //     {
        //         return Ok(_mapper.Map<UserGet>(user));
        //     }

        //     return BadRequest();
        // }

        // GET api/users/getaccesstoken
        // RETURNS 200 OK | 401 Unauthorized
        [HttpGet]
        [Route("getaccesstoken")]
        [Authorize(AuthenticationSchemes = "BasicAuth")]
        public ActionResult<string> GetAccessToken()
        {
            if (!Request.HttpContext.User.HasClaim(x => x.Type == ClaimTypes.NameIdentifier))
                return Unauthorized();

            var claimsId = Request.HttpContext.User.Claims.FirstOrDefault(
                x => x.Type == ClaimTypes.NameIdentifier).Value;

            int userId = 0;
            int.TryParse(claimsId, out userId);

            if (userId == 0)
                return Unauthorized();

            string token = _tokenManager.GenerateToken(userId);

            if (token != string.Empty)
                return Ok(token);

            return Unauthorized();
        }
    }
}