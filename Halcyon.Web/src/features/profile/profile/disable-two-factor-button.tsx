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
import { LoadingButton } from '@/components/loading-button';
import { useDisableTwoFactor } from '@/features/profile/hooks/use-disable-two-factor';

type DisableTwoFactorButtonProps = {
    disabled?: boolean;
    className?: string;
};

export function DisableTwoFactorButton({
    disabled,
    className,
}: DisableTwoFactorButtonProps) {
    const { mutate: disableTwoFactor, isPending: isDisabling } =
        useDisableTwoFactor();

    function onDisable() {
        disableTwoFactor(undefined, {
            onSuccess: () =>
                toast.success('Two-factor authentication has been disabled.'),
            onError: (error) => toast.error(error.message),
        });
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
