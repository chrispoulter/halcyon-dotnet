import { Link, useNavigate } from 'react-router';
import { useState } from 'react';
import { toast } from 'sonner';
import { useAuth } from '@/components/auth-provider';
import { Metadata } from '@/components/metadata';
import { useLogin } from '@/features/account/hooks/use-login';
import { useLoginWithTwoFactor } from '@/features/account/hooks/use-login-with-two-factor';
import { useLoginWithRecoveryCode } from '@/features/account/hooks/use-login-with-recovery-code';
import { LoginForm } from '@/features/account/login/login-form';
import { Input } from '@/components/ui/input';
import { LoadingButton } from '@/components/loading-button';

export function LoginPage() {
    const navigate = useNavigate();

    const { setAuth } = useAuth();

    const { mutate: login, isPending: isSaving } = useLogin();

    const { mutate: login2fa, isPending: isVerifying } =
        useLoginWithTwoFactor();

    const { mutate: login2fa, isPending: isVerifying } =
        useLoginWithTwoFactor();

    const [twoFactorPending, setTwoFactorPending] = useState<{
        emailAddress: string;
        password: string;
    } | null>(null);
    const [code, setCode] = useState('');
    const [useRecovery, setUseRecovery] = useState(false);
    const [recoveryCode, setRecoveryCode] = useState('');

    // handled inline below

    function onSubmitTwoFactor(e: React.FormEvent) {
        e.preventDefault();
        const creds = twoFactorPending;
        if (!creds) return;
        const sanitized = code.trim();
        const recovery = recoveryCode.trim();
        if (!useRecovery) {
            if (sanitized.length !== 6) {
                toast.error('Enter the 6-digit code');
                return;
            }
        } else {
            if (recovery.length < 8) {
                toast.error('Enter a valid recovery code');
                return;
            }
        }
        login2fa(
            {
                emailAddress: creds.emailAddress,
                password: creds.password,
                code: useRecovery ? undefined : sanitized,
                recoveryCode: useRecovery ? recovery : undefined,
            },
            {
                onSuccess: (res) => {
                    toast.success('Signed in');
                    setAuth(res.accessToken);
                    navigate('/');
                },
                onError: (err) => toast.error(err.message),
            }
        );
    }

    return (
        <main className="mx-auto max-w-screen-sm space-y-6 p-6">
            <Metadata title="Login" />

            <h1 className="scroll-m-20 text-4xl font-extrabold tracking-tight text-balance">
                Login
            </h1>

            {twoFactorPending ? (
                <section className="space-y-4">
                    <p className="leading-7">
                        {useRecovery
                            ? 'Enter a recovery code to complete sign-in.'
                            : 'Enter the 6-digit code from your authenticator app to complete sign-in.'}
                    </p>
                    <form className="space-y-4" onSubmit={onSubmitTwoFactor}>
                        {useRecovery ? (
                            <div>
                                <label className="text-sm leading-none font-medium">
                                    Recovery code
                                </label>
                                <Input
                                    value={recoveryCode}
                                    onChange={(e) =>
                                        setRecoveryCode(
                                            e.currentTarget.value.trim()
                                        )
                                    }
                                    disabled={isVerifying}
                                />
                            </div>
                        ) : (
                            <div>
                                <label className="text-sm leading-none font-medium">
                                    Verification code
                                </label>
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
                                    disabled={isVerifying}
                                />
                            </div>
                        )}
                        <div className="flex gap-2">
                            <LoadingButton type="submit" loading={isVerifying}>
                                Verify
                            </LoadingButton>
                            <button
                                type="button"
                                className="text-sm underline underline-offset-4"
                                onClick={() => setTwoFactorPending(null)}
                                disabled={isVerifying}
                            >
                                Back to login
                            </button>
                            <button
                                type="button"
                                className="text-sm underline underline-offset-4"
                                onClick={() => {
                                    setUseRecovery((v) => !v);
                                    setCode('');
                                    setRecoveryCode('');
                                }}
                                disabled={isVerifying}
                            >
                                {useRecovery
                                    ? 'Use authenticator code'
                                    : 'Use recovery code'}
                            </button>
                        </div>
                    </form>
                </section>
            ) : (
                <>
                    <p className="leading-7">
                        Enter your email address below to login to your account.
                    </p>
                    <LoginForm
                        loading={isSaving}
                        onSubmit={(values) =>
                            login(values, {
                                onSuccess: (res) => {
                                    if (res.requiresTwoFactor) {
                                        setTwoFactorPending(values);
                                        setUseRecovery(false);
                                        setCode('');
                                        setRecoveryCode('');
                                        return;
                                    }
                                    if (res.accessToken) {
                                        toast.success('Signed in');
                                        setAuth(res.accessToken);
                                        navigate('/');
                                    } else {
                                        toast.error(
                                            'Login response missing token'
                                        );
                                    }
                                },
                                onError: (error) => toast.error(error.message),
                            })
                        }
                    />
                </>
            )}

            <div className="space-y-2">
                <p className="text-muted-foreground text-sm">
                    Not already a member?{' '}
                    <Link
                        to="/account/register"
                        className="underline underline-offset-4"
                    >
                        Register now
                    </Link>
                </p>
                <p className="text-muted-foreground text-sm">
                    Forgotten your password?{' '}
                    <Link
                        to="/account/forgot-password"
                        className="underline underline-offset-4"
                    >
                        Request reset
                    </Link>
                </p>
            </div>
        </main>
    );
}
