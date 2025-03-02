import { HttpClient, HttpHeaders, HttpParams } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';

import { PageFilter } from '../report/models/page-filter';
import { TeamsPaginated } from './Models/teams-paginated';
import { TeamUser } from './Models/TeamUser';
import { Team } from './Models/team';

@Injectable({
    providedIn: 'root'
})
export class TeamsRepositoryService {
    private readonly baseUrl = 'http://localhost:8080/code-review';

    constructor(private http: HttpClient) {}

    getTeamsByDescription(
        description: string,
        pageFilter: PageFilter
    ): Observable<TeamsPaginated> {
        let params = new HttpParams()
            .set('teamName', description)
            .set('page', pageFilter.page)
            .set('size', pageFilter.size);

        if (pageFilter.order) params = params.set('order', pageFilter.order);

        return this.http.get<TeamsPaginated>(`${this.baseUrl}/api/teams`, {
            params
        });
    }

    createTeam(team: Team): Observable<Team> {
        const headers = new HttpHeaders().set('accept', 'application/json');
        return this.http.post<Team>(`${this.baseUrl}/api/teams`, team, {
            headers
        });
    }

    updateTeam(team: Team): Observable<Team> {
        const headers = new HttpHeaders().set('accept', 'application/json');
        return this.http.put<Team>(
            `${this.baseUrl}/api/teams/${team.externalId}`,
            team,
            {
                headers
            }
        );
    }

    deleteTeam(teamId: string): Observable<any> {
        return this.http.delete<any>(`${this.baseUrl}/api/teams/${teamId}`);
    }

    getUsersFromTeam(teamId: string): Observable<TeamUser[]> {
        return this.http.get<TeamUser[]>(
            `${this.baseUrl}/api/teams/${teamId}/users`
        );
    }

    deleteUserFromTeam(teamId: string, userId: string): Observable<TeamUser[]> {
        const headers = new HttpHeaders().set('accept', 'application/json');
        return this.http.delete<any>(
            `${this.baseUrl}/api/teams/${teamId}/users/${userId}`,
            { headers }
        );
    }

    addUserToTeam(
        teamId: string,
        userId: string,
        role: string
    ): Observable<TeamUser[]> {
        var userTeam: TeamUser = {
            user: {
                id: userId,
                name: 'Random name',
                active: true
            },
            role: role
        };
        return this.http.post<TeamUser[]>(
            `${this.baseUrl}/api/teams/${teamId}/users`,
            [userTeam]
        );
    }
}
