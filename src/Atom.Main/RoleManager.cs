namespace Atom.Main
{
    public static class RoleManager
    {
        public static string[] AllRoles = new[]{
"Fusion.User",
"Fusion.IncidentUser",
"Fusion.IncidentAdmin",
"Fusion.ProjectAdmin",
"Fusion.ProjectUser",
"Fusion.CrfUser",
"Fusion.CrfAdmin",
"Fusion.IT",
"Fusion.SMT",
"Fusion.COM",
"Fusion.SuperUser",
"Fusion.ResourceUser",
"Fusion.ChangeBoard",
"Fusion.SupplierManager"};
        
        public static bool IsUserInRole(string role)
        {
            return true;
        }

        public static string[] GetRolesForUser()
        {
            return AllRoles;
        }

        public static string[] GetRolesForUser(string username)
        {
            return AllRoles;
        }

        public static void AddUserToRoles(string username, params string[] roleNames)
        {
        }

        public static void RemoveUserFromRoles(string username, params string[] roleNames)
        {
        }

        public static string[] GetUsersInRole(string roleName)
        {
            return new string[0];
        }

        public static bool RoleExists(string roleName)
        {
            return true;
        }

        public static bool UserExists(string username)
        {
            return true;
        }

        public static bool AddApplicationAccess(string username, string userid)
        {
            return true;
        }

        public static bool RemoveApplicationAccess(string username, string userid)
        {
            return true;
        }
    }
}