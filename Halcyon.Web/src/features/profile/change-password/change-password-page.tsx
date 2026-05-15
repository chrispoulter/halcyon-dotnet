import { Link, useNavigate } from 'react-router';
import { toast } from 'sonner';
import { Button } from '@/components/ui/button';
import { Metadata } from '@/components/metadata';
import { useChangePassword } from '../hooks/use-change-password';
import {
    ChangePasswordForm,
    type ChangePasswordFormValues,
} from './change-password-form';

export function ChangePasswordPage() {
    const navigate = useNavigate();

    const { mutate: changePassword, isPending: isSaving } = useChangePassword();

    function onSubmit(values: ChangePasswordFormValues) {
        changePassword(values, {
            onSuccess: () => {
                toast.success('Your password has been changed.');
                navigate('/profile');
            },
            onError: (error) => toast.error(error.message),
        });
    }

    return (
        <main className="mx-auto max-w-screen-sm space-y-6 p-6">
            <Metadata title="Change Password" />

            <h1 className="scroll-m-20 text-4xl font-extrabold tracking-tight text-balance">
                Change Password
            </h1>

            <p className="leading-7">
                Change your password below. Choose a strong password and
                don&apos;t reuse it for other accounts. For security reasons,
                change your password on a regular basis.
            </p>

            <ChangePasswordForm
                onSubmit={onSubmit}
                loading={isSaving}
                disabled={isSaving}
            >
                <Button asChild variant="outline">
                    <Link to="/profile">Cancel</Link>
                </Button>
            </ChangePasswordForm>

            <p className="text-sm text-muted-foreground">
                Forgotten your password?{' '}
                <Link
                    to="/account/forgot-password"
                    className="underline underline-offset-4"
                >
                    Request reset
                </Link>
            </p>
        </main>
    );
}
