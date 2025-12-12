import { useForm } from 'react-hook-form';
import { zodResolver } from '@hookform/resolvers/zod';
import { z } from 'zod';
import { TextField } from '@/components/form/text-field';
import { LoadingButton } from '@/components/loading-button';

const schema = z.object({
    recoveryCode: z
        .string({ message: 'Recovery Code must be a valid string' })
        .regex(/^[A-F0-9]{10}$/, 'Recovery Code is not in the correct format'),
});

export type LoginWithRecoveryCodeFormValues = z.infer<typeof schema>;

type LoginWithRecoveryCodeFormProps = {
    loading?: boolean;
    onSubmit: (values: LoginWithRecoveryCodeFormValues) => void;
    children?: React.ReactNode;
};

export function LoginWithRecoveryCodeForm({
    loading,
    onSubmit,
    children,
}: LoginWithRecoveryCodeFormProps) {
    const form = useForm<LoginWithRecoveryCodeFormValues>({
        resolver: zodResolver(schema),
        defaultValues: {
            recoveryCode: '',
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
                name="recoveryCode"
                label="Recovery Code"
                maxLength={10}
                pattern="[A-F][0-9]{10}"
                autoComplete="off"
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
