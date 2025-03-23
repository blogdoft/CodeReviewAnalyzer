import {
    ActivatedRouteSnapshot,
    CanActivateFn,
    Router,
    RouterStateSnapshot,
    UrlTree
} from '@angular/router';
import { inject } from '@angular/core';
import Keycloak from 'keycloak-js';
import { AuthGuardData, createAuthGuard } from 'keycloak-angular';

const isAccessAllowed = async (
    route: ActivatedRouteSnapshot,
    snap: RouterStateSnapshot,
    authData: AuthGuardData,
    router: Router
): Promise<boolean | UrlTree> => {
    const { authenticated, grantedRoles } = authData;

    const requiredRole = route.data['role'];

    if (authenticated && !requiredRole) return true;

    // TODO: parametro role pode ser um array

    const hasRequiredRole = (role: string): boolean =>
        grantedRoles.resourceRoles &&
        Object.values(grantedRoles.resourceRoles).some((roles) =>
            roles.includes(role)
        );

    if (authenticated && hasRequiredRole(requiredRole)) {
        return true;
    }

    return router.parseUrl('/forbidden');
};

export const canActivateAuthRole = createAuthGuard<CanActivateFn>(
    (route, snap, authData) =>
        isAccessAllowed(route, snap, authData, inject(Router))
);
