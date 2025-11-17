import { useMutation, useQueryClient } from '@tanstack/react-query';
import { apiClient } from '@/lib/api-client';

type RegisterRequest = {
    emailAddress: string;
    password: string;
    firstName: string;
    lastName: string;
    dateOfBirth: string;
};

type RegisterResponse = {
    userId: string;
};

export const useRegister = () => {
    const queryClient = useQueryClient();

    return useMutation({
        mutationFn: (request: RegisterRequest) =>
            apiClient.post<RegisterResponse>('/account/register', request),
        onSuccess: () => queryClient.invalidateQueries({ queryKey: ['users'] }),
    });
};
