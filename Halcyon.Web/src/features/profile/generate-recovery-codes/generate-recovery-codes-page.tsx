import { AlertCircleIcon } from 'lucide-react';
import { Alert, AlertTitle } from '@/components/ui/alert';
import { Metadata } from '@/components/metadata';
import { QueryError } from '@/components/query-error';
import { GenerateRecoveryCodesLoading } from '@/features/profile/generate-recovery-codes/generate-recovery-codes-loading';
import { useGenerateRecoveryCodes } from '@/features/profile/hooks/use-generate-recovery-codes';

export function GenerateRecoveryCodesPage() {
    const {
        data: generateRecoveryCodes,
        isPending,
        // isFetching,
        isSuccess,
        error,
    } = useGenerateRecoveryCodes({
        // version: profile?.version,
    });

    if (isPending) {
        return <GenerateRecoveryCodesLoading />;
    }

    if (!isSuccess) {
        return <QueryError error={error} />;
    }

    return (
        <main className="mx-auto max-w-screen-sm space-y-6 p-6">
            <Metadata title="Recovery Codes" />

            <h1 className="scroll-m-20 text-4xl font-extrabold tracking-tight text-balance">
                Recovery Codes
            </h1>

            <Alert>
                <AlertCircleIcon />
                <AlertTitle>Put these codes in a safe place.</AlertTitle>
            </Alert>

            <p className="leading-7">
                If you lose your device and don't have the recovery codes you
                will lose access to your account.
            </p>

            <div className="flex flex-col gap-2 rounded border p-4">
                {generateRecoveryCodes.recoveryCodes.map((code) => (
                    <code
                        key={code}
                        className="bg-muted rounded p-1 font-mono font-semibold"
                    >
                        {code}
                    </code>
                ))}
            </div>
        </main>
    );
}
