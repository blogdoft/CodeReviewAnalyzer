import { TestBed } from '@angular/core/testing';

import { PullRequestReportService } from './pullrequest.report.service';

describe('PullrequestReportService', () => {
    let service: PullRequestReportService;

    beforeEach(() => {
        TestBed.configureTestingModule({});
        service = TestBed.inject(PullRequestReportService);
    });

    it('should be created', () => {
        expect(service).toBeTruthy();
    });
});
