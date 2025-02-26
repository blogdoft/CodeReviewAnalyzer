import { HttpClient, HttpParams } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable, tap } from 'rxjs';

import { CommentData } from './models/comment-data.model';
import { PullRequestTimeReport } from './models/pull-request-report.model';

@Injectable({
    providedIn: 'root'
})
export class PullRequestReportService {
    private readonly baseUrl = 'http://localhost:8080/code-review';
    private readonly apiVersion = '1.0';

    constructor(private http: HttpClient) {}

    /**
     * Obtém os relatórios para um período informado.
     * @param from Data de início no formato YYYY-MM-DD.
     * @param to Data de término no formato YYYY-MM-DD.
     * @param apiVersion Versão da API (padrão: '1.0').
     */
    getReports(
        from: Date,
        to: Date,
        repoTeamId: string | null,
        userTeamId: string | null
    ): Observable<PullRequestTimeReport> {
        let params = new HttpParams()
            .set('from', from.toISOString().split('T')[0])
            .set('to', to.toISOString().split('T')[0])
            .set('api-version', this.apiVersion);

        if (repoTeamId) params = params.set('repoTeamId', repoTeamId);
        if (userTeamId) params = params.set('userTeamId', userTeamId);

        return this.http.get<PullRequestTimeReport>(
            `${this.baseUrl}/api/reports/pull-requests`,
            {
                params
            }
        );
    }

    getReviewerDensity(
        from: Date,
        to: Date,
        repoTeamId: string | null,
        userTeamId: string | null
    ): Observable<CommentData[]> {
        let params = new HttpParams()
            .set('from', from.toISOString().split('T')[0])
            .set('to', to.toISOString().split('T')[0])
            .set('api-version', this.apiVersion);

        if (repoTeamId) params = params.set('repoTeamId', repoTeamId);
        if (userTeamId) params = params.set('userTeamId', userTeamId);

        return this.http.get<CommentData[]>(
            `${this.baseUrl}/api/reports/density`,
            {
                params
            }
        );
    }
}
