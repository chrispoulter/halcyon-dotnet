import { useMutation, useQueryClient } from '@tanstack/react-query';
import { useAuth } from '@/components/auth-provider';
import { apiClient } from '@/lib/api-client';

type DeleteUserRequest = { version?: number };

type DeleteUserResponse = {
    userId: string;
};

export const useDeleteUser = (id: string) => {
    const { accessToken } = useAuth();

    const queryClient = useQueryClient();

    return useMutation({
        mutationFn: (request: DeleteUserRequest) =>
            apiClient.delete<DeleteUserResponse>(`/users/${id}`, request, {
                Authorization: `Bearer ${accessToken}`,
            }),
        onSuccess: (data) => {
            queryClient.invalidateQueries({ queryKey: ['users'] });

            queryClient.invalidateQueries({
                queryKey: ['profile'],
                refetchType: 'none',
            });

            queryClient.invalidateQueries({
                queryKey: ['user', data.userId],
                refetchType: 'none',
            });
        },
    });
};
