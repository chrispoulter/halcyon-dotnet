import { useForm } from 'react-hook-form';
import { zodResolver } from '@hookform/resolvers/zod';
import { z } from 'zod';
import { TextField } from '@/components/form/text-field';
import { LoadingButton } from '@/components/loading-button';

const schema = z.object({
    code: z
        .string({ message: 'Recovery code must be a valid string' })
        .min(1, 'Recovery code is a required field'),
});

export type LoginWithTwoFactorFormValues = z.infer<typeof schema>;

type LoginWithTwoFactorFormProps = {
    loading?: boolean;
    onSubmit: (data: LoginWithTwoFactorFormValues) => void;
    children?: React.ReactNode;
};
    
export function LoginWithTwoFactorForm({
    loading,
    onSubmit,
    children
}: LoginWithTwoFactorFormProps) {
    const form = useForm<LoginWithTwoFactorFormValues>({
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
                maxLength={50}
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
