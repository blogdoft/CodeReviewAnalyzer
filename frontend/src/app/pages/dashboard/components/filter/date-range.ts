export interface DashboardFilter {
    teamRepositoryId: string | null;
    teamUserId: string | null;
    dateRange: DateRange;
}

export interface DateRange {
    from: Date;
    to: Date;
}
