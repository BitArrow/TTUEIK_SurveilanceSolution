using AutoMapper;
using Core.DTO;
using Core.DTO.Constants;
using Core.Models;
using Core.Services.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace Core.Services
{
    public class UserService : IUserService
    {
        private HttpContext HttpContext { get; }
        private ClaimsPrincipal User { get; }
        private SurveillanceContext _db { get; }
        private readonly IMapper _mapper;

        public UserService(IHttpContextAccessor httpContextAccessor, SurveillanceContext db, IMapper mapper)
        {
            HttpContext = httpContextAccessor.HttpContext;
            _db = db;
            _mapper = mapper;

            if (HttpContext != null)
            {
                User = HttpContext.User;
            }
        }

        public string UserB2CId { get { return User.FindFirst(Claims.UserId)?.Value; } }
        public string UserDisplayName { get { return User.FindFirst(Claims.Name)?.Value; } }

        public async Task<ServiceResult> ValidateUser()
        {
            if (string.IsNullOrEmpty(UserB2CId))
                return ServiceResult.Error(ErrorsEnum.UserB2CMissing);

            var user = await _db.Users.Where(u => u.B2CId.Equals(UserB2CId)).FirstOrDefaultAsync();

            if (user != null)
                return ServiceResult.Ok(_mapper.Map<UserDto>(user));

            var newUser = await _db.Users.AddAsync(new User()
            {
                B2CId = UserB2CId,
                Name = UserDisplayName
            });

            await _db.SaveChangesAsync();

            return ServiceResult.Ok(_mapper.Map<UserDto>(newUser.Entity));
        } 

        public async Task<User> GetUser()
        {
            return string.IsNullOrEmpty(UserB2CId) ? null :
                await _db.Users.Where(u => !string.IsNullOrEmpty(u.B2CId) && u.B2CId.Equals(UserB2CId)).SingleOrDefaultAsync();
        }

        public async Task<User> GetUser(long id)
        {
            return string.IsNullOrEmpty(UserB2CId)
               ? null
               : await _db.Users
                        .Where(u => u.Id == id)
                        .SingleOrDefaultAsync();
        }

        public async Task<List<Group>> GetUserGroups(long userId)
        {
            return string.IsNullOrEmpty(UserB2CId) ? null :
                await _db.Groups
                .Include(g => g.GroupUsers)
                .Where(g => g.GroupUsers.Any(gu => gu.UserId == userId) || g.OwnerId == userId)
                .ToListAsync();
        }
    }
}
