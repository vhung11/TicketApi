namespace TicketApi.Modules.Identity.Authorization
{
    /// <summary>
    /// Centralized permission code constants matching the "Resource.Action" convention
    /// used in DatabaseSeeder. Prevents magic strings throughout the codebase.
    /// </summary>
    public static class Permissions
    {
        public static class Users
        {
            public const string Read = "Users.Read";
            public const string Write = "Users.Write";
            public const string Export = "Users.Export";
        }

        public static class Roles
        {
            public const string Read = "Roles.Read";
            public const string Write = "Roles.Write";
            public const string Export = "Roles.Export";
        }

        public static class PermissionMgmt
        {
            public const string Read = "Permissions.Read";
            public const string Write = "Permissions.Write";
        }

        public static class Concerts
        {
            public const string Read = "Concerts.Read";
            public const string Create = "Concerts.Create";
            public const string Update = "Concerts.Update";
            public const string Delete = "Concerts.Delete";
            public const string Export = "Concerts.Export";
            public const string Approve = "Concerts.Approve";
        }

        public static class Orders
        {
            public const string Read = "Orders.Read";
            public const string Create = "Orders.Create";
            public const string Update = "Orders.Update";
            public const string Export = "Orders.Export";
            public const string Approve = "Orders.Approve";
        }

        public static class Tickets
        {
            public const string Read = "Tickets.Read";
            public const string Create = "Tickets.Create";
            public const string Update = "Tickets.Update";
            public const string Export = "Tickets.Export";
        }
    }
}
