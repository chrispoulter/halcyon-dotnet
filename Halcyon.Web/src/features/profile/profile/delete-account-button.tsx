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
import { useDeleteAccount } from '@/features/profile/hooks/use-delete-account';
import type { GetProfileResponse } from '@/features/profile/profile-types';

type DeleteAccountButtonProps = {
    profile: GetProfileResponse;
    disabled?: boolean;
    className?: string;
};

export function DeleteAccountButton({
    profile,
    disabled,
    className,
}: DeleteAccountButtonProps) {
    const navigate = useNavigate();

    const { clearAuth } = useAuth();

    const { mutate: deleteAccount, isPending: isDeleting } = useDeleteAccount();

    function onDelete() {
        deleteAccount(
            {
                version: profile.version,
            },
            {
                onSuccess: () => {
                    toast.success('Your account has been deleted.');
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
                    loading={isDeleting}
                    disabled={disabled}
                    className={className}
                >
                    Delete Account
                </LoadingButton>
            </AlertDialogTrigger>
            <AlertDialogContent>
                <AlertDialogHeader>
                    <AlertDialogTitle>Delete Account</AlertDialogTitle>
                    <AlertDialogDescription>
                        Are you sure you want to delete your account? All of
                        your data will be permanently removed. This action
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
