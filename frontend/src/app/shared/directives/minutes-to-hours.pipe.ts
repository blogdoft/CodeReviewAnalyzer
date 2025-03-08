import { Pipe, PipeTransform } from '@angular/core';

@Pipe({
    name: 'minToHours'
})
export class MinutesToHoursPipe implements PipeTransform {
    transform(value?: number | null): string {
        if (!value) return '';

        if (value === 1) {
            return '1 minute';
        }

        if (value < 60) {
            return `${value} minutes`;
        }

        const hours = (value / 60) | 0;

        const minutes = value - hours * 60;

        if (hours === 1) {
            return `${hours} hour and ${minutes} minutes`;
        }

        return `${hours} hours and ${minutes} minutes`;
    }
}
