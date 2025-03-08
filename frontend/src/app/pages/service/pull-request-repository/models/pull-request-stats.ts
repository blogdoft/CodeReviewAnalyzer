export interface PullRequestStats {
    externalId?: string;
    title?: string;
    createdAt?: string;
    closedAt?: string;
    firstCommentDate?: string;
    firstCommentWaitingMinutes?: number;
    revisionWaitingTimeMinutes?: number;
    mergeWaitingTimeMinutes?: number | null;
    mergeMode?: string;
    fileCount?: number;
    threadCount?: number;
}
