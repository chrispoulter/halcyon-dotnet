import { Link } from 'react-router';
import { useState } from 'react';
import { Button } from '@/components/ui/button';
import { Metadata } from '@/components/metadata';
import { QueryError } from '@/components/query-error';
import { useGetProfile } from '@/features/profile/hooks/use-get-profile';
import { DeleteAccountButton } from '@/features/profile/profile/delete-account-button';
import { ProfileLoading } from '@/features/profile/profile/profile-loading';
import { toDisplay } from '@/lib/dates';
import { TwoFactorSetupDrawer } from '@/features/profile/two-factor/two-factor-setup-drawer';
import {
    AlertDialog,
    AlertDialogAction,
    AlertDialogCancel,
    AlertDialogContent,
    AlertDialogDescription,
    AlertDialogFooter,
    AlertDialogHeader,
    AlertDialogTitle,
    AlertDialogTrigger,
} from '@/components/ui/alert-dialog';
import { useDisableTwoFactor } from '@/features/profile/hooks/use-disable-two-factor';
import { useRegenerateRecoveryCodes } from '@/features/profile/hooks/use-regenerate-recovery-codes';

export function ProfilePage() {
    const {
        data: profile,
        isPending,
        isFetching,
        isSuccess,
        error,
    } = useGetProfile();

    const [setupOpen, setSetupOpen] = useState(false);
    const disable2fa = useDisableTwoFactor();
    const regenerate = useRegenerateRecoveryCodes();
    const [recoveryOpen, setRecoveryOpen] = useState(false);
    const [recoveryCodes, setRecoveryCodes] = useState<string[] | null>(null);

    if (isPending) {
        return <ProfileLoading />;
    }

    if (!isSuccess) {
        return <QueryError error={error} />;
    }

    return (
        <main className="mx-auto max-w-screen-sm space-y-6 p-6">
            <Metadata title="My Account" />

            <h1 className="scroll-m-20 text-4xl font-extrabold tracking-tight text-balance">
                My Account
            </h1>

            <h2 className="scroll-m-20 border-b pb-2 text-3xl font-semibold tracking-tight">
                Personal Details
            </h2>

            <dl className="space-y-2">
                <dt className="text-sm leading-none font-medium">
                    Email Address
                </dt>
                <dd className="text-muted-foreground truncate text-sm">
                    {profile.emailAddress}
                </dd>
                <dt className="text-sm leading-none font-medium">Name</dt>
                <dd className="text-muted-foreground truncate text-sm">
                    {profile.firstName} {profile.lastName}
                </dd>
                <dt className="text-sm leading-none font-medium">
                    Date Of Birth
                </dt>
                <dd className="text-muted-foreground truncate text-sm">
                    {toDisplay(profile.dateOfBirth)}
                </dd>
            </dl>

            <Button asChild className="w-full sm:w-auto">
                <Link to="/profile/update-profile">Update Profile</Link>
            </Button>

            <h2 className="scroll-m-20 border-b pb-2 text-3xl font-semibold tracking-tight">
                Login Details
            </h2>

            <p className="leading-7">
                Choose a strong password and don&apos;t reuse it for other
                accounts. For security reasons, change your password on a
                regular basis.
            </p>

            <Button asChild className="w-full sm:w-auto">
                <Link to="/profile/change-password">Change Password</Link>
            </Button>

            <h2 className="scroll-m-20 border-b pb-2 text-3xl font-semibold tracking-tight">
                Settings
            </h2>

            <p className="leading-7">
                Once you delete your account all of your data and settings will
                be removed. Please be certain.
            </p>

            <DeleteAccountButton
                profile={profile}
                disabled={isFetching}
                className="w-full sm:w-auto"
            />

            <h2 className="scroll-m-20 border-b pb-2 text-3xl font-semibold tracking-tight">
                Two-Factor Authentication
            </h2>

            {profile.isTwoFactorEnabled ? (
                <div className="space-y-3">
                    <p className="leading-7">
                        Two-factor authentication is currently{' '}
                        <span className="font-semibold">enabled</span> on your
                        account.
                    </p>
                    <AlertDialog>
                        <AlertDialogTrigger asChild>
                            <Button
                                variant="outline"
                                className="w-full sm:w-auto"
                            >
                                Disable Two-Factor
                            </Button>
                        </AlertDialogTrigger>
                        <AlertDialogContent>
                            <AlertDialogHeader>
                                <AlertDialogTitle>
                                    Disable two-factor authentication?
                                </AlertDialogTitle>
                                <AlertDialogDescription>
                                    This will remove your authenticator
                                    configuration and recovery codes. You can
                                    re-enable 2FA at any time.
                                </AlertDialogDescription>
                            </AlertDialogHeader>
                            <AlertDialogFooter>
                                <AlertDialogCancel>Cancel</AlertDialogCancel>
                                <AlertDialogAction
                                    onClick={() => disable2fa.mutate()}
                                >
                                    Disable
                                </AlertDialogAction>
                            </AlertDialogFooter>
                        </AlertDialogContent>
                    </AlertDialog>
                    <Button
                        variant="outline"
                        className="w-full sm:w-auto"
                        onClick={() => {
                            regenerate.mutate(undefined, {
                                onSuccess: (data) => {
                                    setRecoveryCodes(data.recoveryCodes);
                                    setRecoveryOpen(true);
                                },
                            });
                        }}
                    >
                        Regenerate Recovery Codes
                    </Button>

                    <AlertDialog open={recoveryOpen} onOpenChange={setRecoveryOpen}>
                        <AlertDialogContent>
                            <AlertDialogHeader>
                                <AlertDialogTitle>New recovery codes</AlertDialogTitle>
                                <AlertDialogDescription>
                                    Save these codes now. They will not be shown again.
                                </AlertDialogDescription>
                            </AlertDialogHeader>
                            <div className="grid grid-cols-2 gap-2">
                                {(recoveryCodes ?? []).map((rc) => (
                                    <code key={rc} className="bg-muted rounded px-2 py-1 text-sm">{rc}</code>
                                ))}
                            </div>
                            <AlertDialogFooter>
                                <AlertDialogCancel>Close</AlertDialogCancel>
                                <AlertDialogAction
                                    onClick={() => {
                                        const list = recoveryCodes ?? [];
                                        const content = list.join('\n');
                                        const blob = new Blob([content], { type: 'text/plain;charset=utf-8' });
                                        const url = URL.createObjectURL(blob);
                                        const a = document.createElement('a');
                                        a.href = url;
                                        a.download = 'recovery-codes.txt';
                                        document.body.appendChild(a);
                                        a.click();
                                        a.remove();
                                        URL.revokeObjectURL(url);
                                    }}
                                >
                                    Download
                                </AlertDialogAction>
                            </AlertDialogFooter>
                        </AlertDialogContent>
                    </AlertDialog>
                </div>
            ) : (
                <div className="space-y-3">
                    <p className="leading-7">
                        Protect your account by adding a second step to sign in.
                    </p>
                    <Button
                        className="w-full sm:w-auto"
                        onClick={() => setSetupOpen(true)}
                    >
                        Set up Two-Factor
                    </Button>
                    <TwoFactorSetupDrawer
                        open={setupOpen}
                        onOpenChange={setSetupOpen}
                    />
                </div>
            )}
        </main>
    );
}
