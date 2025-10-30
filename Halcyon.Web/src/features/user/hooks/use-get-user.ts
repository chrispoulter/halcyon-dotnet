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
    version: number;
};

export const useGetUser = (id: string) => {
    const { accessToken } = useAuth();

    return useQuery({
        queryKey: ['user', id],
        queryFn: ({ signal }) =>
            apiClient.get<GetUserResponse>(`/user/${id}`, signal, undefined, {
                Authorization: `Bearer ${accessToken}`,
            }),
    });
};
