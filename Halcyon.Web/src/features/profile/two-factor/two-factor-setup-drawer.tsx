import { useEffect, useMemo, useState } from 'react';
import {
    Drawer,
    DrawerContent,
    DrawerDescription,
    DrawerFooter,
    DrawerHeader,
    DrawerTitle,
} from '@/components/ui/drawer';
import { Button } from '@/components/ui/button';
import { Input } from '@/components/ui/input';
import {
    Field,
    FieldContent,
    FieldDescription,
    FieldLabel,
} from '@/components/ui/field';
import { useSetupTwoFactor } from '@/features/profile/hooks/use-setup-two-factor';
import { useVerifyTwoFactor } from '@/features/profile/hooks/use-verify-two-factor';
import QRCode from 'qrcode';
import { toast } from 'sonner';

export type TwoFactorSetupDrawerProps = {
    open: boolean;
    onOpenChange: (open: boolean) => void;
};

export function TwoFactorSetupDrawer({
    open,
    onOpenChange,
}: TwoFactorSetupDrawerProps) {
    const setup = useSetupTwoFactor();
    const verify = useVerifyTwoFactor();

    const [secret, setSecret] = useState<string | null>(null);
    const [qrDataUrl, setQrDataUrl] = useState<string | null>(null);
    const [code, setCode] = useState('');
    const [showRecovery, setShowRecovery] = useState<string[] | null>(null);
    const [error, setError] = useState<string | null>(null);

    useEffect(() => {
        if (!open) return;

        // Fetch setup details on open
        setup.mutate(undefined, {
            onSuccess: async (data) => {
                // initialize fresh state from server
                setCode('');
                setShowRecovery(null);
                setError(null);
                setSecret(data.secret);
                try {
                    const url = await QRCode.toDataURL(data.otpAuthUri, {
                        margin: 1,
                        width: 256,
                    });
                    setQrDataUrl(url);
                } catch {
                    setQrDataUrl(null);
                }
            },
            onError: (err: unknown) =>
                setError(
                    (err as Error)?.message ?? 'Failed to start 2FA setup'
                ),
        });
    }, [open, setup]);

    const canVerify = useMemo(() => code.trim().length === 6, [code]);

    return (
        <Drawer open={open} onOpenChange={onOpenChange} direction="bottom">
            <DrawerContent>
                <DrawerHeader>
                    <DrawerTitle>Set up two-factor authentication</DrawerTitle>
                    <DrawerDescription>
                        Scan the QR code with your authenticator app, then enter
                        the 6-digit code to verify.
                    </DrawerDescription>
                </DrawerHeader>
                <div className="space-y-4 p-4">
                    {error && <p className="text-sm text-red-600">{error}</p>}

                    {showRecovery ? (
                        <div className="space-y-3">
                            <h3 className="font-semibold">Recovery codes</h3>
                            <p className="text-muted-foreground text-sm">
                                Save these codes in a secure place. They can be
                                used if you lose access to your authenticator.
                            </p>
                            <div className="grid grid-cols-2 gap-2">
                                {showRecovery.map((rc) => (
                                    <code
                                        key={rc}
                                        className="bg-muted rounded px-2 py-1 text-sm"
                                    >
                                        {rc}
                                    </code>
                                ))}
                            </div>
                        </div>
                    ) : (
                        <div className="grid gap-4 sm:grid-cols-[auto_1fr]">
                            <div className="flex items-center justify-center">
                                {qrDataUrl ? (
                                    <img
                                        src={qrDataUrl}
                                        alt="2FA QR Code"
                                        className="h-48 w-48 rounded border"
                                    />
                                ) : (
                                    <div className="text-muted-foreground grid h-48 w-48 place-items-center rounded border text-sm">
                                        QR unavailable
                                    </div>
                                )}
                            </div>
                            <div className="space-y-4">
                                <Field>
                                    <FieldContent>
                                        <FieldLabel>Secret</FieldLabel>
                                        <FieldDescription>
                                            Use this secret if you can’t scan
                                            the QR code.
                                        </FieldDescription>
                                    </FieldContent>
                                    <div className="rounded border p-2 text-sm break-all select-all">
                                        {secret ?? '—'}
                                    </div>
                                </Field>

                                <Field>
                                    <FieldContent>
                                        <FieldLabel>
                                            Verification code
                                        </FieldLabel>
                                        <FieldDescription>
                                            Enter the 6-digit code from your
                                            app.
                                        </FieldDescription>
                                    </FieldContent>
                                    <Input
                                        inputMode="numeric"
                                        pattern="[0-9]*"
                                        maxLength={6}
                                        value={code}
                                        onChange={(e) =>
                                            setCode(
                                                e.currentTarget.value.replace(
                                                    /\D/g,
                                                    ''
                                                )
                                            )
                                        }
                                    />
                                </Field>
                            </div>
                        </div>
                    )}
                </div>
                <DrawerFooter>
                    {showRecovery ? (
                        <div className="flex gap-2">
                            <Button
                                variant="outline"
                                onClick={() => {
                                    if (!showRecovery) return;
                                    const content = showRecovery.join('\n');
                                    const blob = new Blob([content], {
                                        type: 'text/plain;charset=utf-8',
                                    });
                                    const url = URL.createObjectURL(blob);
                                    const a = document.createElement('a');
                                    a.href = url;
                                    a.download = 'recovery-codes.txt';
                                    document.body.appendChild(a);
                                    a.click();
                                    a.remove();
                                    URL.revokeObjectURL(url);
                                    toast.success('Downloaded recovery codes');
                                }}
                            >
                                Download codes
                            </Button>
                            <Button
                                variant="outline"
                                onClick={async () => {
                                    try {
                                        await navigator.clipboard.writeText(
                                            (showRecovery ?? []).join('\n')
                                        );
                                        toast.success('Copied recovery codes');
                                    } catch {
                                        toast.error('Copy failed');
                                    }
                                }}
                            >
                                Copy codes
                            </Button>
                            <Button onClick={() => onOpenChange(false)}>
                                Done
                            </Button>
                        </div>
                    ) : (
                        <div className="flex gap-2">
                            <Button
                                disabled={!canVerify || verify.isPending}
                                onClick={() => {
                                    setError(null);
                                    verify.mutate(
                                        { code: code.trim() },
                                        {
                                            onSuccess: (data) => {
                                                setShowRecovery(
                                                    data.recoveryCodes
                                                );
                                                toast.success(
                                                    'Two-factor enabled'
                                                );
                                            },
                                            onError: (err: unknown) =>
                                                setError(
                                                    (err as Error)?.message ??
                                                        'Failed to verify code'
                                                ),
                                        }
                                    );
                                }}
                            >
                                Verify & Enable
                            </Button>
                            <Button
                                variant="outline"
                                onClick={() => onOpenChange(false)}
                            >
                                Cancel
                            </Button>
                        </div>
                    )}
                </DrawerFooter>
            </DrawerContent>
        </Drawer>
    );
}
