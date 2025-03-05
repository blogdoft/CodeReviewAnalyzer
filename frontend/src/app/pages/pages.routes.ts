import { Routes } from '@angular/router';

import { TeamCrud } from './team-crud/crud';

export default [
    { path: 'team', component: TeamCrud },
    { path: '**', redirectTo: '/notfound' }
] as Routes;
