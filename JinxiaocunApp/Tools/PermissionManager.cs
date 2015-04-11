using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using JinxiaocunApp.Models;
using System.Web.Security;

namespace JinxiaocunApp.Tools
{
    public class PermissionManager
    {
        public static bool CheckUserHasPermision(string userName, string permissionName)
        {
            List<Role> roleList = new List<Role>();
            List<PermissionsInRoles> permissionsInRolesList = new List<PermissionsInRoles>();
            using (JinxiaocunAppContext db = new JinxiaocunAppContext())
            {
                roleList = db.Roles.AsEnumerable<Role>().ToList<Role>();

            }

            using (JinxiaocunAppContext db = new JinxiaocunAppContext())
            {

                permissionsInRolesList = db.PermissionsInRoles.Include("Permission").Include("Role")
                                            .AsEnumerable<PermissionsInRoles>().ToList<PermissionsInRoles>();
            }

            string[] currentRoles = Roles.GetRolesForUser(userName);
            foreach (var roleName in currentRoles)
            {
                List<Permission> permissionList = permissionsInRolesList.Where(e => e.Role.RoleName == roleName)
                                                                            .Select(e => e.Permission).ToList<Permission>();

                foreach (var permission in permissionList)
                {
                    if (permission.PermissionName == permissionName)
                    {
                        return true;
                    }
                }
            }

            return false;
        }
    }
}