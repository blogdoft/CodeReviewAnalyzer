import { Routes } from '@angular/router';

import { TeamCrud } from './team-crud/team-crud';

export default [
    { path: 'teams', component: TeamCrud },
    { path: '**', redirectTo: '/notfound' }
] as Routes;
