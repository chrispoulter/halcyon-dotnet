import { useMutation, useQueryClient } from '@tanstack/react-query';
import { useAuth } from '@/components/auth-provider';
import { apiClient } from '@/lib/api-client';

type UpdateProfileRequest = {
    emailAddress: string;
    firstName: string;
    lastName: string;
    dateOfBirth: string;
    version?: number;
};

type UpdateProfileResponse = { id: string };

export const useUpdateProfile = () => {
    const { accessToken } = useAuth();

    const queryClient = useQueryClient();

    return useMutation({
        mutationFn: (request: UpdateProfileRequest) =>
            apiClient.put<UpdateProfileResponse>('/profile', request, {
                Authorization: `Bearer ${accessToken}`,
            }),
        onSuccess: (response) => {
            queryClient.invalidateQueries({ queryKey: ['profile'] });
            queryClient.invalidateQueries({ queryKey: ['users'] });
            queryClient.invalidateQueries({ queryKey: ['user', response.id] });
        },
    });
};
