import { useMutation, useQueryClient } from '@tanstack/react-query';
import { useAuth } from '@/components/auth-provider';
import { apiClient } from '@/lib/api-client';

type UnlockUserRequest = { version?: number };

type UnlockUserResponse = {
    id: string;
};

export const useUnlockUser = (id: string) => {
    const { accessToken } = useAuth();

    const queryClient = useQueryClient();

    return useMutation({
        mutationFn: (request: UnlockUserRequest) =>
            apiClient.put<UnlockUserResponse>(`/user/${id}/unlock`, request, {
                Authorization: `Bearer ${accessToken}`,
            }),
        onSuccess: (data) => {
            queryClient.invalidateQueries({ queryKey: ['profile'] });
            queryClient.invalidateQueries({ queryKey: ['users'] });
            queryClient.invalidateQueries({ queryKey: ['user', data.id] });
        },
    });
};
