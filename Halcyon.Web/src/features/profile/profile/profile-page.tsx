import { Link } from 'react-router';
import { Button } from '@/components/ui/button';
import { Metadata } from '@/components/metadata';
import { QueryError } from '@/components/query-error';
import { useGetProfile } from '@/features/profile/hooks/use-get-profile';
import { GenerateRecoveryCodesButton } from '@/features/profile/profile/generate-recovery-codes-button';
import { DisableTwoFactorButton } from '@/features/profile/profile/disable-two-factor-button';
import { DeleteAccountButton } from '@/features/profile/profile/delete-account-button';
import { ProfileLoading } from '@/features/profile/profile/profile-loading';
import { toDisplay } from '@/lib/dates';

export function ProfilePage() {
    const {
        data: profile,
        isPending,
        isFetching,
        isSuccess,
        error,
    } = useGetProfile();

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
                Two-Factor Authentication
            </h2>

            <p className="leading-7">
                Enhance the security of your account by enabling two-factor
                authentication.
            </p>

            <div className="flex flex-col gap-2 sm:flex-row">
                <Button asChild>
                    <Link to="/profile/enable-authenticator">
                        Configure Authenticator App
                    </Link>
                </Button>
                {profile.isTwoFactorEnabled && (
                    <>
                        <GenerateRecoveryCodesButton />
                        <DisableTwoFactorButton />
                    </>
                )}
            </div>

            <h2 className="scroll-m-20 border-b pb-2 text-3xl font-semibold tracking-tight">
                Settings
            </h2>

            <p className="leading-7">
                Once you delete your account all of your data and settings will
                be removed. Please be certain.
            </p>

            <DeleteAccountButton
                disabled={isFetching}
                className="w-full sm:w-auto"
            />
        </main>
    );
}
