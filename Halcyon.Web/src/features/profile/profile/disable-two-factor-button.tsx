import { useNavigate } from 'react-router';
import { toast } from 'sonner';
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
import { useAuth } from '@/components/auth-provider';
import { LoadingButton } from '@/components/loading-button';
import type { GetProfileResponse } from '@/features/profile/hooks/use-get-profile';
import { useDisableTwoFactor } from '@/features/profile/hooks/use-disable-two-factor';

type DisableTwoFactorButtonProps = {
    profile: GetProfileResponse;
    disabled?: boolean;
    className?: string;
};

export function DisableTwoFactorButton({
    profile,
    disabled,
    className,
}: DisableTwoFactorButtonProps) {
    const navigate = useNavigate();

    const { clearAuth } = useAuth();

    const { mutate: disableTwoFactor, isPending: isDisabling } =
        useDisableTwoFactor();

    function onDisable() {
        disableTwoFactor(
            {
                version: profile.version,
            },
            {
                onSuccess: () => {
                    toast.success(
                        'Two-factor authentication has been disabled.'
                    );
                    clearAuth();
                    navigate('/');
                },
                onError: (error) => toast.error(error.message),
            }
        );
    }

    return (
        <AlertDialog>
            <AlertDialogTrigger asChild>
                <LoadingButton
                    variant="destructive"
                    loading={isDisabling}
                    disabled={disabled}
                    className={className}
                >
                    Disable Two-Factor
                </LoadingButton>
            </AlertDialogTrigger>
            <AlertDialogContent>
                <AlertDialogHeader>
                    <AlertDialogTitle>
                        Disable Two-Factor Authentication
                    </AlertDialogTitle>
                    <AlertDialogDescription>
                        Are you sure you want to disable two-factor
                        authentication?
                    </AlertDialogDescription>
                </AlertDialogHeader>
                <AlertDialogFooter>
                    <AlertDialogCancel>Cancel</AlertDialogCancel>
                    <AlertDialogAction
                        disabled={disabled || isDisabling}
                        onClick={onDisable}
                    >
                        Continue
                    </AlertDialogAction>
                </AlertDialogFooter>
            </AlertDialogContent>
        </AlertDialog>
    );
}
