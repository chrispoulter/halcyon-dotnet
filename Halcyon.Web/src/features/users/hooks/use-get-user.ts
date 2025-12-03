import { useQuery } from '@tanstack/react-query';
import { useAuth } from '@/components/auth-provider';
import { apiClient } from '@/lib/api-client';
import type { Role } from '@/lib/session';

export type GetUserResponse = {
    id: string;
    emailAddress: string;
    firstName: string;
    lastName: string;
    dateOfBirth: string;
    isLockedOut?: boolean;
    roles?: Role[];
};

export const useGetUser = (id: string) => {
    const { accessToken } = useAuth();

    return useQuery({
        queryKey: ['user', id],
        queryFn: ({ signal }) =>
            apiClient
                .get(`/api/users/${id}`, {
                    headers: { Authorization: `Bearer ${accessToken}` },
                    signal,
                })
                .json<GetUserResponse>(),
    });
};
