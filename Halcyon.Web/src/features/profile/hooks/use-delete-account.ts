import { useMutation, useQueryClient } from '@tanstack/react-query';
import { useAuth } from '@/components/auth-provider';
import { apiClient } from '@/lib/api-client';

type DeleteAccountResponse = { id: string };

export const useDeleteAccount = () => {
    const { accessToken } = useAuth();

    const queryClient = useQueryClient();

    return useMutation({
        mutationFn: () =>
            apiClient.delete<DeleteAccountResponse>('/profile', undefined, {
                Authorization: `Bearer ${accessToken}`,
            }),
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
