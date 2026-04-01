# API Permission Map

This document maps the suggested APIs to the permissions already present in the system, and highlights missing permissions that should be added for a cleaner authorization model.

## Legend
- `Public`: no authentication required.
- `Authenticated`: JWT required, no fine-grained permission yet.
- `Existing permission`: already present in the seeded permission set.
- `Suggested permission`: not present yet, recommended to add.

## API to Permission Map

| API | Method | Permission | Status | Notes |
|---|---|---|---|---|
| /api/auth/register | POST | Public | Public | Open endpoint |
| /api/auth/login | POST | Public | Public | Open endpoint |
| /api/auth/me | GET | Authenticated | Suggested permission | Recommended: `Auth.ReadSelf` or `Users.ReadSelf` |
| /api/auth/refresh-token | POST | Auth.Refresh | Suggested permission | Token lifecycle |
| /api/auth/logout | POST | Auth.Logout | Suggested permission | Token/session revoke |
| /api/auth/change-password | POST | Auth.ChangePassword | Suggested permission | Current user only |
| /api/auth/forgot-password | POST | Public | Public | Password recovery start |
| /api/auth/reset-password | POST | Public | Public | Password recovery finish |
| /api/users | GET | Users.Read | Existing permission | List users |
| /api/users/{id} | GET | Users.Read | Existing permission | Get user detail |
| /api/users | POST | Users.Write | Existing permission | Create user |
| /api/users/{id} | PUT | Users.Write | Existing permission | Update user |
| /api/users/{id}/status | PATCH | Users.Write | Existing permission | Enable/disable user |
| /api/users/{id}/roles | GET | Users.Read | Existing permission | View user roles |
| /api/users/{id}/roles | PUT | Users.Write | Existing permission | Assign roles |
| /api/users/{id}/permissions | GET | Permissions.Read | Existing permission | View direct permissions |
| /api/users/{id}/permissions | PUT | Permissions.Write | Existing permission | Assign direct permissions |
| /api/roles | GET | Roles.Read | Existing permission | List roles |
| /api/roles | POST | Roles.Write | Existing permission | Create role |
| /api/roles/{id} | PUT | Roles.Write | Existing permission | Update role |
| /api/roles/{id} | DELETE | Roles.Write | Existing permission | Consider adding `Roles.Delete` |
| /api/roles/{id}/permissions | GET | Roles.Read | Existing permission | View role permissions |
| /api/roles/{id}/permissions | PUT | Roles.Write | Existing permission | Assign permissions |
| /api/permissions | GET | Permissions.Read | Existing permission | List permissions |
| /api/permissions/by-resource/{resource} | GET | Permissions.Read | Existing permission | Filter by resource |
| /api/concerts | GET | Concerts.Read | Existing permission | List concerts |
| /api/concerts/{id} | GET | Concerts.Read | Existing permission | Concert detail |
| /api/concerts | POST | Concerts.Create | Existing permission | Create concert |
| /api/concerts/{id} | PUT | Concerts.Update | Existing permission | Update concert |
| /api/concerts/{id} | DELETE | Concerts.Delete | Existing permission | Delete concert |
| /api/concerts/{id}/tickets | GET | Tickets.Read | Existing permission | Tickets by concert |
| /api/concerts/{id}/availability | GET | Concerts.Read | Existing permission | Availability summary |
| /api/tickets | GET | Tickets.Read | Existing permission | List tickets |
| /api/tickets/{id} | GET | Tickets.Read | Existing permission | Ticket detail |
| /api/tickets/bulk-create | POST | Tickets.Create | Existing permission | Bulk create tickets |
| /api/tickets/{id}/status | PATCH | Tickets.Update | Existing permission | Update ticket status |
| /api/tickets/search | GET | Tickets.Read | Existing permission | Search/filter tickets |
| /api/tickets/{id}/reserve | POST | Tickets.Update | Existing permission | Reserve ticket |
| /api/tickets/{id}/release | POST | Tickets.Update | Existing permission | Release ticket |
| /api/orders | POST | Orders.Create | Existing permission | Create order |
| /api/orders/{id} | GET | Orders.Read | Existing permission | Order detail |
| /api/orders/my | GET | Orders.ReadSelf | Suggested permission | Current user orders only |
| /api/orders | GET | Orders.Read | Existing permission | List all orders |
| /api/orders/{id}/confirm | POST | Orders.Approve | Existing permission | Confirm order |
| /api/orders/{id}/cancel | POST | Orders.Update | Existing permission | Cancel order |
| /api/orders/{id}/expire | POST | Orders.Update | Existing permission | Expire order |
| /api/orders/{id}/tickets | GET | Orders.Read | Existing permission | Tickets in order |
| /api/payments/orders/{orderId}/create-intent | POST | Payments.Create | Suggested permission | Payment start |
| /api/payments/webhook | POST | Payments.Webhook | Suggested permission | Usually signature-based, not JWT-based |
| /api/payments/orders/{orderId}/status | GET | Payments.Read | Suggested permission | Payment status |
| /api/payments/orders/{orderId}/refund | POST | Payments.Refund | Suggested permission | Refund flow |
| /api/reports/sales | GET | Reports.Read | Suggested permission | Revenue report |
| /api/reports/concerts/{id}/occupancy | GET | Reports.Read | Suggested permission | Occupancy report |
| /api/reports/orders/export | GET | Orders.Export | Existing permission | Export orders |
| /api/reports/users/export | GET | Users.Export | Existing permission | Export users |
| /api/health | GET | Public or System.Health.Read | Suggested permission | Can be public/internal |
| /api/version | GET | Public | Public | No permission needed |
| /api/audit-logs | GET | AuditLogs.Read | Suggested permission | Audit trail |

## Suggested Missing Permissions

### Auth
- `Auth.ReadSelf`
- `Auth.Refresh`
- `Auth.Logout`
- `Auth.ChangePassword`

### Self-service
- `Users.ReadSelf`
- `Orders.ReadSelf`

### Payments
- `Payments.Read`
- `Payments.Create`
- `Payments.Refund`
- `Payments.Webhook`

### Reports and Audit
- `Reports.Read`
- `AuditLogs.Read`

### Optional finer-grained permissions
- `Roles.Delete`
- `Users.Delete`
- `Orders.Cancel`
- `Orders.Expire`
- `Tickets.Reserve`
- `Tickets.Release`

## Recommended Role Strategy

### Admin
- Full access to existing permissions.
- Add the suggested permissions for payments, reports, audit, and auth management.

### Operator
- `Concerts.Read`, `Concerts.Create`, `Concerts.Update`
- `Tickets.Read`, `Tickets.Update`, `Tickets.Create`
- `Orders.Read`, `Orders.Update`, `Orders.Approve`
- `Reports.Read`

### Customer
- `Concerts.Read`
- `Tickets.Read`
- `Orders.Create`
- `Orders.ReadSelf`
- `Auth.ReadSelf`
- `Auth.ChangePassword`

## Notes
- The current seeded permission set already covers most CRUD-style admin workflows.
- The main gaps are self-service auth, payment processing, audit logs, and cleaner separation between own-data access and full admin access.
- If you want, this table can be converted into a seed-ready permission matrix for `DatabaseSeeder.cs`.
