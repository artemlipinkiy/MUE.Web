using Microsoft.Owin.Security;
using MUE.Web.Entities;
using MUE.Web.EntitiesDTO.UserDTO;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Threading.Tasks;
using System.Web;

namespace MUE.Web.Services
{
    public class AccountService
    {
        private readonly OwnerService ownerService = new OwnerService();
        public async Task Logout(IAuthenticationManager AuthenticationManager) 
        {
            AuthenticationManager.SignOut();
        }
        public async Task<bool> CreateOwner(CreateOwnerDTO dto)
        {
            if (await LoginExist(dto.Login))
            {
                return false;
            }
            Guid id = Guid.NewGuid();
            Role ownerrole = await GetRole("Owner");
            var ownerId = await ownerService.Create(new OwnerDTO
            {
                FirstName = dto.FirstName,
                LastName = dto.LastName,
                MiddlleName = dto.MiddlleName,
                Status = dto.Status,
            });
            using (MUEContext db = new MUEContext())
            {
                User user = new User
                {
                    Login = dto.Login,
                    UserId = id,
                    RoleId = ownerrole.RoleId,
                    HashPassword = HashPassword(dto.Password),
                    OwnerId = ownerId
                };
                db.Users.Add(user);
                await db.SaveChangesAsync();
            }
            return true;
        }
        public async Task<bool> CreateAdm(CreateUserDTO dto)
        {
            if (await LoginExist(dto.Login))
            {
                return false;
            }
            Guid id = Guid.NewGuid();
            Role sysadmrole = await GetRole("SysAdmin");
            using (MUEContext db = new MUEContext())
            {
                User user = new User
                {
                    Login = dto.Login,
                    UserId = id,
                    RoleId = sysadmrole.RoleId,
                    HashPassword = HashPassword(dto.Password),
                };
                db.Users.Add(user);
                await db.SaveChangesAsync();
            }
            return true;
        }
        public async Task<ProfileDTO> MyProfile(string Login) 
        {
            using (MUEContext db = new MUEContext())
            {
                var user = await db.Users.Where(u => u.Login == Login).FirstOrDefaultAsync();
                var roles = await GetAllRoles();
                OwnerDTO owner = null;
                if (user.OwnerId.HasValue)
                {
                    owner = await ownerService.GetDTO(user.OwnerId.Value);
                }
                return new ProfileDTO
                {
                    FirstName = owner != null ? owner.FirstName : "",
                    LastName = owner != null ? owner.LastName : "",
                    MiddlleName = owner != null ? owner.MiddlleName : "",
                    Role = roles.Where(r => r.RoleId == user.RoleId).Select(r => r.Name).FirstOrDefault(),
                    Login = user.Login,
                    Status = owner != null ? owner.Status : ""

                };
            }
        }
        public async Task<IEnumerable<UserDTO>> GetAll() 
        {
            var roles = await GetAllRoles();
            using (MUEContext db = new MUEContext())
            {
                var users = await db.Users.ToListAsync();
                return users.Select(u => new UserDTO { 
                Login = u.Login,
                Role = roles.Where(r => r.RoleId == u.RoleId).Select(r => r.Name).FirstOrDefault(),
                RoleId = u.RoleId,
                UserId = u.UserId
                }).ToList();
            }
        }
        public async Task<IEnumerable<RoleDTO>> GetAllRoles() 
        {
            using (MUEContext db = new MUEContext())
            {
                return await db.Roles.Select(r => new RoleDTO { 
                RoleId = r.RoleId,
                Name = r.Name,
                Description = r.Description
                }).ToListAsync();
            }
        }
        public async Task<Guid> GetUserId(string login) 
        {
            using (MUEContext db = new MUEContext())
            {
                return await db.Users.Where(u => u.Login == login).Select(u => u.UserId).FirstOrDefaultAsync();
            }
        }
        public async Task<bool> Register(CreateUserDTO dto)
        {
            if (await LoginExist(dto.Login))
            {
                return false;
            }
            Guid id = Guid.NewGuid();
            Role defaultrole = await GetRole("Default");
            using (MUEContext db = new MUEContext())
            {
                User user = new User {
                    Login = dto.Login, 
                    UserId = id, 
                    RoleId = defaultrole.RoleId,
                    HashPassword = HashPassword(dto.Password)
                };
                db.Users.Add(user);
                await db.SaveChangesAsync();
            }
            return true;
        }
        public async Task<bool> Login(LoginUserDTO dto, IAuthenticationManager AuthenticationManager) 
        {
            using (MUEContext db = new MUEContext())
            {
                User user = await db.Users.Include(r => r.Role).Where(u => u.Login == dto.Login).FirstOrDefaultAsync();
                if (user == null)
                {
                    return false;
                }
                else
                {
                    if (await VerifyHashedPassword(user.HashPassword,dto.Password))
                    {
                        ClaimsIdentity claim = new ClaimsIdentity("ApplicationCookie", ClaimsIdentity.DefaultNameClaimType, ClaimsIdentity.DefaultRoleClaimType);
                        claim.AddClaim(new Claim(ClaimTypes.NameIdentifier, user.UserId.ToString(), ClaimValueTypes.String));
                        claim.AddClaim(new Claim(ClaimsIdentity.DefaultNameClaimType, user.Login, ClaimValueTypes.String));
                        claim.AddClaim(new Claim("http://schemas.microsoft.com/accesscontrolservice/2010/07/claims/identityprovider",
                            "OWIN Provider", ClaimValueTypes.String));
                        if (user.Role != null)
                            claim.AddClaim(new Claim(ClaimsIdentity.DefaultRoleClaimType, user.Role.Name, ClaimValueTypes.String));

                        AuthenticationManager.SignOut();
                        AuthenticationManager.SignIn(new AuthenticationProperties
                        {
                            IsPersistent = true
                        }, claim);
                        return true; 
                    }
                    else
                    {
                        return false;
                    }
                }
            }
        }
        private async Task<bool> LoginExist(string login)
        {
            using (MUEContext db = new MUEContext())
            {
                if (await db.Users.Where(u => u.Login == login).CountAsync() > 0)
                    return true;
                else
                    return false;

            }
        }
        private async Task<Role> GetRole(string Name)
        {
            using (MUEContext db = new MUEContext())
            {
               return await db.Roles.Where(r => r.Name == Name).FirstOrDefaultAsync();
            }
        }
        #region Password

        private static string HashPassword(string password)
        {
            byte[] salt;
            byte[] buffer2;
            if (password == null)
            {
                throw new ArgumentNullException("password");
            }
            using (Rfc2898DeriveBytes bytes = new Rfc2898DeriveBytes(password, 0x10, 0x3e8))
            {
                salt = bytes.Salt;
                buffer2 = bytes.GetBytes(0x20);
            }
            byte[] dst = new byte[0x31];
            Buffer.BlockCopy(salt, 0, dst, 1, 0x10);
            Buffer.BlockCopy(buffer2, 0, dst, 0x11, 0x20);
            return Convert.ToBase64String(dst);
        }
        public async Task<bool> VerifyHashedPassword(string hashedPassword, string password)
        {
            byte[] buffer4;
            if (hashedPassword == null)
            {
                return false;
            }
            if (password == null)
            {
                throw new ArgumentNullException("password");
            }
            byte[] src = Convert.FromBase64String(hashedPassword);
            if ((src.Length != 0x31) || (src[0] != 0))
            {
                return false;
            }
            byte[] dst = new byte[0x10];
            Buffer.BlockCopy(src, 1, dst, 0, 0x10);
            byte[] buffer3 = new byte[0x20];
            Buffer.BlockCopy(src, 0x11, buffer3, 0, 0x20);
            using (Rfc2898DeriveBytes bytes = new Rfc2898DeriveBytes(password, dst, 0x3e8))
            {
                buffer4 = bytes.GetBytes(0x20);
            }
            return ByteArraysEqual(buffer3, buffer4);
        }
        private static bool ByteArraysEqual(byte[] b1, byte[] b2)
        {
            if (b1 == b2) return true;
            if (b1 == null || b2 == null) return false;
            if (b1.Length != b2.Length) return false;
            for (int i = 0; i < b1.Length; i++)
            {
                if (b1[i] != b2[i]) return false;
            }
            return true;
        }

        #endregion

        public async Task<Guid> GetOwnerId(Guid id) 
        {
            using (MUEContext db = new MUEContext())
            {
                return await db.Users.Where(u => u.UserId == id).Select(u => u.OwnerId.HasValue? u.OwnerId.Value : Guid.Empty).FirstOrDefaultAsync();
            }
        }
    }
}