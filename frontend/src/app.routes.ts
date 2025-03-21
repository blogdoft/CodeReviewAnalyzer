import { Routes } from '@angular/router';
import { AppLayout } from './app/layout/component/app.layout';
import { Dashboard } from './app/pages/dashboard/dashboard';
import { Landing } from './app/pages/landing/landing';
import { Notfound } from './app/pages/notfound/notfound';
import { ForbiddenComponent } from './app/pages/forbidden/forbidden';

export const appRoutes: Routes = [
    {
        path: '',
        component: AppLayout,
        children: [
            { path: '', component: Dashboard },
            {
                path: 'admin',
                loadChildren: () => import('./app/pages/pages.routes')
            }
        ]
    },
    { path: 'landing', component: Landing },
    { path: 'notfound', component: Notfound },
    { path: 'forbidden', component: ForbiddenComponent },
    { path: '**', redirectTo: '/notfound' }
];
