import { useForm } from 'react-hook-form';
import { zodResolver } from '@hookform/resolvers/zod';
import { z } from 'zod';
import { TextField } from '@/components/form/text-field';
import { LoadingButton } from '@/components/loading-button';

const schema = z
    .object({
        emailAddress: z.email('Email Address must be a valid email'),
        newPassword: z
            .string({ message: 'New Password must be a valid string' })
            .min(8, 'New Password must be at least 8 characters')
            .max(50, 'New Password must be no more than 50 characters'),
        confirmNewPassword: z
            .string({ message: 'Confirm New Password must be a valid string' })
            .min(1, 'Confirm New Password is a required field'),
    })
    .refine((data) => data.newPassword === data.confirmNewPassword, {
        message: 'Passwords do not match',
        path: ['confirmNewPassword'],
    });

export type ResetPasswordFormValues = z.infer<typeof schema>;

type ResetPasswordFormProps = {
    loading?: boolean;
    onSubmit: (values: ResetPasswordFormValues) => void;
};

export function ResetPasswordForm({
    loading,
    onSubmit,
}: ResetPasswordFormProps) {
    const form = useForm<ResetPasswordFormValues>({
        resolver: zodResolver(schema),
        defaultValues: {
            emailAddress: '',
            newPassword: '',
            confirmNewPassword: '',
        },
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
                disabled={loading}
            />

            <div className="flex flex-col gap-6 sm:flex-row">
                <TextField
                    control={form.control}
                    name="newPassword"
                    label="New Password"
                    type="password"
                    maxLength={50}
                    autoComplete="new-password"
                    required
                    disabled={loading}
                />

                <TextField
                    control={form.control}
                    name="confirmNewPassword"
                    label="Confirm New Password"
                    type="password"
                    maxLength={50}
                    autoComplete="new-password"
                    required
                    disabled={loading}
                />
            </div>

            <div className="flex flex-col-reverse justify-end gap-2 sm:flex-row">
                <LoadingButton type="submit" loading={loading}>
                    Submit
                </LoadingButton>
            </div>
        </form>
    );
}
