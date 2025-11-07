import { useForm } from 'react-hook-form';
import { zodResolver } from '@hookform/resolvers/zod';
import { z } from 'zod';
import { DateField } from '@/components/form/date-field';
import { TextField } from '@/components/form/text-field';
import { LoadingButton } from '@/components/loading-button';
import type { GetProfileResponse } from '@/features/profile/hooks/use-get-profile';
import { isInPast } from '@/lib/dates';

const schema = z.object({
    emailAddress: z.email('Email Address must be a valid email'),
    firstName: z
        .string({ message: 'First Name must be a valid string' })
        .min(1, 'First Name is a required field')
        .max(50, 'First Name must be no more than 50 characters'),
    lastName: z
        .string({ message: 'Last Name must be a valid string' })
        .min(1, 'Last Name is a required field')
        .max(50, 'Last Name must be no more than 50 characters'),
    dateOfBirth: z.iso
        .date('Date Of Birth must be a valid date')
        .refine(isInPast, { message: 'Date Of Birth must be in the past' }),
});

export type UpdateProfileFormValues = z.infer<typeof schema>;

type UpdateProfileFormProps = {
    profile: GetProfileResponse;
    onSubmit: (data: UpdateProfileFormValues) => void;
    loading?: boolean;
    disabled?: boolean;
    children?: React.ReactNode;
};

export function UpdateProfileForm({
    profile,
    onSubmit,
    loading,
    disabled,
    children,
}: UpdateProfileFormProps) {
    const form = useForm<UpdateProfileFormValues>({
        resolver: zodResolver(schema),
        values: profile,
    });

    return (
        <form
            noValidate
            onSubmit={form.handleSubmit(onSubmit)}
            className="space-y-6"
        >
            <TextField
                control={form.control}
                name="emailAddress"
                label="Email Address"
                type="email"
                maxLength={254}
                autoComplete="username"
                required
                disabled={disabled}
            />

            <div className="flex flex-col gap-6 sm:flex-row">
                <TextField
                    control={form.control}
                    name="firstName"
                    label="First Name"
                    maxLength={50}
                    autoComplete="given-name"
                    required
                    disabled={disabled}
                />

                <TextField
                    control={form.control}
                    name="lastName"
                    label="Last Name"
                    maxLength={50}
                    autoComplete="family-name"
                    required
                    disabled={disabled}
                />
            </div>

            <DateField
                control={form.control}
                name="dateOfBirth"
                label="Date Of Birth"
                required
                disabled={disabled}
            />

            <div className="flex flex-col-reverse justify-end gap-2 sm:flex-row">
                {children}

                <LoadingButton
                    type="submit"
                    loading={loading}
                    disabled={disabled}
                >
                    Submit
                </LoadingButton>
            </div>
        </form>
    );
}
