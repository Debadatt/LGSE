using LGSE_APIService.Common;
using LGSE_APIService.Common.Utilities;
using LGSE_APIService.DataObjects;
using LGSE_APIService.Models;
using LGSE_APIService.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;

namespace LGSE_APIService.Authorization
{
    /// <summary>
    /// Authorization will be handled by this entity
    /// </summary>
    public class AuthorizationManager
    {
        //private static AuthorizationManager _instance;
        //private static object syncLock = new object();
        //public static AuthorizationManager GetIntance()
        //{
        //    if (_instance == null)
        //    {
        //        lock (syncLock)
        //        {
        //            if (_instance == null)
        //            {
        //                _instance = new AuthorizationManager();
        //            }
        //        }
        //    }
        //    return _instance;
        //}
        public LGSE_APIContext dbContext { get; set; }
        public AuthorizationManager(LGSE_APIContext inputdbContext)
        {
            dbContext = inputdbContext;
            //dbContext = inputdbContext;
            //DbUtilities.dbContext = inputdbContext;
        }

        public bool IsUserExists(string userName)
        {
            return  dbContext.Users.Any(item => item.Email == userName && !item.Deleted && item.IsActiveUser && item.Domain.IsActive==true);
           
        }
        //List<Features>
        public bool AuthorizeUser(string userName, string userRole, string token, Features[] dataEntity, OperationType ops)// Abhijeet - 30-10-2018 -Added role id
        {
            try
            {
                LGSELogger.Information("AuthorizeUser method has veen called");
            

                
                //if (isUserExist)
                //{
                    if (CheckPermission(userName, userRole,dataEntity, ops))// Abhijeet - 30-10-2018 -Added role id
                    {
                        
                        return true;
                    }
                //}
                //else
                //{
                    
                //    throw new UnauthorizedAccessException(ErrorCodes.USER_DOES_NOT_EXISTS.ToString());
                //}
                return false;
            }
            catch (UnauthorizedAccessException ex)
            {
                LGSELogger.Error(ex);
                throw ex;
            }
            catch (Exception ex)
            {
                LGSELogger.Error(ex);
                throw ex;
            }

        }
        /// <summary>
        /// Checks whether user has any Roles or not.
        /// </summary>
        /// <param name="user"></param>
        private void CheckUserRoles(User user)
        {
            //UserRoleMap userRoleMap = dbContext.UserRoleMaps.FirstOrDefault(item => item.UserId == user.Id && item.Deleted==false);
            bool isroleexists = user.UserRoleMap.Any(i => i.Deleted == false);
            if (!isroleexists)
            {
                throw new UnauthorizedAccessException(ErrorCodes.USER_DOES_NOT_HAVE_ROLES.ToString());
            }
        }
        /// <summary>
        /// Checks the permissions of the roles
        /// </summary>
        /// <param name="user"></param>
        /// <param name="dataEntity"></param>
        /// <param name="op"></param>
        /// <returns></returns>
        private bool CheckPermission(string userName,string userRole, Features[] dataEntity, OperationType op )// Abhijeet - 30-10-2018 -Added role id
        {
            LGSELogger.Information("CheckPermission for UserName"+userName);
            // DbUtilities.dbContext = dbContext;
            // Role preferredRole =GetUserPreferredRole(userName);
            char roleChar='N';
            foreach (var item in dataEntity)
            {

               
                var rolePermission = (from rp in dbContext.RolePermissions
                        .Where(
                            i => i.Feature.FeatureName == item.ToString() &&
                                 i.RoleId == userRole
                                 && !i.Deleted)
                    select rp).FirstOrDefault();
                
                if (rolePermission != null)
                {
                    roleChar = GetPermissionCharByOperation(op, rolePermission);
                    LGSELogger.Information("Permission char for data entity {0},Operation {1}", dataEntity.ToString(),
                        op.ToString());
                    if (roleChar.Equals('A'))
                        return true;
                }
                else
                {
                    roleChar = 'N';
                }

                //var OperPerm = rolePermOnOPer.FirstOrDefault(item => item.Value != 'N');
               
            }
            if (roleChar.Equals('N'))
            {
                
                throw new UnauthorizedAccessException(ErrorCodes.USER_DOES_NOT_PERM_ON_OPERATION.ToString());
            }
            return true;
        }

        /// <summary>
        /// Return the permission based on operation
        /// </summary>
        /// <param name="operation"></param>
        /// <param name="rolePer"></param>
        /// <returns></returns>
        private char GetPermissionCharByOperation(OperationType operation,RolePermission rolePer)
        {
            char result='N';
            switch (operation)
            {
                case OperationType.CREATE:
                    result = rolePer.CreatePermission[0];
                    break;
                case OperationType.READ:
                    result = rolePer.ReadPermission[0];
                    break;
                case OperationType.UPDATE:
                    result = rolePer.UpdatePermission[0];
                    break;
                case OperationType.DELETE:
                    result = rolePer.DeletePermission[0];
                    break;
                default:
                    break;
            }
            return result;
        }
        ///// <summary>
        ///// Verifies the Roles Permission codes against the required codes
        ///// </summary>
        ///// <param name="rolesPermCodes"></param>
        ///// <param name="codes"></param>
        //private void CheckRolePermissionCodes(List<KeyValuePair<Role, char>> rolesPermCodes, Char[] codes)
        //{
        //    foreach (var rolePermCode in rolesPermCodes)
        //    {
        //        if (codes.Contains(rolePermCode.Value))
        //        {
        //            return;
        //        }
        //    }
        //    throw new UnauthorizedAccessException(Constants.NO_OPER_PERM);
        //}
        /// <summary>
        /// Get Incident Detials
        /// </summary>
        /// <param name="inputUsers"></param>
        /// <returns></returns>
        public  Role GetUserPreferredRole(string email)
        {
            //if single Role is present then return that.
            var allRoles = (from usr in dbContext.Users.Where(i => i.Email == email && i.Deleted == false).ToList()
                            join urm in dbContext.UserRoleMaps.Where(i => i.Deleted == false).ToList() on usr.Id equals urm.UserId
                            select new
                            {
                                Id = urm.Role.Id,
                                RoleName = urm.Role.RoleName,
                                urm.IsPreferredRole
                            }).ToList();

            if (allRoles != null && allRoles.Count > 1)
            {
                var result = allRoles.Where(i => i.IsPreferredRole == true).Select(i => new Role { Id = i.Id, RoleName = i.RoleName }).FirstOrDefault();
                if (result == null)
                {
                    //PReferred Role is not set by the user.
                    var response = HttpUtilities.FrameHTTPResp(System.Net.HttpStatusCode.BadRequest, ErrorCodes.USER_PREFERED_ROLE_NOTSET);
                    throw new HttpResponseException(response);
                }
                else
                {
                    return result;
                }
            }
            else
                return allRoles.Select(i => new Role { Id = i.Id, RoleName = i.RoleName }).FirstOrDefault();
        }

    }
}