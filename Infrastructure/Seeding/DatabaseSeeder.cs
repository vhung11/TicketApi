using TicketApi.Infrastructure.Context;
using TicketApi.Modules.Identity.Entities;
using TicketApi.Modules.Orders.Entities;
using TicketApi.Modules.Orders.Enums;

namespace TicketApi.Infrastructure.Seeding
{
    public static class DatabaseSeeder
    {
        private const string AdminRoleName = "Admin";
        private const string UserRoleName = "User";
        private const string AdminEmail = "admin@ticketapi.local";
        private const string AdminPassword = "Admin@123";
        private const string AdminDisplayName = "System Administrator";

        public static void Seed(ApplicationDbContext context)
        {
            context.Database.EnsureCreated();

            var permissionDefinitions = GetPermissionDefinitions();
            var rolesByName = EnsureRoles(context);
            var permissionsByCode = EnsurePermissions(context, permissionDefinitions);

            EnsureRolePermissions(
                context,
                rolesByName,
                permissionsByCode);

            EnsureUsers(context, rolesByName);
            EnsureSampleData(context);
        }

        private static Dictionary<string, Role> EnsureRoles(ApplicationDbContext context)
        {
            var existingRoles = context.Roles.ToDictionary(r => r.Name);
            var roleNames = new[] { AdminRoleName, UserRoleName };

            foreach (var roleName in roleNames)
            {
                if (existingRoles.ContainsKey(roleName))
                {
                    continue;
                }

                var role = new Role
                {
                    Name = roleName,
                    IsActive = true
                };

                context.Roles.Add(role);
                existingRoles[roleName] = role;
            }

            if (context.ChangeTracker.HasChanges())
            {
                context.SaveChanges();
            }

            return existingRoles;
        }

        private static Dictionary<string, Permission> EnsurePermissions(
            ApplicationDbContext context,
            IEnumerable<(string Code, string Resource)> definitions)
        {
            var existingPermissions = context.Permissions.ToDictionary(p => p.Code);

            foreach (var (code, resource) in definitions)
            {
                if (existingPermissions.ContainsKey(code))
                {
                    continue;
                }

                var permission = new Permission
                {
                    Code = code,
                    Resource = resource,
                    IsActive = true
                };

                context.Permissions.Add(permission);
                existingPermissions[code] = permission;
            }

            if (context.ChangeTracker.HasChanges())
            {
                context.SaveChanges();
            }

            return existingPermissions;
        }

        private static void EnsureRolePermissions(
            ApplicationDbContext context,
            IReadOnlyDictionary<string, Role> rolesByName,
            IReadOnlyDictionary<string, Permission> permissionsByCode)
        {
            var existingPairs = context.RolePermissions
                .AsEnumerable()
                .Select(rp => (rp.RoleId, rp.PermissionId))
                .ToHashSet();

            var adminRole = rolesByName[AdminRoleName];
            var adminPermissionIds = permissionsByCode.Values
                .Select(permission => permission.Id)
                .ToHashSet();
            SyncRolePermissions(context, existingPairs, adminRole.Id, adminPermissionIds);

            var userRole = rolesByName[UserRoleName];
            var userPermissionCodes = new[]
            {
                "Concerts.Read",
                "Tickets.Read",
                "Orders.Create",
                "Orders.Read",
                "Orders.Update"
            };
            var userPermissionIds = userPermissionCodes
                .Where(permissionsByCode.ContainsKey)
                .Select(code => permissionsByCode[code].Id)
                .ToHashSet();
            SyncRolePermissions(context, existingPairs, userRole.Id, userPermissionIds);

            if (context.ChangeTracker.HasChanges())
            {
                context.SaveChanges();
            }
        }

        private static void EnsureUsers(
            ApplicationDbContext context,
            IReadOnlyDictionary<string, Role> rolesByName)
        {
            var existingUsers = context.Users.ToDictionary(u => u.Email);
            var existingAssignments = context.UserRoles
                .AsEnumerable()
                .Select(ur => (ur.UserId, ur.RoleId))
                .ToHashSet();
            var createdAdminUser = false;

            if (!existingUsers.TryGetValue(AdminEmail, out var adminUser))
            {
                adminUser = new User
                {
                    Name = AdminDisplayName,
                    Email = AdminEmail,
                    PasswordHash = BCrypt.Net.BCrypt.HashPassword(AdminPassword),
                    IsActive = true
                };

                context.Users.Add(adminUser);
                existingUsers[AdminEmail] = adminUser;
                createdAdminUser = true;
            }
            else
            {
                adminUser.Name = AdminDisplayName;
                adminUser.IsActive = true;
            }

            if (createdAdminUser)
            {
                context.SaveChanges();
            }

            var adminRole = rolesByName[AdminRoleName];
            if (existingAssignments.Add((adminUser.Id, adminRole.Id)))
            {
                context.UserRoles.Add(new UserRole
                {
                    UserId = adminUser.Id,
                    RoleId = adminRole.Id
                });
            }

            if (context.ChangeTracker.HasChanges())
            {
                context.SaveChanges();
            }
        }

        private static void EnsureSampleData(ApplicationDbContext context)
        {
            if (context.Concerts.Any()) return;

            var concerts = new List<Concert>
            {
                new Concert { Name = "Taylor Swift - The Eras Tour", Venue = "National Stadium", StartTime = DateTime.UtcNow.AddDays(30), EndTime = DateTime.UtcNow.AddDays(30).AddHours(3) },
                new Concert { Name = "Coldplay - Music of the Spheres", Venue = "Skyline Arena", StartTime = DateTime.UtcNow.AddDays(45), EndTime = DateTime.UtcNow.AddDays(45).AddHours(4) },
                new Concert { Name = "Ed Sheeran - Mathematics Tour", Venue = "Grand Plaza", StartTime = DateTime.UtcNow.AddDays(15), EndTime = DateTime.UtcNow.AddDays(15).AddHours(2.5) },
                new Concert { Name = "BlackPink - Born Pink", Venue = "Metropolitan Hall", StartTime = DateTime.UtcNow.AddDays(60), EndTime = DateTime.UtcNow.AddDays(60).AddHours(3) },
                new Concert { Name = "Bruno Mars - Live in Asia", Venue = "Emerald Gardens", StartTime = DateTime.UtcNow.AddDays(20), EndTime = DateTime.UtcNow.AddDays(20).AddHours(3) },
                new Concert { Name = "The Weeknd - After Hours Til Dawn", Venue = "Crystal Dome", StartTime = DateTime.UtcNow.AddDays(10), EndTime = DateTime.UtcNow.AddDays(10).AddHours(3.5) },
                new Concert { Name = "Imagine Dragons - Mercury Tour", Venue = "Thunder Field", StartTime = DateTime.UtcNow.AddDays(5), EndTime = DateTime.UtcNow.AddDays(5).AddHours(2.5) }
            };

            context.Concerts.AddRange(concerts);
            context.SaveChanges();

            foreach (var concert in concerts)
            {
                // Seed some tickets for each concert
                var tickets = new List<Ticket>();

                // VIP Tickets
                for (int i = 1; i <= 5; i++)
                {
                    tickets.Add(new Ticket
                    {
                        ConcertId = concert.Id,
                        Zone = TicketZone.VIP,
                        Price = 500m,
                        Status = TicketStatus.Available,
                        SeatNumber = $"V-{i:D3}"
                    });
                }

                // Standard Tickets
                for (int i = 1; i <= 10; i++)
                {
                    tickets.Add(new Ticket
                    {
                        ConcertId = concert.Id,
                        Zone = TicketZone.STANDARD,
                        Price = 250m,
                        Status = TicketStatus.Available,
                        SeatNumber = $"S-{i:D3}"
                    });
                }

                // Economy Tickets
                for (int i = 1; i <= 5; i++)
                {
                    tickets.Add(new Ticket
                    {
                        ConcertId = concert.Id,
                        Zone = TicketZone.ECONOMY,
                        Price = 1000m,
                        Status = TicketStatus.Available,
                        SeatNumber = $"E-{i:D3}"
                    });
                }

                context.Tickets.AddRange(tickets);
            }

            context.SaveChanges();
        }

        private static void AddRolePermissionIfMissing(
            ApplicationDbContext context,
            ISet<(int RoleId, int PermissionId)> existingPairs,
            int roleId,
            int permissionId)
        {
            if (!existingPairs.Add((roleId, permissionId)))
            {
                return;
            }

            context.RolePermissions.Add(new RolePermission
            {
                RoleId = roleId,
                PermissionId = permissionId
            });
        }

        private static void SyncRolePermissions(
            ApplicationDbContext context,
            ISet<(int RoleId, int PermissionId)> existingPairs,
            int roleId,
            ISet<int> expectedPermissionIds)
        {
            var rolePermissions = context.RolePermissions
                .Where(rp => rp.RoleId == roleId)
                .ToList();

            foreach (var rolePermission in rolePermissions)
            {
                if (expectedPermissionIds.Contains(rolePermission.PermissionId))
                {
                    continue;
                }

                context.RolePermissions.Remove(rolePermission);
                existingPairs.Remove((rolePermission.RoleId, rolePermission.PermissionId));
            }

            foreach (var permissionId in expectedPermissionIds)
            {
                AddRolePermissionIfMissing(context, existingPairs, roleId, permissionId);
            }
        }

        private static IEnumerable<(string Code, string Resource)> GetPermissionDefinitions()
        {
            var resources = new Dictionary<string, string[]>
            {
                ["Users"] = new[] { "Read", "Write", "Export" },
                ["Roles"] = new[] { "Read", "Write", "Export" },
                ["Permissions"] = new[] { "Read", "Write" },
                ["Concerts"] = new[] { "Read", "Create", "Update", "Delete", "Export", "Approve" },
                ["Orders"] = new[] { "Read", "Create", "Update", "Export", "Approve" },
                ["Tickets"] = new[] { "Read", "Create", "Update", "Export" }
            };

            foreach (var (resource, actions) in resources)
            {
                foreach (var action in actions)
                {
                    yield return ($"{resource}.{action}", resource);
                }
            }
        }
    }
}
