import { format, isBefore, startOfDay } from 'date-fns';
import { UTCDate, utc } from '@date-fns/utc';

export const currentYear = new Date().getFullYear();

export function toDisplay(value: string | Date) {
    return format(value, 'PPP', { in: utc });
}

export function toDateOnly(value: string | Date | undefined) {
    return value ? format(value, 'yyyy-MM-dd', { in: utc }) : undefined;
}

export function isInPast(value: string | Date) {
    const today = startOfDay(new UTCDate());
    const checkDate = startOfDay(value);
    return isBefore(checkDate, today);
}
