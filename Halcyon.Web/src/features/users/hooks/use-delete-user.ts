import { useMutation, useQueryClient } from '@tanstack/react-query';
import { useAuth } from '@/components/auth-provider';
import { apiClient } from '@/lib/api-client';

type DeleteUserResponse = {
    id: string;
};

export const useDeleteUser = (id: string) => {
    const { accessToken } = useAuth();

    const queryClient = useQueryClient();

    return useMutation({
        mutationFn: () =>
            apiClient
                .delete(`users/${id}`, {
                    context: {
                        accessToken,
                    },
                })
                .json<DeleteUserResponse>(),
        onSuccess: (data) => {
            queryClient.invalidateQueries({ queryKey: ['users'] });

            queryClient.invalidateQueries({
                queryKey: ['profile'],
                refetchType: 'none',
            });

            queryClient.invalidateQueries({
                queryKey: ['user', data.id],
                refetchType: 'none',
            });
        },
    });
};
