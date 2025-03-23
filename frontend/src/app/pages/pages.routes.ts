import { Routes } from '@angular/router';

import { TeamCrud } from './team-crud/team-crud';
import { canActivateAuthRole } from '../guards/auth.guard';
import { ForbiddenComponent } from './forbidden/forbidden';

export default [
    {
        path: 'teams',
        component: TeamCrud,
        canActivate: [canActivateAuthRole],
        data: { role: 'project-admin' }
    },
    { path: 'forbidden', component: ForbiddenComponent },
    { path: '**', redirectTo: '/notfound', pathMatch: 'full' }
] as Routes;
