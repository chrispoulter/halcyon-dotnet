import { useQuery } from '@tanstack/react-query';
import { useAuth } from '@/components/auth-provider';
import { apiClient } from '@/lib/api-client';

export type GetProfileResponse = {
    id: string;
    emailAddress: string;
    firstName: string;
    lastName: string;
    dateOfBirth: string;
    version: number;
};

export const useGetProfile = () => {
    const { accessToken } = useAuth();

    return useQuery({
        queryKey: ['profile'],
        queryFn: ({ signal }) =>
            apiClient.get<GetProfileResponse>('/profile', signal, undefined, {
                Authorization: `Bearer ${accessToken}`,
            }),
    });
};
