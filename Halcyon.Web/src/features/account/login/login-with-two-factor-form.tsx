import { useForm } from 'react-hook-form';
import { zodResolver } from '@hookform/resolvers/zod';
import { z } from 'zod';
import { TextField } from '@/components/form/text-field';
import { LoadingButton } from '@/components/loading-button';

const schema = z.object({
    authenticatorCode: z
        .string({ message: 'Authenticator Code must be a valid string' })
        .regex(/^[0-9]{6}$/, 'Authenticator Code is not in the correct format'),
});

export type LoginWithTwoFactorFormValues = z.infer<typeof schema>;

type LoginWithTwoFactorFormProps = {
    loading?: boolean;
    onSubmit: (values: LoginWithTwoFactorFormValues) => void;
    children?: React.ReactNode;
};

export function LoginWithTwoFactorForm({
    loading,
    onSubmit,
    children,
}: LoginWithTwoFactorFormProps) {
    const form = useForm<LoginWithTwoFactorFormValues>({
        resolver: zodResolver(schema),
        defaultValues: {
            authenticatorCode: '',
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
                name="authenticatorCode"
                label="Authenticator Code"
                maxLength={6}
                inputMode="numeric"
                pattern="[0-9]{6}"
                autoComplete="one-time-code"
                required
                disabled={loading}
            />

            <div className="flex flex-col-reverse justify-end gap-2 sm:flex-row">
                {children}

                <LoadingButton type="submit" loading={loading}>
                    Submit
                </LoadingButton>
            </div>
        </form>
    );
}
