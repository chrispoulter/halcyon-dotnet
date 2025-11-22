import {
    Controller,
    type Control,
    type FieldPath,
    type FieldValues,
} from 'react-hook-form';
import { REGEXP_ONLY_DIGITS } from 'input-otp';
import { Field, FieldError, FieldLabel } from '@/components/ui/field';
import {
    InputOTP,
    InputOTPGroup,
    InputOTPSlot,
} from '@/components/ui/input-otp';

interface OtpFieldProps<T extends FieldValues>
    extends React.ComponentProps<'input'> {
    control: Control<T>;
    name: FieldPath<T>;
    label?: string;
}

export function OtpField<T extends FieldValues>({
    control,
    name,
    label,
    ...props
}: OtpFieldProps<T>) {
    return (
        <Controller
            name={name}
            control={control}
            render={({ field, fieldState }) => (
                <Field data-invalid={fieldState.invalid}>
                    {label && <FieldLabel htmlFor={name}>{label}</FieldLabel>}
                    <InputOTP
                        maxLength={6}
                        pattern={REGEXP_ONLY_DIGITS}
                        aria-invalid={fieldState.invalid}
                        {...props}
                        {...field}
                    >
                        <InputOTPGroup>
                            <InputOTPSlot index={0} />
                            <InputOTPSlot index={1} />
                            <InputOTPSlot index={2} />
                        </InputOTPGroup>
                        <InputOTPGroup>
                            <InputOTPSlot index={3} />
                            <InputOTPSlot index={4} />
                            <InputOTPSlot index={5} />
                        </InputOTPGroup>
                    </InputOTP>
                    {fieldState.invalid && (
                        <FieldError errors={[fieldState.error]} />
                    )}
                </Field>
            )}
        />
    );
}
