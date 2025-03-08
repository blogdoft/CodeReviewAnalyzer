import { TestBed } from '@angular/core/testing';

import { PullRequestRepositoryService } from './pull-request-repository.service';

describe('PullRequestRepositoryService', () => {
    let service: PullRequestRepositoryService;

    beforeEach(() => {
        TestBed.configureTestingModule({});
        service = TestBed.inject(PullRequestRepositoryService);
    });

    it('should be created', () => {
        expect(service).toBeTruthy();
    });
});
