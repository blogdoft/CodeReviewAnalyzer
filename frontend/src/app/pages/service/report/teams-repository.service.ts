import { HttpClient, HttpParams } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';

import { PageFilter } from './models/page-filter';
import { TeamsPaginated } from './models/teams-paginated';

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
}
