import { useMutation, useQueryClient } from '@tanstack/react-query';
import { useAuth } from '@/components/auth-provider';
import { apiClient } from '@/lib/api-client';
import type { Role } from '@/lib/session';

type CreateUserRequest = {
    emailAddress: string;
    password: string;
    firstName: string;
    lastName: string;
    dateOfBirth: string;
    roles?: Role[];
};

type CreateUserResponse = {
    userId: string;
};

export const useCreateUser = () => {
    const { accessToken } = useAuth();

    const queryClient = useQueryClient();

    return useMutation({
        mutationFn: (request: CreateUserRequest) =>
            apiClient.post<CreateUserResponse>('/users', request, {
                Authorization: `Bearer ${accessToken}`,
            }),
        onSuccess: () => queryClient.invalidateQueries({ queryKey: ['users'] }),
    });
};
