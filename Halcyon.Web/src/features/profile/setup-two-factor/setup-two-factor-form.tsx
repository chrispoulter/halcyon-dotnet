import { useForm } from 'react-hook-form';
import { zodResolver } from '@hookform/resolvers/zod';
import { z } from 'zod';
import { TextField } from '@/components/form/text-field';
import { LoadingButton } from '@/components/loading-button';

const schema = z.object({
    code: z
        .string({ message: 'Authenticator code must be a valid string' })
        .min(1, 'Authenticator code is a required field'),
});

export type SetupTwoFactorFormValues = z.infer<typeof schema>;

type SetupTwoFactorFormProps = {
    onSubmit: (data: SetupTwoFactorFormValues) => void;
    loading?: boolean;
    disabled?: boolean;
    children?: React.ReactNode;
};

export function SetupTwoFactorForm({
    onSubmit,
    loading,
    disabled,
    children,
}: SetupTwoFactorFormProps) {
    const form = useForm<SetupTwoFactorFormValues>({
        resolver: zodResolver(schema),
        defaultValues: {
            code: '',
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
                name="code"
                label="Authenticator Code"
                type="text"
                maxLength={6}
                pattern="\d{6}"
                inputMode="numeric"
                autoComplete="one-time-code"
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
