import { HttpClient, HttpParams } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';

import { CommentData } from './models/comment-data.model';
import { PullRequestTimeReport } from './models/pull-request-report.model';

@Injectable({
  providedIn: 'root',
})
export class PullRequestReportService {
  private readonly baseUrl = 'http://localhost:8080/code-review';
  private readonly apiVersion = '1.0';

  constructor(private http: HttpClient) {}

  /**
   * Obtém os relatórios para um período informado.
   * @param begin Data de início no formato YYYY-MM-DD.
   * @param end Data de término no formato YYYY-MM-DD.
   * @param apiVersion Versão da API (padrão: '1.0').
   */
  getReports(begin: Date, end: Date): Observable<PullRequestTimeReport> {
    const params = new HttpParams()
      .set('begin', begin.toISOString().split('T')[0])
      .set('end', end.toISOString().split('T')[0])
      .set('api-version', this.apiVersion);

    return this.http.get<PullRequestTimeReport>(
      `${this.baseUrl}/api/Reports/pull-requests`,
      {
        params,
      }
    );
  }

  getReviewerDensity(from: Date, to: Date): Observable<CommentData[]> {
    const params = new HttpParams()
      .set('begin', from.toISOString().split('T')[0])
      .set('end', to.toISOString().split('T')[0])
      .set('api-version', this.apiVersion);

    return this.http.get<CommentData[]>(`${this.baseUrl}/api/Reports/density`, {
      params,
    });
  }
}
