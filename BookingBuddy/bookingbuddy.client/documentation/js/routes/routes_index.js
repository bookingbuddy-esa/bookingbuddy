var ROUTES_INDEX = {"name":"<root>","kind":"module","className":"AppModule","children":[{"name":"routes","filename":"src/app/app-routing.module.ts","module":"AppRoutingModule","children":[{"path":"","component":"HomepageComponent"},{"path":"hosting/create","component":"PropertyAdCreateComponent","canActivate":["AuthGuard"]},{"path":"hosting/calendar","component":"CalendarComponent","canActivate":["AuthGuard","LandlordRoleGuardService"]},{"path":"hosting/promote","component":"PropertyPromoteComponent","canActivate":["AuthGuard","LandlordRoleGuardService"]},{"path":"hosting/performance","component":"PropertyPerformanceComponent","canActivate":["AuthGuard","LandlordRoleGuardService"]},{"path":"hosting/bookings","component":"HostingBookingComponent","canActivate":["AuthGuard","LandlordRoleGuardService"]},{"path":"property/:id","component":"PropertyAdRetrieveComponent"},{"path":"bookings","component":"BookingComponent","canActivate":["AuthGuard"]},{"path":"profile","component":"ProfileComponent","canActivate":["AuthGuard"]},{"path":"unauthorized","component":"UnauthorizedComponent"},{"path":"bad-request","component":"BadRequestComponent"},{"path":"forbidden","component":"ForbiddenComponent"},{"path":"**","component":"NotfoundComponent"}],"kind":"module"}]}
