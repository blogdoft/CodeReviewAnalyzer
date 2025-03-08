import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { PullRequestStats } from './models/pull-request-stats';
import { Observable } from 'rxjs';

@Injectable({
    providedIn: 'root'
})
export class PullRequestRepositoryService {
    private readonly baseUrl = 'http://localhost:8080/code-review';
    private readonly apiVersion = '1.0';

    constructor(private http: HttpClient) {}

    public getStatsFor(externalId: string): Observable<PullRequestStats> {
        return this.http.get<PullRequestStats>(
            `${this.baseUrl}/api/pull-requests/${externalId}:stats`
        );
    }
}
