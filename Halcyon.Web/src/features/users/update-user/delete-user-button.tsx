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
import { LoadingButton } from '@/components/loading-button';
import type { GetUserResponse } from '@/features/users/hooks/use-get-user';
import { useDeleteUser } from '@/features/users/hooks/use-delete-user';

type DeleteUserButtonProps = {
    user: GetUserResponse;
    disabled?: boolean;
    className?: string;
};

export function DeleteUserButton({
    user,
    disabled,
    className,
}: DeleteUserButtonProps) {
    const navigate = useNavigate();

    const { mutate: deleteUser, isPending: isDeleting } = useDeleteUser(
        user.id
    );

    function onDelete() {
        deleteUser(undefined, {
            onSuccess: () => {
                toast.success('User successfully deleted.');
                navigate('/users');
            },
            onError: (error) => toast.error(error.message),
        });
    }

    return (
        <AlertDialog>
            <AlertDialogTrigger asChild>
                <LoadingButton
                    variant="destructive"
                    loading={isDeleting}
                    disabled={disabled}
                    className={className}
                >
                    Delete
                </LoadingButton>
            </AlertDialogTrigger>
            <AlertDialogContent>
                <AlertDialogHeader>
                    <AlertDialogTitle>Delete User</AlertDialogTitle>
                    <AlertDialogDescription>
                        Are you sure you want to delete this user account? All
                        of the data will be permanently removed. This action
                        cannot be undone.
                    </AlertDialogDescription>
                </AlertDialogHeader>
                <AlertDialogFooter>
                    <AlertDialogCancel>Cancel</AlertDialogCancel>
                    <AlertDialogAction
                        disabled={disabled || isDeleting}
                        onClick={onDelete}
                    >
                        Continue
                    </AlertDialogAction>
                </AlertDialogFooter>
            </AlertDialogContent>
        </AlertDialog>
    );
}
