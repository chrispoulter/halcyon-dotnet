import { useMutation, useQueryClient } from '@tanstack/react-query';
import { useAuth } from '@/components/auth-provider';
import { apiClient } from '@/lib/api-client';

type LockUserRequest = { version?: number };

type LockUserResponse = {
    id: string;
};

export const useLockUser = (id: string) => {
    const { accessToken } = useAuth();

    const queryClient = useQueryClient();

    return useMutation({
        mutationFn: (request: LockUserRequest) =>
            apiClient.put<LockUserResponse>(`/users/${id}/lock`, request, {
                Authorization: `Bearer ${accessToken}`,
            }),
        onSuccess: (data) => {
            queryClient.invalidateQueries({ queryKey: ['profile'] });
            queryClient.invalidateQueries({ queryKey: ['users'] });
            queryClient.invalidateQueries({ queryKey: ['user', data.id] });
        },
    });
};
