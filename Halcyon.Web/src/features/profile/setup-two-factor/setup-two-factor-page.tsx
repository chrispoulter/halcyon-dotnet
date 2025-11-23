import { Link, useNavigate } from 'react-router';
import { toast } from 'sonner';
import { QRCodeSVG } from 'qrcode.react';
import { Button } from '@/components/ui/button';
import { Metadata } from '@/components/metadata';
import { QueryError } from '@/components/query-error';
import {
    SetupTwoFactorForm,
    type SetupTwoFactorFormValues,
} from '@/features/profile/setup-two-factor/setup-two-factor-form';
import { SetupTwoFactorLoading } from '@/features/profile/setup-two-factor/setup-two-factor-loading';
import { useSetupTwoFactor } from '@/features/profile/hooks/use-setup-two-factor';
import { useVerifyTwoFactor } from '@/features/profile/hooks/use-verify-two-factor';

export function SetupTwoFactorPage() {
    const navigate = useNavigate();

    const {
        data: setupTwoFactor,
        isPending,
        isFetching,
        isSuccess,
        error,
    } = useSetupTwoFactor({
        // version: profile?.version,
    });

    const { mutate: verifyTwoFactor, isPending: isSaving } =
        useVerifyTwoFactor();

    if (isPending) {
        return <SetupTwoFactorLoading />;
    }

    if (!isSuccess) {
        return <QueryError error={error} />;
    }

    function onSubmit(data: SetupTwoFactorFormValues) {
        verifyTwoFactor(
            {
                ...data,
                // version: profile?.version,
            },
            {
                onSuccess: () => {
                    toast.success(
                        'Two-factor authentication has been configured.'
                    );
                    navigate('/profile');
                },
                onError: (error) => toast.error(error.message),
            }
        );
    }

    return (
        <main className="mx-auto max-w-screen-sm space-y-6 p-6">
            <Metadata title="Two-Factor Authentication" />

            <h1 className="scroll-m-20 text-4xl font-extrabold tracking-tight text-balance">
                Two-Factor Authentication
            </h1>

            <p className="leading-7">
                To use an authenticator app go through the following steps:
            </p>

            <ol className="ml-6 list-decimal space-y-2">
                <li>
                    Download a two-factor authenticator app like Microsoft
                    Authenticator for{' '}
                    <a
                        href="https://go.microsoft.com/fwlink/?Linkid=825072"
                        className="underline underline-offset-4"
                    >
                        Android
                    </a>{' '}
                    and{' '}
                    <a
                        href="https://go.microsoft.com/fwlink/?Linkid=825073"
                        className="underline underline-offset-4"
                    >
                        iOS
                    </a>{' '}
                    or Google Authenticator for{' '}
                    <a
                        href="https://play.google.com/store/apps/details?id=com.google.android.apps.authenticator2&amp;hl=en"
                        className="underline underline-offset-4"
                    >
                        Android
                    </a>{' '}
                    and{' '}
                    <a
                        href="https://itunes.apple.com/us/app/google-authenticator/id388497605?mt=8"
                        className="underline underline-offset-4"
                    >
                        iOS
                    </a>
                    .
                </li>
                <li>
                    Scan the QR Code or enter this key{' '}
                    <kbd className="bg-muted rounded p-1 font-mono font-semibold">
                        {setupTwoFactor.secret}
                    </kbd>{' '}
                    into your two factor authenticator app. Spaces and casing do
                    not matter.
                    <QRCodeSVG
                        value={setupTwoFactor.otpAuthUri}
                        className="mt-2 rounded border bg-white p-1"
                    />
                </li>
                <li>
                    Once you have scanned the QR code or input the key above,
                    your two factor authentication app will provide you with a
                    unique code. Enter the code in the confirmation box below.
                </li>
            </ol>

            <SetupTwoFactorForm
                onSubmit={onSubmit}
                loading={isSaving}
                disabled={isFetching || isSaving}
            >
                <Button asChild variant="outline">
                    <Link to="/profile">Cancel</Link>
                </Button>
            </SetupTwoFactorForm>
        </main>
    );
}
