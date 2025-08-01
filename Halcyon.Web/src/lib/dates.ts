import { format, isBefore, startOfDay } from 'date-fns';

export const currentYear = new Date().getFullYear();

export function toDisplay(value: string | Date) {
    return format(value, 'PPP');
}

export function toDateOnly(value: string | Date | undefined) {
    return value ? format(value, 'yyyy-MM-dd') : undefined;
}

export function isInPast(value: string | Date) {
    const today = startOfDay(new Date());
    const checkDate = startOfDay(value);
    return isBefore(checkDate, today);
}
