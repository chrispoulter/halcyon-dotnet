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
import { useLockUser } from '@/features/users/hooks/use-lock-user';

type LockUserButtonProps = {
    user: GetUserResponse;
    disabled?: boolean;
    className?: string;
};

export function LockUserButton({
    user,
    disabled,
    className,
}: LockUserButtonProps) {
    const { mutate: lockUser, isPending: isLocking } = useLockUser(user.id);

    function onLock() {
        lockUser(
            {
                version: user.version,
            },
            {
                onSuccess: () => toast.success('User successfully locked.'),
                onError: (error) => toast.error(error.message),
            }
        );
    }

    return (
        <AlertDialog>
            <AlertDialogTrigger asChild>
                <LoadingButton
                    variant="secondary"
                    loading={isLocking}
                    disabled={disabled}
                    className={className}
                >
                    Lock
                </LoadingButton>
            </AlertDialogTrigger>
            <AlertDialogContent>
                <AlertDialogHeader>
                    <AlertDialogTitle>Lock User</AlertDialogTitle>
                    <AlertDialogDescription>
                        Are you sure you want to lock this user account? The
                        user will no longer be able to access the system.
                    </AlertDialogDescription>
                </AlertDialogHeader>
                <AlertDialogFooter>
                    <AlertDialogCancel>Cancel</AlertDialogCancel>
                    <AlertDialogAction
                        disabled={disabled || isLocking}
                        onClick={onLock}
                    >
                        Continue
                    </AlertDialogAction>
                </AlertDialogFooter>
            </AlertDialogContent>
        </AlertDialog>
    );
}
