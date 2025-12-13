import { useState } from 'react';
import { Link, useNavigate } from 'react-router';
import { toast } from 'sonner';
import { QRCodeSVG } from 'qrcode.react';
import { Button } from '@/components/ui/button';
import { Metadata } from '@/components/metadata';
import { QueryError } from '@/components/query-error';
import { useSetupTwoFactor } from '@/features/profile/hooks/use-setup-two-factor';
import { useVerifyTwoFactor } from '@/features/profile/hooks/use-verify-two-factor';
import { RecoveryCodesDialog } from '@/features/profile/generate-recovery-codes/recovery-codes-dialog';
import { EnableAuthenticatorLoading } from '@/features/profile/enable-authenticator/enable-authenticator-loading';
import {
    type EnableAuthenticatorFormValues,
    EnableAuthenticatorForm,
} from '@/features/profile/enable-authenticator/enable-authenticator-form';

export function EnableAuthenticatorPage() {
    const navigate = useNavigate();

    const [recoveryCodes, setRecoveryCodes] = useState<string[] | undefined>();

    const [showRecoveryCodesDialog, setShowRecoveryCodesDialog] =
        useState(false);

    const {
        data: setupTwoFactor,
        isPending,
        isFetching,
        isSuccess,
        error,
    } = useSetupTwoFactor();

    const { mutate: verifyTwoFactor, isPending: isVerifying } =
        useVerifyTwoFactor();

    if (isPending) {
        return <EnableAuthenticatorLoading />;
    }

    if (!isSuccess) {
        return <QueryError error={error} />;
    }

    function onSubmit(values: EnableAuthenticatorFormValues) {
        verifyTwoFactor(values, {
            onSuccess: (data) => {
                toast.success('Two-factor authentication has been enabled.');
                setRecoveryCodes(data.recoveryCodes);
                setShowRecoveryCodesDialog(true);
            },
            onError: (error) => toast.error(error.message),
        });
    }

    function onRecoveryCodeDialogChange(open: boolean) {
        setShowRecoveryCodesDialog(open);

        if (!open) {
            navigate('/profile');
        }
    }

    return (
        <main className="mx-auto max-w-screen-sm space-y-6 p-6">
            <Metadata title="Configure Authenticator App" />

            <h1 className="scroll-m-20 text-4xl font-extrabold tracking-tight text-balance">
                Configure Authenticator App
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
                    <code className="bg-muted rounded px-2 py-1 font-mono font-semibold">
                        {setupTwoFactor.secret}
                    </code>{' '}
                    into your two-factor authenticator app.
                    <QRCodeSVG
                        value={setupTwoFactor.otpauth}
                        width={180}
                        height={180}
                        className="mt-2 rounded border bg-white p-1"
                    />
                </li>
                <li>
                    Once you have scanned the QR code or input the key above,
                    your two-factor authentication app will provide you with a
                    unique code. Enter the code in the confirmation box below.
                </li>
            </ol>

            <EnableAuthenticatorForm
                onSubmit={onSubmit}
                disabled={isFetching || isVerifying}
                loading={isVerifying}
            >
                <Button asChild variant="outline">
                    <Link to="/profile">Cancel</Link>
                </Button>
            </EnableAuthenticatorForm>

            <RecoveryCodesDialog
                open={showRecoveryCodesDialog}
                onOpenChange={onRecoveryCodeDialogChange}
                codes={recoveryCodes}
            />
        </main>
    );
}
