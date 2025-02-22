import { MetricStatus } from './metric-status.enum';

export interface MetricStatsOptions {
    title: string;
    current: DataPair;
    previous: DataPair;
    icon?: string | null;
    status: MetricStatus;
}

export interface DataPair {
    value: string;
    referenceDate: Date;
}
