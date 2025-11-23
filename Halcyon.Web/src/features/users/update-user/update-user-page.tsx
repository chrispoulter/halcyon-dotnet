import { Link, useNavigate, useParams } from 'react-router';
import { toast } from 'sonner';
import { Button } from '@/components/ui/button';
import { Metadata } from '@/components/metadata';
import { QueryError } from '@/components/query-error';
import { useGetUser } from '@/features/users/hooks/use-get-user';
import { useUpdateUser } from '@/features/users/hooks/use-update-user';
import {
    UpdateUserForm,
    type UpdateUserFormValues,
} from '@/features/users/update-user/update-user-form';
import { DeleteUserButton } from '@/features/users/update-user/delete-user-button';
import { LockUserButton } from '@/features/users/update-user/lock-user-button';
import { UnlockUserButton } from '@/features/users/update-user/unlock-user-button';
import { UpdateUserLoading } from '@/features/users/update-user/update-user-loading';

type UpdateUserPageParams = { id: string };

export function UpdateUserPage() {
    const { id } = useParams() as UpdateUserPageParams;

    const navigate = useNavigate();

    const {
        data: user,
        isPending,
        isFetching,
        isSuccess,
        error,
    } = useGetUser(id);

    const { mutate: updateUser, isPending: isSaving } = useUpdateUser(id);

    if (isPending) {
        return <UpdateUserLoading />;
    }

    if (!isSuccess) {
        return <QueryError error={error} />;
    }

    function onSubmit(values: UpdateUserFormValues) {
        updateUser(
            {
                ...values,
                version: user?.version,
            },
            {
                onSuccess: () => {
                    toast.success('User successfully updated.');
                    navigate('/users');
                },
                onError: (error) => toast.error(error.message),
            }
        );
    }

    return (
        <main className="mx-auto max-w-screen-sm space-y-6 p-6">
            <Metadata title={`${user.firstName} ${user.lastName}`} />

            <h1 className="scroll-m-20 text-4xl font-extrabold tracking-tight text-balance">
                User
            </h1>
            <h2 className="scroll-m-20 border-b pb-2 text-3xl font-semibold tracking-tight">
                Update
            </h2>

            <p className="leading-7">
                Update the user&apos;s details below. The email address is used
                to login to the account.
            </p>

            <UpdateUserForm
                user={user}
                onSubmit={onSubmit}
                disabled={isFetching}
                loading={isSaving}
            >
                <Button asChild variant="outline">
                    <Link to="/users">Cancel</Link>
                </Button>

                {user.isLockedOut ? (
                    <UnlockUserButton user={user} disabled={isFetching} />
                ) : (
                    <LockUserButton user={user} disabled={isFetching} />
                )}

                <DeleteUserButton user={user} disabled={isFetching} />
            </UpdateUserForm>
        </main>
    );
}
