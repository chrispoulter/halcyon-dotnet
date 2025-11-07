import { useState } from 'react';
import {
    Controller,
    type Control,
    type FieldPath,
    type FieldValues,
} from 'react-hook-form';
import { ChevronDownIcon } from 'lucide-react';
import { Button } from '@/components/ui/button';
import { Calendar } from '@/components/ui/calendar';
import { Field, FieldError, FieldLabel } from '@/components/ui/field';
import {
    Popover,
    PopoverTrigger,
    PopoverContent,
} from '@/components/ui/popover';
import { isInPast, toDateOnly, toDisplay } from '@/lib/dates';
import { cn } from '@/lib/utils';

interface DateFieldProps<T extends FieldValues> {
    control: Control<T>;
    name: FieldPath<T>;
    label: string;
    required?: boolean;
    disabled?: boolean;
}

export function DateField<T extends FieldValues>({
    control,
    name,
    label,
    required,
    disabled,
}: DateFieldProps<T>) {
    const [open, setOpen] = useState(false);

    return (
        <Controller
            name={name}
            control={control}
            render={({ field, fieldState }) => {
                function onSelect(date?: Date) {
                    const value = toDateOnly(date);
                    field.onChange(value);
                    setOpen(false);
                }

                function isDateDisabled(date: Date) {
                    return !isInPast(date);
                }

                return (
                    <Field data-invalid={fieldState.invalid}>
                        <FieldLabel htmlFor={name}>{label}</FieldLabel>
                        <Popover open={open} onOpenChange={setOpen}>
                            <PopoverTrigger asChild>
                                <Button
                                    variant="outline"
                                    disabled={disabled}
                                    aria-invalid={fieldState.invalid}
                                    className={cn(
                                        'w-48 justify-between font-normal',
                                        !field.value && 'text-muted-foreground'
                                    )}
                                >
                                    {field.value ? (
                                        toDisplay(field.value)
                                    ) : (
                                        <span>Select...</span>
                                    )}
                                    <ChevronDownIcon />
                                </Button>
                            </PopoverTrigger>
                            <PopoverContent
                                className="w-auto overflow-hidden p-0"
                                align="start"
                            >
                                <Calendar
                                    timeZone="UTC"
                                    mode="single"
                                    captionLayout="dropdown"
                                    selected={field.value}
                                    defaultMonth={field.value}
                                    required={required}
                                    onSelect={onSelect}
                                    disabled={isDateDisabled}
                                />
                            </PopoverContent>
                        </Popover>
                        {fieldState.invalid && (
                            <FieldError errors={[fieldState.error]} />
                        )}
                    </Field>
                );
            }}
        />
    );
}
