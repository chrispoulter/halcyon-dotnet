import { useMutation, useQueryClient } from '@tanstack/react-query';
import { useAuth } from '@/components/auth-provider';
import { apiClient } from '@/lib/api-client';
import type { Role } from '@/lib/session';

type UpdateUserRequest = {
    emailAddress: string;
    firstName: string;
    lastName: string;
    dateOfBirth: string;
    roles?: Role[];
    version?: number;
};

type UpdateUserResponse = {
    id: string;
};

export const useUpdateUser = (id: string) => {
    const { accessToken } = useAuth();

    const queryClient = useQueryClient();

    return useMutation({
        mutationFn: (request: UpdateUserRequest) =>
            apiClient.put<UpdateUserResponse>(`/users/${id}`, request, {
                Authorization: `Bearer ${accessToken}`,
            }),
        onSuccess: (data) => {
            queryClient.invalidateQueries({ queryKey: ['profile'] });
            queryClient.invalidateQueries({ queryKey: ['users'] });
            queryClient.invalidateQueries({ queryKey: ['user', data.id] });
        },
    });
};
