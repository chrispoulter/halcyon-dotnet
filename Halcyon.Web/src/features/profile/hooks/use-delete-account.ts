import { useMutation, useQueryClient } from '@tanstack/react-query';
import { useAuth } from '@/components/auth-provider';
import { apiClient } from '@/lib/api-client';

type DeleteAccountRequest = { version?: number };

type DeleteAccountResponse = { id: string };

export const useDeleteAccount = () => {
    const { accessToken } = useAuth();

    const queryClient = useQueryClient();

    return useMutation({
        mutationFn: (request: DeleteAccountRequest) =>
            apiClient.delete<DeleteAccountResponse>('/profile', request, {
                Authorization: `Bearer ${accessToken}`,
            }),
        onSuccess: (response) => {
            queryClient.invalidateQueries({ queryKey: ['users'] });

            queryClient.invalidateQueries({
                queryKey: ['profile'],
                refetchType: 'none',
            });

            queryClient.invalidateQueries({
                queryKey: ['user', response.id],
                refetchType: 'none',
            });
        },
    });
};
