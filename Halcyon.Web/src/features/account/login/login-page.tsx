import { useState } from 'react';
import { Link, useNavigate } from 'react-router';
import { toast } from 'sonner';
import { Button } from '@/components/ui/button';
import { useAuth } from '@/components/auth-provider';
import { Metadata } from '@/components/metadata';
import { useLogin } from '@/features/account/hooks/use-login';
import { useLoginWithTwoFactor } from '@/features/account/hooks/use-login-with-two-factor';
import { useLoginWithRecoveryCode } from '@/features/account/hooks/use-login-with-recovery-code';
import {
    LoginForm,
    type LoginFormValues,
} from '@/features/account/login/login-form';
import {
    LoginWithTwoFactorForm,
    type LoginWithTwoFactorFormValues,
} from '@/features/account/login/login-with-two-factor-form';
import {
    LoginWithRecoveryCodeForm,
    type LoginWithRecoveryCodeFormValues,
} from '@/features/account/login/login-with-recovery-code-form';

type FormStage = 'Login' | 'TwoFactor' | 'RecoveryCode';

type State = { stage: FormStage; loginFormValues?: LoginFormValues };

export function LoginPage() {
    const navigate = useNavigate();

    const { setAuth } = useAuth();

    const [state, setState] = useState<State>({
        stage: 'Login',
    });

    const { mutate: login, isPending: isLoginSaving } = useLogin();

    const { mutate: loginWithTwoFactor, isPending: isTwoFactorSaving } =
        useLoginWithTwoFactor();

    const { mutate: loginWithRecoveryCode, isPending: isRecoveryCodeSaving } =
        useLoginWithRecoveryCode();

    function onLoginSubmit(values: LoginFormValues) {
        login(values, {
            onSuccess: (data) => {
                if (data.requiresTwoFactor) {
                    setState({
                        stage: 'TwoFactor',
                        loginFormValues: values,
                    });
                } else if (data.accessToken) {
                    setAuth(data.accessToken);
                    navigate('/');
                }
            },
            onError: (error) => toast.error(error.message),
        });
    }

    function onTwoFactorSubmit(data: LoginWithTwoFactorFormValues) {
        loginWithTwoFactor(
            {
                ...state.loginFormValues!,
                twoFactorCode: data.twoFactorCode,
            },
            {
                onSuccess: (data) => {
                    setAuth(data.accessToken);
                    navigate('/');
                },
                onError: (error) => toast.error(error.message),
            }
        );
    }

    function onRecoveryCodeSubmit(data: LoginWithRecoveryCodeFormValues) {
        loginWithRecoveryCode(
            {
                ...state.loginFormValues!,
                recoveryCode: data.recoveryCode,
            },
            {
                onSuccess: (data) => {
                    setAuth(data.accessToken);
                    navigate('/');
                },
                onError: (error) => toast.error(error.message),
            }
        );
    }

    switch (state.stage) {
        case 'TwoFactor':
            return (
                <main className="mx-auto max-w-screen-sm space-y-6 p-6">
                    <Metadata title="Two-Factor Authentication" />

                    <h1 className="scroll-m-20 text-4xl font-extrabold tracking-tight text-balance">
                        Two-Factor Authentication
                    </h1>

                    <p className="leading-7">
                        Your login is protected with an authenticator app. Enter
                        your authenticator code below.
                    </p>

                    <LoginWithTwoFactorForm
                        loading={isTwoFactorSaving}
                        onSubmit={onTwoFactorSubmit}
                    >
                        <Button
                            variant="outline"
                            onClick={() => setState({ stage: 'Login' })}
                        >
                            Cancel
                        </Button>
                    </LoginWithTwoFactorForm>

                    <div className="space-y-2">
                        <p className="text-muted-foreground text-sm">
                            Don&apos;t have access to your authenticator device?
                            You can{' '}
                            <Button
                                variant="link"
                                onClick={() =>
                                    setState((prev) => ({
                                        ...prev,
                                        stage: 'RecoveryCode',
                                    }))
                                }
                                className="underline underline-offset-4"
                            >
                                login with a recovery code
                            </Button>
                            .
                        </p>
                    </div>
                </main>
            );

        case 'RecoveryCode':
            return (
                <main className="mx-auto max-w-screen-sm space-y-6 p-6">
                    <Metadata title="Recovery Code Verification" />

                    <h1 className="scroll-m-20 text-4xl font-extrabold tracking-tight text-balance">
                        Recovery Code Verification
                    </h1>

                    <p className="leading-7">
                        You have requested to login using a recovery code.
                        Please enter one of your recovery codes below.
                    </p>

                    <LoginWithRecoveryCodeForm
                        loading={isRecoveryCodeSaving}
                        onSubmit={onRecoveryCodeSubmit}
                    >
                        <Button
                            variant="outline"
                            onClick={() =>
                                setState((prev) => ({
                                    ...prev,
                                    stage: 'TwoFactor',
                                }))
                            }
                        >
                            Cancel
                        </Button>
                    </LoginWithRecoveryCodeForm>
                </main>
            );

        default:
            return (
                <main className="mx-auto max-w-screen-sm space-y-6 p-6">
                    <Metadata title="Login" />

                    <h1 className="scroll-m-20 text-4xl font-extrabold tracking-tight text-balance">
                        Login
                    </h1>

                    <p className="leading-7">
                        Enter your email address below to login to your account.
                    </p>

                    <LoginForm
                        loading={isLoginSaving}
                        onSubmit={onLoginSubmit}
                    />

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
}
