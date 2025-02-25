import { HttpClient, HttpParams } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';

import { Team } from './models/team';

@Injectable({
    providedIn: 'root'
})
export class TeamsRepositoryService {
    private readonly baseUrl = 'http://localhost:8080/code-review';
    private readonly apiVersion = '1.0';

    constructor(private http: HttpClient) {}

    getTeamsByDescription(description: string): Observable<Team[]> {
        const params = new HttpParams()
            .set('teamName', description)
            .set('api-version', this.apiVersion);

        return this.http.get<Team[]>(`${this.baseUrl}/api/teams`, { params });
    }
}
