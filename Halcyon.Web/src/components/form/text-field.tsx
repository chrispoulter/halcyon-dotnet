import {
    Controller,
    type Control,
    type FieldPath,
    type FieldValues,
} from 'react-hook-form';
import { Field, FieldError, FieldLabel } from '@/components/ui/field';
import { Input } from '@/components/ui/input';

type TextFieldProps<T extends FieldValues> = {
    control: Control<T>;
    name: FieldPath<T>;
    label?: string;
    type?: string;
    placeholder?: string;
    disabled?: boolean;
    maxLength?: number;
    autoComplete?: string;
    required?: boolean;
};

export function TextField<T extends FieldValues>({
    control,
    name,
    label,
    ...props
}: TextFieldProps<T>) {
    return (
        <Controller
            name={name}
            control={control}
            render={({ field, fieldState }) => (
                <Field data-invalid={fieldState.invalid}>
                    {label && (
                        <FieldLabel htmlFor={field.name}>{label}</FieldLabel>
                    )}
                    <Input
                        {...props}
                        {...field}
                        id={field.name}
                        aria-invalid={fieldState.invalid}
                    />
                    {fieldState.invalid && (
                        <FieldError errors={[fieldState.error]} />
                    )}
                </Field>
            )}
        />
    );
}
