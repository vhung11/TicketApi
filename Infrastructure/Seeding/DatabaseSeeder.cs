using Microsoft.EntityFrameworkCore;
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

        public static async Task SeedAsync(ApplicationDbContext context)
        {
            await context.Database.EnsureCreatedAsync();

            var permissionDefinitions = GetPermissionDefinitions();
            var rolesByName = await EnsureRolesAsync(context);
            var permissionsByCode = await EnsurePermissionsAsync(context, permissionDefinitions);

            await EnsureRolePermissionsAsync(
                context,
                rolesByName,
                permissionsByCode);

            await EnsureUsersAsync(context, rolesByName);
            await EnsureSampleDataAsync(context);
        }

        private static async Task<Dictionary<string, Role>> EnsureRolesAsync(ApplicationDbContext context)
        {
            var existingRoles = await context.Roles.ToDictionaryAsync(r => r.Name);
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
                await context.SaveChangesAsync();
            }

            return existingRoles;
        }

        private static async Task<Dictionary<string, Permission>> EnsurePermissionsAsync(
            ApplicationDbContext context,
            IEnumerable<(string Code, string Resource)> definitions)
        {
            var existingPermissions = await context.Permissions.ToDictionaryAsync(p => p.Code);

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
                await context.SaveChangesAsync();
            }

            return existingPermissions;
        }

        private static async Task EnsureRolePermissionsAsync(
            ApplicationDbContext context,
            IReadOnlyDictionary<string, Role> rolesByName,
            IReadOnlyDictionary<string, Permission> permissionsByCode)
        {
            var existingPairsList = await context.RolePermissions.ToListAsync();
            var existingPairs = existingPairsList
                .Select(rp => (rp.RoleId, rp.PermissionId))
                .ToHashSet();

            var adminRole = rolesByName[AdminRoleName];
            var adminPermissionIds = permissionsByCode.Values
                .Select(permission => permission.Id)
                .ToHashSet();
            await SyncRolePermissionsAsync(context, existingPairs, adminRole.Id, adminPermissionIds);

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
            await SyncRolePermissionsAsync(context, existingPairs, userRole.Id, userPermissionIds);

            if (context.ChangeTracker.HasChanges())
            {
                await context.SaveChangesAsync();
            }
        }

        private static async Task EnsureUsersAsync(
            ApplicationDbContext context,
            IReadOnlyDictionary<string, Role> rolesByName)
        {
            var existingUsers = await context.Users.ToDictionaryAsync(u => u.Email);
            var existingAssignmentsList = await context.UserRoles.ToListAsync();
            var existingAssignments = existingAssignmentsList
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
                await context.SaveChangesAsync();
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
                await context.SaveChangesAsync();
            }
        }

        private static async Task EnsureSampleDataAsync(ApplicationDbContext context)
        {
            if (await context.Concerts.AnyAsync()) return;

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
            await context.SaveChangesAsync();

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

            await context.SaveChangesAsync();
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

        private static async Task SyncRolePermissionsAsync(
            ApplicationDbContext context,
            ISet<(int RoleId, int PermissionId)> existingPairs,
            int roleId,
            ISet<int> expectedPermissionIds)
        {
            var rolePermissions = await context.RolePermissions
                .Where(rp => rp.RoleId == roleId)
                .ToListAsync();

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
