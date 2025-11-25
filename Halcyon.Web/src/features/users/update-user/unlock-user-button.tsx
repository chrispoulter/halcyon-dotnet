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
import type { GetUserResponse } from '@/features/users/hooks/use-get-user';
import { useUnlockUser } from '@/features/users/hooks/use-unlock-user';

type UnlockUserButtonProps = {
    user: GetUserResponse;
    disabled?: boolean;
    className?: string;
};

export function UnlockUserButton({
    user,
    disabled,
    className,
}: UnlockUserButtonProps) {
    const { mutate: unlockUser, isPending: isUnlocking } = useUnlockUser(
        user.id
    );

    function onUnlock() {
        unlockUser(undefined, {
            onSuccess: () => toast.success('User successfully unlocked.'),
            onError: (error) => toast.error(error.message),
        });
    }

    return (
        <AlertDialog>
            <AlertDialogTrigger asChild>
                <LoadingButton
                    variant="secondary"
                    loading={isUnlocking}
                    disabled={disabled}
                    className={className}
                >
                    Unlock
                </LoadingButton>
            </AlertDialogTrigger>
            <AlertDialogContent>
                <AlertDialogHeader>
                    <AlertDialogTitle>Unlock User</AlertDialogTitle>
                    <AlertDialogDescription>
                        Are you sure you want to unlock this user account? The
                        user will now be able to access the system.
                    </AlertDialogDescription>
                </AlertDialogHeader>
                <AlertDialogFooter>
                    <AlertDialogCancel>Cancel</AlertDialogCancel>
                    <AlertDialogAction
                        disabled={disabled || isUnlocking}
                        onClick={onUnlock}
                    >
                        Continue
                    </AlertDialogAction>
                </AlertDialogFooter>
            </AlertDialogContent>
        </AlertDialog>
    );
}
